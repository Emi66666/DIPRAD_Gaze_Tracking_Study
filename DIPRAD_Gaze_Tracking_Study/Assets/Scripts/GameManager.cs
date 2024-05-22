using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class GameManager : MonoBehaviour
{
    public GameObject startAreaColliders;

    private PhotonView _photonView;

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
