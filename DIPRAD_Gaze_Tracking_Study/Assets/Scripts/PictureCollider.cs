using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PictureCollider : MonoBehaviour
{
    public GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType(typeof(GameManager)) as GameManager;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            gameManager.PlayerFoundPictureCollider();
            gameObject.SetActive(false);
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}
