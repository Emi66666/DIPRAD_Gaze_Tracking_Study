using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class GameManager : MonoBehaviour
{
    public GameObject startAreaColliders;

    private PhotonView _photonView;
    public NetworkManager networkManager;
    public Timer timer;

    public List<RawImage> pictures;
    public List<GameObject> pictureColliders;

    public GameObject OVRCameraRig;
    public Transform spawnPoint;

    public RawImage handMenuImage;

    void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RemoveNetworkText", RpcTarget.All);
            _photonView.RPC("DeactivateStartAreaColliders", RpcTarget.All);
            _photonView.RPC("StartTimer", RpcTarget.All);
        }
    }

    public void NextPicture()
    {
        startAreaColliders.SetActive(true);
        OVRCameraRig.transform.position = spawnPoint.position;

        int pictureNumber = pictures.Count;
        int randomNumber = Random.Range(0, pictureNumber - 1);

        RawImage picture = pictures[randomNumber];
        pictures.Remove(picture);

        GameObject pictureCollider = pictureColliders[randomNumber];
        pictureColliders.Remove(pictureCollider);

        handMenuImage.texture = picture.texture;
        pictureCollider.SetActive(true);
    }

    [PunRPC]
    public void RemoveNetworkText()
    {
        networkManager.RemoveNetworkText();
    }

    [PunRPC]
    public void StartTimer()
    {
        timer.StartTimer();
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
