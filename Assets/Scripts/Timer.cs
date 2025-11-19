using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float liveTime;
    public TextMeshProUGUI showTimeHud;
    public Slider timeIndicator;
    string finisher = "Dawn";

    int liveTimeInt;


    void Update()
    {
        liveTime += Time.deltaTime;
        liveTimeInt = Mathf.FloorToInt(liveTime);
        timeIndicator.value = liveTimeInt;

        showTimeHud.text = liveTimeInt.ToString();

        TimeEnds();

    }
    void Start()
    {
        timeIndicator.minValue = 0;
        timeIndicator.maxValue = 61;
    }
    void TimeEnds()
    {
        if (liveTimeInt > 60)
        {
            showTimeHud.text = finisher;
            showTimeHud.fontSize = 15;
        }
    }

}
