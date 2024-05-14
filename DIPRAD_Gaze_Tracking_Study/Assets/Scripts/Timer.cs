using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timer = 30.0f;
    private bool started = false;
    private bool finished = false;

    public TMP_Text[] timeTextFields;

    public void StartTimer()
    {
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
                TimerEnded();
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

    void TimerEnded()
    {
        finished = true;
        timer = 0.0f;
    }
}
