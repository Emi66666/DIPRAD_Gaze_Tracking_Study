using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PictureCollider : MonoBehaviour
{
    public GameManager gameManager;
    private PhotonView _photonView;

    void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        gameManager = FindFirstObjectByType(typeof(GameManager)) as GameManager;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
        Debug.Log(collision.gameObject.layer);
        if (_photonView.IsMine && collision.gameObject.layer == 8)
        {
            gameManager.PlayerFoundPictureCollider();
            gameObject.SetActive(false);
        }
    }
}
