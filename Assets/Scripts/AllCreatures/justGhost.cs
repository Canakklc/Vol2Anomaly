using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class justGhost : MonoBehaviour
{
    public List<GameObject> roomLights = new List<GameObject>();
    public List<bool> canFlicker = new List<bool>();
    objectAnomalies takeIndex;
    public bool canCreateRandom = true;
    public float timerForFlick;
    int realTime; //int one 



    void Start()
    {

    }
    void Update()
    {

        ChooseRandomLamp();
        if (takeIndex.setCreatureType[0] == true)
        {
            timerForFlick += Time.deltaTime;
            realTime = Mathf.FloorToInt(timerForFlick);
            //Debug.Log("time=" + realTime);
        }
    }

    void Awake()
    {
        takeIndex = GetComponent<objectAnomalies>();
    }

    void ChooseRandomLamp()
    {
        if (takeIndex.setCreatureType[0] == true)
        {

            if (takeIndex.Index >= 0 && takeIndex.Index <= 10)
            {
                if (takeIndex.Index < 3)
                {
                    roomLights[0].SetActive(false);
                    Debug.Log("LightShouldOFF" + takeIndex.Index);
                }
                else if (takeIndex.Index >= 3 && takeIndex.Index > 7)
                {
                    roomLights[1].SetActive(false);
                    Debug.Log("SecShouldOff");
                }
                else
                {
                    roomLights[2].SetActive(false);
                    Debug.Log("thirthShouldOff");
                }
            }
        }
    }


}






