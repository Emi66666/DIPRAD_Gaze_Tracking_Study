using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using Unity.VisualScripting;

[RequireComponent(typeof(PhotonView))]
public class GameManager : MonoBehaviour
{
    public GameObject startAreaColliders;

    private PhotonView _photonView;
    public NetworkManager networkManager;
    public Timer timer;

    public TMP_Dropdown gamemodePicker;

    public List<RawImage> pictures;
    private List<RawImage> picturesCopy;
    public List<GameObject> pictureColliders;

    public GameObject OVRCameraRig;
    public Transform spawnPoint;

    public GameObject handMenuPictureText;
    public RawImage handMenuPicture;

    private string fileName;
    private string path;
    private StreamWriter sr;

    public Gamemode gamemode;

    public GameObject[] exploreTimerTexts;
    public RawImage[] menuPictures;
    public GameObject[] pictureMenus;
    public GameObject[] questions;
    public GameObject[] otherPlayerAnswers;
    public GameObject[] otherPlayerWaitingTexts;

    public Scrollbar[] answers1;
    public Scrollbar[] answers2;

    private int answersSubmitted;
    private int pictureCollidersFound;

    public GameObject[] pictureTimerTexts;

    public float timeToFindPicture = 60f;

    public TMP_Text[] points;
    public GameObject[] gameEndMenus;
    public TMP_Text[] finalPoints;

    private int playerOnePoints;
    private int playerTwoPoints;
    private int togetherPlayerPoints;

    public bool pictureFound;

    private GameObject lastPictureCollider;

    void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        fileName = "/GazeTracking_" + System.DateTime.Now.ToString("MM-dd-yy_hh-mm-ss") + ".txt";
        path = Application.persistentDataPath + fileName;
        sr = File.CreateText(path);

