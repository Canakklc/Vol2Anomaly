using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCTVandCAM : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject camUnit;
    //other scirpts;
    CamPostProcess takeEffects;
    [Header("values")]
    public float distanceToCCTV;
    [Header("bools")]
    public bool onCCTV;
    public bool canEnterCCTV;
    public bool canExitCCTV;
    public Button nextCam;
    public Button exitButton;
    charMovement charMove;
    objectAnomalies objectFall;
    Raycast takeRay;
    public List<Camera> allCams = new List<Camera>();
    void Start()
    {
        canEnterCCTV = true;
        onMainCam = true;
        nextCam.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
    }
    void Awake()
    {
        charMove = GameObject.FindWithTag("Player").GetComponent<charMovement>();
        takeRay = GetComponent<Raycast>();
        mainCam = Camera.main;
        takeEffects = GameObject.FindWithTag("Settings").GetComponent<CamPostProcess>();
        objectFall = GetComponent<objectAnomalies>();
    }
    void Update()
    {
        CalculateDistanceCCTV();
        ControlCCTV();
        //CameraLogic();

    }

    void CalculateDistanceCCTV()
    {
        distanceToCCTV = Vector3.Distance(charMove.playerCam.position, camUnit.transform.position);

    }
    void ControlCCTV()
    {
        var hit = takeRay.rayCastInfo.collider?.name == "CCTV";
        var keyButton = Input.GetKeyDown(KeyCode.E);
        if (hit && distanceToCCTV <= 3 && keyButton && canEnterCCTV == true)
        {
            onCCTV = true;
            canExitCCTV = true;
            canEnterCCTV = false;
            nextCam.gameObject.SetActive(true);
            exitButton.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            StartCoroutine(CameraLogic());

        }
        else if (canExitCCTV == true && keyButton)
        {
            onCCTV = false;
            canExitCCTV = false;
            canEnterCCTV = true;
            nextCam.gameObject.SetActive(false);
            exitButton.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            ExitButton();
        }

    }

    //
    //CamLogicStats
    //
    [Header("CAMERALOGIC")]
    public Camera mainCam;
    [Header("bools")]
    public bool canTriggerRepeat;
    public bool onMainCam;
    public bool triggerFortThirth;
    public bool canActive = true;
    [Header("others")]
    public Camera activeCam;
    public Camera previousCam;

    public void ExitButton()
    {
        for (int i = 0; i < allCams.Count; i++)
        {
            allCams[i].depth = -2;
            onMainCam = true;
            onCCTV = false;
            canExitCCTV = false;
            canEnterCCTV = true;
            nextCam.gameObject.SetActive(false);
            exitButton.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    IEnumerator CameraLogic()
    {
        if (onMainCam == true && canActive == true)
        {
            canActive = false;
            takeEffects.ResetAllVals();
            takeEffects.StartingCoro();
            allCams[0].depth = 0;
            onMainCam = false;
            Debug.Log("FirstOpen");
            yield return new WaitForSeconds(2f);
            canActive = true;

        }
        else if (allCams[3].depth == 0 && canTriggerRepeat == true && onMainCam == false && canActive == true)
        {
            canActive = false;
            takeEffects.ResetAllVals();
            takeEffects.StartingCoro();
            allCams[1].depth = -2;
            allCams[0].depth = 0;
            canTriggerRepeat = false;
            Debug.Log("back to first");
            yield return new WaitForSeconds(2f);
            canActive = true;
        }

        else if (onMainCam == false && allCams[0].depth == 0 && canActive == true)
        {
            canActive = false;
            takeEffects.ResetAllVals();
            takeEffects.StartingCoro();
            allCams[0].depth = -2;
            allCams[1].depth = 0;
            Debug.Log("Cam Second");
            triggerFortThirth = true;
            yield return new WaitForSeconds(2f);
            canActive = true;
        }
        else if (onMainCam == false && allCams[1].depth == 0 && triggerFortThirth == true && canActive == true)
        {
            canActive = false;
            takeEffects.ResetAllVals();
            takeEffects.StartingCoro();
            allCams[1].depth = -2;
            allCams[2].depth = 0;
            triggerFortThirth = false;
            Debug.Log("on Thirth");
            yield return new WaitForSeconds(2f);
            canActive = true;
        }
        else if (onMainCam == false && allCams[2].depth == 0 && canActive == true)
        {
            canActive = false;
            takeEffects.ResetAllVals();
            takeEffects.StartingCoro();
            allCams[2].depth = -2;
            allCams[3].depth = 0;
            canTriggerRepeat = true;
            Debug.Log("onFourth");
            yield return new WaitForSeconds(2f);
            canActive = true;
        }

    }
    public void TriggerCamLogic()
    {
        StartCoroutine(CameraLogic());
        objectFall.StartPossibilities();

    }




}
