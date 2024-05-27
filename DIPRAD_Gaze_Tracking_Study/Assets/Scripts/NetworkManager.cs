using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject[] connectingToServerTexts;
    public GameObject[] connectingToLobbyTexts;
    public GameObject[] connectingToRoomTexts;
    public GameObject[] waitingForPlayerTexts;

    public GameObject gamemodePicker;
    public GameObject startButton; 

    public GameObject ownerStartingText;
    public GameObject handMenuNetworkText;

    public GameObject player;

    void Start()
    {
        Debug.Log("Connecting to server...");

        foreach (var connectingToServerText in connectingToServerTexts)
        {
            connectingToServerText.SetActive(true);
        }

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server.");

        foreach (var connectingToServerText in connectingToServerTexts)
        {
            connectingToServerText.SetActive(false);
        }

        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("Connecting to lobby...");

        foreach (var connectingToLobbyText in connectingToLobbyTexts)
        {
            connectingToLobbyText.SetActive(true);
        }

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Connected to lobby.");

        foreach (var connectingToLobbyText in connectingToLobbyTexts)
        {
            connectingToLobbyText.SetActive(false);
        }

        Debug.Log("Creating or joining new room...");

        foreach (var connectingToRoomText in connectingToRoomTexts)
        {
            connectingToRoomText.SetActive(true);
        }

        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions() { MaxPlayers = 2 }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Successfully joined the room.");

        foreach (var connectingToRoomText in connectingToRoomTexts)
        {
            connectingToRoomText.SetActive(false);
        }

        player = PhotonNetwork.Instantiate("NetworkedPrefabs/Player", new Vector3(0, 0, 0), Quaternion.identity, 0);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            foreach (var waitingForPlayerText in waitingForPlayerTexts)
            {
                waitingForPlayerText.SetActive(true);
            }
        }
        else
        {
            ShowGameMenu();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        foreach (var waitingForPlayerText in waitingForPlayerTexts)
        {
            waitingForPlayerText.SetActive(false);
        }

        ShowGameMenu();
    }

    public void ShowGameMenu()
    {
        ownerStartingText.SetActive(true);
        gamemodePicker.SetActive(true);
        startButton.SetActive(true);

        if (!PhotonNetwork.IsMasterClient)
        {
            startButton.GetComponent<Button>().interactable = false;
        }
    }

    public void RemoveNetworkText()
    {
        Debug.Log("Game starting...");

        ownerStartingText.SetActive(false);
        startButton.SetActive(false);
        handMenuNetworkText.SetActive(false);
    }
}
