using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerSec : MonoBehaviour
{
    public Slider Slider;
    public TextMeshProUGUI Text;
    public int hourTime = 10;
    public float minuteTime = 00;
    string timeText;
    public bool stopTime = false;
    float totalPastSec;


    void Start()
    {
        Slider.minValue = 0;
        Slider.maxValue = 177;
    }

    void Update()
    {
        ClockLogic();
        string timeText = $"{hourTime:00}:{minuteTime:00}PM";

        if (stopTime == false)
        {
            minuteTime += Time.deltaTime;
            totalPastSec += Time.deltaTime;
        }

        if (hourTime >= 12 && minuteTime > 59)
        {
            timeText = "01:00AM";
            stopTime = true;

        }

        Text.text = timeText;
        Slider.value = totalPastSec;
    }

    void ClockLogic()
    {
        if (minuteTime > 59 && stopTime == false)
        {
            hourTime += 1;
            minuteTime = 0;
        }

    }


}
