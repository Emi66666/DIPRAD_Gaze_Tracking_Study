using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerInstantiator : MonoBehaviourPunCallbacks
{
    public GameObject player;
    public Timer timer;

    public GameObject[] waitingForPlayerTexts;
    public GameObject[] timerTexts;

    private PhotonView _photonView;

    private bool initialized = false;

    void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        player = PhotonNetwork.Instantiate("NetworkedPrefabs/Player", new Vector3(0, 0, 0), Quaternion.identity, 0);
        ExitGames.Client.Photon.Hashtable playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
        playerCustomProperties.Add("Loaded", PhotonNetwork.NickName);
        PhotonNetwork.SetPlayerCustomProperties(playerCustomProperties);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            foreach (var waitingForPlayerText in waitingForPlayerTexts)
            {
                waitingForPlayerText.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (!initialized && PhotonNetwork.IsMasterClient)
        {
            int loadedCount = 0;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.ContainsKey("Loaded"))
                {
                    loadedCount++;
                }
            }

            if (loadedCount == 2)
            {
                _photonView.RPC("StartTimer", RpcTarget.All);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (_photonView.IsMine && PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("StartTimer", RpcTarget.All);
        }
    }

    [PunRPC]
    public void StartTimer()
    {
        foreach (var waitingForPlayerText in waitingForPlayerTexts)
        {
            waitingForPlayerText.SetActive(false);
        }
        foreach (var timerText in timerTexts)
        {
            timerText.SetActive(true);
        }
        timer.StartTimer();
    }
}
