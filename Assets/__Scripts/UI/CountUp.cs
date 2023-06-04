using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountUp : MonoBehaviour
{
    public TMP_Text textBox;
    public float timeStart;
    public string finalTime;

    private bool timerActive = false;

    // Start is called before the first frame update
    void Start()
    {
        timeStart = 0;
        activateTimer();
    }

    // Update is called once per frame
    void Update()
    {

        if (timerActive)
        {
            timeStart += Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(timeStart);
        textBox.SetText(time.ToString(@"mm\:ss\:ff"));

    }

    public void activateTimer()
    {
        //start level
        timerActive = true;
    }

    public void deactivateTimer()
    {
        //player die some condition
        timerActive = false;
    }

    public string getCurrentTime()
    {
        TimeSpan time = TimeSpan.FromSeconds(timeStart);
        finalTime = time.ToString(@"mm\:ss\:ff");
        return finalTime;
    }
}
