using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class CountUp : MonoBehaviour

{

    public TMP_Text textBox;
    public float timeStart;

    bool timerActive=false;
    // Start is called before the first frame update
    void Start()
    {

        textBox.text = timeStart.ToString("F2");

    }

    // Update is called once per frame
    void Update()
    {

        if (timerActive)
        {
            timeStart += Time.deltaTime;
            textBox.SetText(timeStart.ToString("F2"));
        }

    }

    public void timerButton() 
    {
    
        timerActive = !timerActive;

    }
}
