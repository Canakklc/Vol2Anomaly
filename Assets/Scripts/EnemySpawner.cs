using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();
    public List<int> Numbers = new List<int>();
    public List<GameObject> Monsters = new List<GameObject>();


    int Index;
    int Limit = 101;
    [Header("Bools")]
    public bool canSpawnEnemy = false;
    public bool canSpawnAtFirst = true;
    public bool canSpawnAtSec = true;
    public Transform currentPos;

    void Start()
    {
        for (int i = 0; i < Limit; i++)
        {
            Numbers.Add(i);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            PickEvent();
        }
    }

    public void PickEvent()
    {
        Index = UnityEngine.Random.Range(0, Numbers.Count);
        Debug.Log(Index);
        canSpawnEnemy = true;
        SpawnPosChoose();
    }

    public void SpawnPosChoose()
    {
        if (Index > 0 && Index <= 10)
        {
            if (Index < 6 && canSpawnAtFirst == true)
            {
                currentPos = spawnPoints[0];
                InstantiateGhost();
                canSpawnAtFirst = false;
            }
            else if (canSpawnAtSec == true)
            {
                currentPos = spawnPoints[1];
                InstantiateGhost();
                canSpawnAtSec = false;
            }

        }
        else
        {
            Debug.Log("out of range");
            return;
        }

    }
    public void InstantiateGhost()
    {

        if (currentPos == null)
        {
            Debug.Log("currentPose is null");
            return;
        }
        else if (canSpawnEnemy == true)
        {
            Instantiate(Monsters[0], currentPos.position, currentPos.rotation);
            Debug.Log("EnemySpawned");
        }
        canSpawnEnemy = false;
        //currentPos = null;

    }



}

