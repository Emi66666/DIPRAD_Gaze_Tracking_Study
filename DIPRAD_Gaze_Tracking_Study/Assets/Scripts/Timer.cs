using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class Timer : MonoBehaviour
{
    public float timer = 30.0f;
    private bool started = false;
    private bool finished = false;

    public GameObject[] explorationTimerTexts;
    public GameObject[] pictureTimerTexts;

    public TMP_Text[] timeTextFields;

    private PhotonView _photonView;

    public GameManager gameManager;

    bool exploration = true;

    void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    public void StartExplorationTimer()
    {
        foreach (var timerText in explorationTimerTexts)
        {
            timerText.SetActive(true);
        }
        started = true;
        finished = false;
        exploration = true;
    }

    public void StartPictureTimer(float time)
    {
        timer = time;
        foreach (var timerText in pictureTimerTexts)
        {
            timerText.SetActive(true);
        }
        started = true;
        finished = false;
    }

    void Update()
    {
        if (started)
        {
            if (!finished)
            {
                timer -= Time.deltaTime;

                if (timer <= 0.0f)
                {
                    if (exploration && PhotonNetwork.IsMasterClient)
                    {
                        _photonView.RPC("ExplorationEnded", RpcTarget.All);
                    }
                    else if (!exploration && !gameManager.pictureFound)
                    {
                        PictureNotFound();
                    }
                }
            }
            ShowOnGUI();
        }
    }

    void PictureNotFound()
    {
        finished = true;
        timer = 0.0f;
        gameManager.PictureNotFound();
    }

    void ShowOnGUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);

        string timerText = string.Format("{0:00}:{1:00}", minutes, seconds);

        foreach (var timeTextField in timeTextFields)
        {
            timeTextField.text = timerText;
        }
    }

    [PunRPC]
    public void ExplorationEnded()
    {
        finished = true;
        timer = 0.0f;
        exploration = false;

        gameManager.StartQuestions();
    }
}
