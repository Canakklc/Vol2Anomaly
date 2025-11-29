using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectAnomalies : MonoBehaviour
{
    public List<GameObject> objectCanFall = new List<GameObject>();
    public List<Transform> whereToFall = new List<Transform>();
    public List<Vector3> originalObjPos = new List<Vector3>();
    public List<Quaternion> originalRots = new List<Quaternion>();
    public List<bool> boolsToCutAction = new List<bool>();
    public int Index;
    public int maxValue = 100;
    public bool canCreateNewRandom = true;
    /// <summary>
    /// Setting the ghost type from the beginning
    /// </summary>
    public List<bool> setCreatureType = new List<bool>();//1- ghost 2-Poltergeist 3-Demon
    public int determineCreatureInt;

    void Start()
    {
        for (int i = 0; i < objectCanFall.Count; i++)
        {
            originalObjPos.Add(objectCanFall[i].transform.position);
            originalRots.Add(objectCanFall[i].transform.rotation);
            boolsToCutAction.Add(false);
        }
        determineCreatureInt = UnityEngine.Random.Range(1, 4);
        for (int i = 0; i < 3; i++)
        {
            setCreatureType.Add(false);
        }
        if (determineCreatureInt == 1)
        {
            setCreatureType[0] = true; //means ghost active
        }

    }
    void Update()
    {

    }
    IEnumerator Possibility()
    {
        Index = UnityEngine.Random.Range(0, maxValue);
        if (Index >= 0 && Index <= 10)
        {
            Debug.Log(Index);
            if (Index < 3 && boolsToCutAction[0] == false) //book first in list //put checker here!!!(ghost type)
            {
                canCreateNewRandom = false;
                objectCanFall[0].transform.position = whereToFall[0].transform.position;
                objectCanFall[0].transform.rotation = Quaternion.Euler(-24.721f, 0, 0);
                boolsToCutAction[0] = true;
                Debug.Log("Book pos changed");
            }
            else if (Index >= 3 && Index < 7 && boolsToCutAction[1] == false)//paper cond;
            {
                canCreateNewRandom = false;
                objectCanFall[1].transform.position = whereToFall[1].transform.position;
                objectCanFall[1].transform.rotation = Quaternion.Euler(0, 180, 0);
                boolsToCutAction[1] = true;

                Debug.Log("Paper pos changed");
            }
            else //kettle
            {
                if (boolsToCutAction[2] == false)
                {
                    canCreateNewRandom = false;
                    objectCanFall[2].transform.position = whereToFall[2].transform.position;
                    objectCanFall[2].transform.rotation = Quaternion.Euler(2.401f, -5.953f, -95.849f);
                    Debug.Log("Kettle moved");
                    boolsToCutAction[2] = true;
                }
            }
            yield return new WaitForSeconds(2f);
            canCreateNewRandom = true;
        }
        else if (Index > 10)
        {
            {
                canCreateNewRandom = false;
                Debug.Log("out of range" + Index);
                yield return new WaitForSeconds(2f);
                canCreateNewRandom = true;
            }
        }

    }
    public void StartPossibilities()
    {
        if (canCreateNewRandom == true)
        {
            StartCoroutine(Possibility());
        }
    }

}
