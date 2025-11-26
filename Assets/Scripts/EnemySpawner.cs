using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> spawnPoses = new List<Transform>();
    public List<int> numbers = new List<int>();
    public GameObject Ghost;
    public GameObject Player;
    public bool canInstantiate = true;
    int index;
    int choosenNum;



    void Start()
    {
        for (int i = 0; i <= 100; i++)
        {
            numbers.Add(i);
        }
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.G))
        {
            JackPot();
            GhostInstantiate();
        }
    }

    void GhostInstantiate()
    {

        if (canInstantiate == true)
        {
            var booling = choosenNum < 10;
            if (booling)
            {
                index = UnityEngine.Random.Range(0, spawnPoses.Count);
                GameObject newGhost = Instantiate(Ghost, spawnPoses[index].transform.position, spawnPoses[index].transform.rotation);
                canInstantiate = false;
                Vector3 dir = Player.transform.position - Ghost.transform.position;
                newGhost.transform.LookAt(dir);
            }
        }
    }
    void JackPot()
    {
        choosenNum = UnityEngine.Random.Range(0, numbers.Count);
        if (choosenNum < 10)
        {
            Debug.Log("spawn should work" + choosenNum);
        }
        else
        {
            Debug.Log("missed" + choosenNum);
        }
    }
}

