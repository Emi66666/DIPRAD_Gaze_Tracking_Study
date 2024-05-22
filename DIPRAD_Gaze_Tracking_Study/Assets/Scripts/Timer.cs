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

    public GameObject OVRCameraRig;
    public Transform spawnPoint;

    public GameObject handMenuTimerText;

    public TMP_Text[] timeTextFields;

    private PhotonView _photonView;

    void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void StartTimer()
    {
        handMenuTimerText.SetActive(true);
        started = true;
    }

    void Update()
    {
        if (started)
        {
            if (!finished)
            {
                timer -= Time.deltaTime;
            }

            ShowOnGUI();

            if (timer <= 0.0f)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    _photonView.RPC("TimerEnded", RpcTarget.All);
                }
            }
        }
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
    public void TimerEnded()
    {
        Debug.Log("Timer Ended called");
        finished = true;
        timer = 0.0f;

        OVRCameraRig.transform.position = spawnPoint.position;
        // TODO: Teleport player back after timer expires
    }
}
