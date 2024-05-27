using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureCollider : MonoBehaviour
{
    public GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType(typeof(GameManager)) as GameManager;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            gameManager.PlayerFoundPictureCollider();
        }
    }
}
