using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    public Ray raycast;
    public RaycastHit rayCastInfo;
    public RectTransform cursorUI;


    void Update()
    {
        raycast = Camera.main.ScreenPointToRay(cursorUI.position);

        if (Physics.Raycast(raycast, out rayCastInfo))
        {
            if (Input.GetMouseButton(0))
            {
                Debug.Log(rayCastInfo.collider.name);

            }
        }



    }


}
