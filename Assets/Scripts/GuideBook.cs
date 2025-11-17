using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GuideBook : MonoBehaviour
{
    public GameObject Player;
    public Transform bookPlace;
    public GameObject guideBook;
    private Vector3 initalBookPos;
    [Header("Bools")]
    public bool bookVisible = false;



    void Start()
    {
        initalBookPos = guideBook.transform.localPosition;
        guideBook.SetActive(false);

        ////pages////
        for (int i = 0; i < Pages.Count; i++)
        {
            startRot.Add(Pages[i].localRotation);
            endRot.Add(Pages[i].localRotation * Quaternion.Euler(0, 169.62f, 0));
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartResetPages();
            currentIndex = 0;
            StartBookVis();

        }
        if (Input.GetKeyDown(KeyCode.E) && bookVisible == true)
        {
            if (currentIndex <= Pages.Count)
            {
                StartMovePages();
                currentIndex++;
            }
            else { return; }
        }
        if (Input.GetKeyDown(KeyCode.Q) && bookVisible == true)
        {
            if (currentIndex <= Pages.Count)
            {
                MovePagesBack();
                currentIndex--;
            }
            else { return; }
        }
    }

    IEnumerator BookVisiblity()
    {
        bookVisible = !bookVisible;

        if (bookVisible)
        {
            guideBook.SetActive(true);
            StartCoroutine(CarryBook(true));
        }
        else
        {
            StartCoroutine(CarryBook(false));
            yield return new WaitForSeconds(1f);
            guideBook.SetActive(false);

        }

    }
    void StartBookVis()
    {
        StartCoroutine(BookVisiblity());
    }
    IEnumerator CarryBook(bool Opening)
    {
        Vector3 startPos = Opening ? initalBookPos : bookPlace.localPosition;
        Vector3 endPos = Opening ? bookPlace.localPosition : initalBookPos;
        float elapsed = 0;
        float Duration = 1f;
        if (guideBook == null) yield break;
        while (elapsed < Duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / Duration;
            t = Mathf.SmoothStep(0f, 1f, t);
            guideBook.transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;

        }
        guideBook.transform.localPosition = endPos;

    }

    ////PageMech////

    [Header("Page Section")]
    public List<Transform> Pages = new List<Transform>();
    public List<Quaternion> startRot = new List<Quaternion>();
    public List<Quaternion> endRot = new List<Quaternion>();

    public int currentIndex = 0;

    IEnumerator MovePages(int index, bool front)
    {
        Quaternion startDir = front ? startRot[index] : endRot[index];
        Quaternion endDir = front ? endRot[index] : startRot[index];

        float elapsed = 0;
        float Duration = 1f;
        while (elapsed < Duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / Duration;
            t = Mathf.SmoothStep(0f, 1f, t);
            Pages[index].transform.localRotation = Quaternion.Slerp(startDir, endDir, t);
            yield return null;

        }

    }
    void StartMovePages()
    {
        StartCoroutine(MovePages(currentIndex, true));
    }
    void MovePagesBack()
    {
        StartCoroutine(MovePages(currentIndex, false));
        Debug.Log("PAGES RE MOVIGN BACK");
    }
    IEnumerator ResetPages()
    {
        for (int i = 0; i < Pages.Count; i++)
        {
            yield return new WaitForSeconds(1f);
            Pages[i].transform.localRotation = startRot[i];
        }
    }
    void StartResetPages()
    {
        StartCoroutine(ResetPages());
    }




}