        picturesCopy = new List<RawImage>(pictures);
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RemoveNetworkText", RpcTarget.All);
            _photonView.RPC("DeactivateStartAreaColliders", RpcTarget.All);
            _photonView.RPC("StartExplorationTimer", RpcTarget.All);
            _photonView.RPC("SetGamemode", RpcTarget.All, (Gamemode) gamemodePicker.value);
        }
    }

    [PunRPC]
    public void SetGamemode(Gamemode gamemode)
    {
        this.gamemode = gamemode;
        sr.WriteLine("Gamemode: " + gamemode);
    }

    public void StartQuestions()
    {
        foreach (var timerText in exploreTimerTexts)
        {
            timerText.SetActive(false);
        }
        foreach (var pictureMenu in pictureMenus)
        {
            pictureMenu.SetActive(true);
        }

        handMenuPictureText.SetActive(true);

        NextPictureNumber();
    }

    public void NextPictureNumber()
    {
        Debug.Log("[TEST] Next Picture Number");

        timer.PauseTimer();
        foreach (var otherPlayerWaitingText in otherPlayerWaitingTexts)
        {
            otherPlayerWaitingText.SetActive(false);
        }
        if (PhotonNetwork.IsMasterClient)
        {
            int pictureNumber = pictures.Count;
            int randomNumber = Random.Range(0, pictureNumber - 1);
            _photonView.RPC("NextPicture", RpcTarget.All, randomNumber);
        }
    }

    [PunRPC]
    public void NextPicture(int randomNumber)
    {
        Debug.Log("[TEST] Next picture");
        foreach (var pictureTimerText in pictureTimerTexts)
        {
            pictureTimerText.SetActive(false);
        }

        answersSubmitted = 0;
        pictureCollidersFound = 0;
        pictureFound = false;

        if (PhotonNetwork.IsMasterClient)
        {
            questions[0].SetActive(true);
            otherPlayerAnswers[0].SetActive(false);
            otherPlayerAnswers[1].SetActive(true);
        }
        else
        {
            questions[1].SetActive(true);
            otherPlayerAnswers[1].SetActive(false);
            otherPlayerAnswers[0].SetActive(true);
        }

        startAreaColliders.SetActive(true);
        OVRCameraRig.transform.position = spawnPoint.position;

        foreach (var answer in answers1)
        {
            answer.value = 0f;
        }
        foreach (var answer in answers2)
        {
            answer.value = 0f;
        }

        RawImage picture = pictures[randomNumber];
        pictures.Remove(picture);

        GameObject pictureCollider = pictureColliders[randomNumber];
        pictureColliders.Remove(pictureCollider);

        lastPictureCollider = pictureCollider.gameObject;

        handMenuPicture.texture = picture.texture;
        foreach (var menuPicture in menuPictures)
        {
            menuPicture.texture = picture.texture;
        }
        
        pictureCollider.GetComponent<MeshRenderer>().enabled = true;
        pictureCollider.GetComponent<BoxCollider>().enabled = true;

        sr.WriteLine();
        sr.WriteLine("Picture " + (picturesCopy.IndexOf(picture) + 1) + ":");
    }

    public void SubmitAnswers()
    {
        sr.WriteLine("I can remember the place on the picture very well: ");
        if (PhotonNetwork.IsMasterClient)
        {
            sr.WriteLine(answers1[0].value * 4 + 1);
        }
        else
        {
            sr.WriteLine(answers1[1].value * 4 + 1);
        }

        sr.WriteLine("I could find my way back to the place on the picture: ");
        if (PhotonNetwork.IsMasterClient)
        {
            sr.WriteLine(answers2[0].value * 4 + 1);
        }
        else
        {
            sr.WriteLine(answers2[1].value * 4 + 1);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            questions[0].SetActive(false);
            otherPlayerAnswers[0].SetActive(true);
        }
        else
        {
            questions[1].SetActive(false);
            otherPlayerAnswers[1].SetActive(true);
        }
        _photonView.RPC("AnswerSubmitted", RpcTarget.All);
    }

    [PunRPC]
    public void AnswerSubmitted()
    {
        answersSubmitted++;

        if (answersSubmitted == 2)
        {
            FindPictureCollider();
        }
    }

    public void FindPictureCollider()
    {
        Debug.Log("[TEST] Find Picture Collider");
        foreach (var otherPlayerAnswer in otherPlayerAnswers)
        {
            otherPlayerAnswer.SetActive(false);
        }
        startAreaColliders.SetActive(false);

        foreach (var pictureTimerText in pictureTimerTexts)
        {
            pictureTimerText.SetActive(true);
        }
        timer.StartPictureTimer(timeToFindPicture);
    }

    public void PictureNotFound()
    {
        Debug.Log("[TEST] Picture not found in GameManager");

        lastPictureCollider.SetActive(false);

        foreach (var pictureTimerText in pictureTimerTexts)
        {
            pictureTimerText.SetActive(false);
        }

        sr.WriteLine("Time: " + timeToFindPicture + " (Not found)");
        sr.WriteLine("Points: " + 0f);

        _photonView.RPC("PictureColliderFound", RpcTarget.All, 0);
    }

    void EndGame()
    {
        Debug.Log("[TEST] End game");

        timer.PauseTimer();
        OVRCameraRig.transform.position = spawnPoint.position;

        foreach (var pictureMenu in pictureMenus)
        {
            pictureMenu.SetActive(false);
        }
        foreach (var gameEndMenu in gameEndMenus)
        {
            gameEndMenu.SetActive(true);
        }
        foreach (var otherPlayerWaitingText in otherPlayerWaitingTexts)
        {
            otherPlayerWaitingText.SetActive(false);
        }

        finalPoints[0].text = playerOnePoints.ToString();
        finalPoints[1].text = playerTwoPoints.ToString();

        if (PhotonNetwork.IsMasterClient)
        {
            sr.WriteLine();
            sr.WriteLine("Total points: " + playerOnePoints);
        }
        else
        {
            sr.WriteLine();
            sr.WriteLine("Total points: " + playerTwoPoints);
        }

        sr.Close();
    }

    public void PlayerFoundPictureCollider()
    {
        Debug.Log("[TEST] Player found picture collider");

        startAreaColliders.SetActive(true);
        OVRCameraRig.transform.position = spawnPoint.position;

        foreach (var pictureTimerText in pictureTimerTexts)
        {
            pictureTimerText.SetActive(false);
        }
        foreach (var otherPlayerWaitingText in otherPlayerWaitingTexts)
        {
            otherPlayerWaitingText.SetActive(true);
        }

        pictureFound = true;

        int pointsAwarded = (int) (timer.timer / timeToFindPicture * 1000);
        if (gamemode == Gamemode.COLLABORATIVE)
        {
            pointsAwarded /= 2;
        }

        _photonView.RPC("PictureColliderFound", RpcTarget.All, pointsAwarded, PhotonNetwork.IsMasterClient);
        
        sr.WriteLine("Time: " + (timeToFindPicture - timer.timer));
        sr.WriteLine("Points: " + pointsAwarded);
    }

    [PunRPC]
    public void PictureColliderFound(int pointsAwarded, bool isMasterClient)
    {
        Debug.Log("[TEST] Picture collider found");

        pictureCollidersFound++;

        if (gamemode == Gamemode.COMPETITIVE)
        {
            if (isMasterClient)
            {
                playerOnePoints += pointsAwarded;
            }
            else
            {
                playerTwoPoints += pointsAwarded;
            }
        }
        else
        {
            togetherPlayerPoints += pointsAwarded;
            playerOnePoints = togetherPlayerPoints;
            playerTwoPoints = togetherPlayerPoints;
        }

        UpdatePlayerPoints();

        if (pictureCollidersFound == 2)
        {
            foreach (var otherPlayerWaitingText in otherPlayerWaitingTexts)
            {
                otherPlayerWaitingText.SetActive(false);
            }

            if (pictures.Count > 0)
            {
                NextPictureNumber();
            }
            else
            {
                EndGame();
            }
        }
    }

    public void UpdatePlayerPoints()
    {
        points[0].text = playerOnePoints.ToString();
        points[1].text = playerTwoPoints.ToString();
    }

    [PunRPC]
    public void RemoveNetworkText()
    {
        networkManager.RemoveNetworkText();
    }

    [PunRPC]
    public void StartExplorationTimer()
    {
        timer.StartExplorationTimer();
    }

    [PunRPC]
    public void ActivateStartAreaColliders()
    {
        startAreaColliders.SetActive(true);
    }

    [PunRPC]
    public void DeactivateStartAreaColliders()
    {
        startAreaColliders.SetActive(false);
    }
}
