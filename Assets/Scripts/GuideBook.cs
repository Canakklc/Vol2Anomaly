using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class GuideBook : MonoBehaviour
{
    //other scripts
    charMovement handleRot;
    public GameObject Player;
    public Transform bookPlace;
    public GameObject guideBook;
    private Vector3 initalBookPos;
    [Header("Bools")]
    public bool bookVisible = false;
    Quaternion targetRot;
    Coroutine limitRoutine;
    public bool onBook = false;
    public bool canLook = true;




    void Awake()
    {
        handleRot = GameObject.FindWithTag("Player").GetComponent<charMovement>();
    }


    void Start()
    {
        initalBookPos = guideBook.transform.localPosition;
        guideBook.SetActive(false);

        ////pages////
        for (int i = 0; i < Pages.Count; i++)
        {
            startRot.Add(Pages[i].localRotation);
            endRot.Add(Pages[i].localRotation * Quaternion.Euler(0, 169.62f, 0));

            siblings.Add(Pages[i].GetSiblingIndex());

        }
    }
    void Update()
    {
        limitation();

        if (Input.GetKeyDown(KeyCode.B))
        {
            canFlipPages = true;
            StartResetPages();
            currentIndex = 0;
            StartBookVis();

        }
        if (Input.GetKeyDown(KeyCode.E) && bookVisible == true)
        {
            if (currentIndex < Pages.Count && canFlipPages == true)
            {
                canFlipPages = false;
                StartMovePages();
                currentIndex++;
            }
            else { return; }
        }
        if (Input.GetKeyDown(KeyCode.Q) && bookVisible == true && canFlipPages == true)
        {
            canFlipPages = false;
            if (currentIndex > 0)
            {
                MovePagesBack();
                currentIndex--;
            }
            else { return; }
        }
        if (bookVisible == true)
        {
            if (limitRoutine == null)
            {
                onBook = true;
                limitRoutine = StartCoroutine(limitLook());

            }

        }
        else if (limitRoutine != null)
        {
            onBook = false;
            StopCoroutine(limitRoutine);
            limitRoutine = null;
        }

        IEnumerator limitLook()
        {
            float duration = 0.7f;
            float elapsed = 0f;


            handleRot.xRotation = Mathf.Clamp(handleRot.xRotation, -15f, 15f);

            Quaternion startRot = handleRot.playerCam.localRotation;
            targetRot = Quaternion.Euler(handleRot.xRotation, 0f, 0f);


            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                t = Mathf.SmoothStep(0f, 1f, t);
                canLook = false;
                handleRot.playerCam.localRotation = Quaternion.Slerp(startRot, targetRot, t);

                yield return null;
            }
            canLook = true;
            handleRot.playerCam.localRotation = targetRot;

        }
        void limitation()
        {
            if (onBook == true)
            {
                handleRot.xRotation = Mathf.Clamp(handleRot.xRotation, -15f, 15f);
                handleRot.playerCam.localRotation = Quaternion.Euler(handleRot.xRotation, 0f, 0f);
            }
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
    private List<int> siblings = new List<int>();

    public int currentIndex = 0;
    public bool canFlipPages = true;

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
        canFlipPages = true;

    }
    void StartMovePages()
    {
        if (currentIndex < 0 || currentIndex >= Pages.Count) return;
        Pages[currentIndex].SetAsLastSibling();
        StartCoroutine(MovePages(currentIndex, true));

    }
    void MovePagesBack()
    {
        int backIndex = currentIndex - 1;
        if (backIndex < 0) return;
        Pages[backIndex].SetAsLastSibling();
        StartCoroutine(MovePages(backIndex, false));
    }
    IEnumerator ResetPages()
    {
        for (int i = 0; i < Pages.Count; i++)
        {
            yield return new WaitForSeconds(0.5f);
            Pages[i].transform.localRotation = startRot[i];
            Pages[i].SetSiblingIndex(siblings[i]);
        }
    }
    void StartResetPages()
    {
        StartCoroutine(ResetPages());
    }




}

