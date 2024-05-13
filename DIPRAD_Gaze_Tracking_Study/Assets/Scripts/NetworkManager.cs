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
    public GameObject connectingToServerText;
    public GameObject connectingToLobbyText;
    public GameObject connectingToRoomText;
    public GameObject waitingForPlayerText;
    public GameObject gameStartingText;

    public GameObject gameMenu;
    public Button startButton;

    public TMP_Dropdown mapDropdown;
    public TMP_Dropdown gamemodeDropdown;

    public GameObject player;

    void Start()
    {
        Debug.Log("Connecting to server...");
        connectingToServerText.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server.");
        connectingToServerText.SetActive(false);
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("Connecting to lobby...");
        connectingToLobbyText.SetActive(true);
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Connected to lobby.");
        connectingToLobbyText.SetActive(false);
        Debug.Log("Creating or joining new room...");
        connectingToRoomText.SetActive(true);
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions() { MaxPlayers = 2 }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Successfully joined the room.");
        connectingToRoomText.SetActive(false);
        player = PhotonNetwork.Instantiate("NetworkedPrefabs/Player", new Vector3(0, 0, 0), Quaternion.identity, 0);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            waitingForPlayerText.SetActive(true);
        }
        else
        {
            ShowGameMenu();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        waitingForPlayerText.SetActive(false);
        ShowGameMenu();
    }

    public void ShowGameMenu()
    {
        gameMenu.SetActive(true);
        if (!PhotonNetwork.IsMasterClient)
        {
            startButton.interactable = false;
            mapDropdown.interactable = false;
            gamemodeDropdown.interactable = false;
        }
    }

    public void StartGame()
    {
        Debug.Log("Game starting...");
        gameStartingText.SetActive(true);
        int mapValue = mapDropdown.value;
        int gamemodeValue = gamemodeDropdown.value;
        // TODO: Add to scriptable object

        PhotonNetwork.LoadLevel(mapValue + 1);
    }
}
