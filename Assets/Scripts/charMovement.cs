using System;
using UnityEngine;

public class charMovement : MonoBehaviour
{
    // Other Scripts
    PostProcesses camEffects;
    CCTVandCAM takeBool;
    Rigidbody rb;
    GuideBook bookBool;
    public Transform playerCam;

    [Header("Movement Settings")]
    public float sensivity = 3f;
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;

    [Header("Jump Settings")]
    public float jumpStrength = 20f;

    [Header("Stamina Settings")]
    public float staminaCounter;
    public bool canSprint;

    // Bools
    public bool canJump;
    public bool isRunning;

    public float xRotation = 0f;
    bool wasRunning = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        camEffects = GameObject.FindWithTag("Settings").GetComponent<PostProcesses>();
        takeBool = GameObject.FindWithTag("Settings").GetComponent<CCTVandCAM>();
        bookBool = GameObject.FindWithTag("Settings").GetComponent<GuideBook>();

    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (takeBool.onCCTV == false)
        {
            if (bookBool.canLook == true)
            {
                Look();
            }
            HandleJump();
            FallMultiplier();
            sprintStamina();
            staminaCounter = Mathf.Clamp(staminaCounter, 0f, 3f);
        }
    }

    void FixedUpdate()
    {
        if (takeBool.onCCTV == false)
        {
            Move();
        }
        bool sprintKey = Input.GetKey(KeyCode.LeftShift) && canSprint;


        if (sprintKey && !wasRunning)
        {
            if (takeBool.onCCTV == false && Input.GetKey(KeyCode.W) && bookBool.bookVisible == false)
            {
                camEffects.StartFOW();
            }
        }

        else if (!sprintKey && wasRunning)
        {
            if (takeBool.onCCTV == false && Input.GetKey(KeyCode.W) && bookBool.bookVisible == false)
            {
                camEffects.FinishFow();
            }
        }

        if (sprintKey)
        {
            if (takeBool.onCCTV == false && bookBool.bookVisible == false)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    Sprint();
                }
            }
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        wasRunning = isRunning;
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        rb.velocity = new Vector3(move.x * moveSpeed, rb.velocity.y, move.z * moveSpeed);
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        if (bookBool.onBook == false)
        {
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        }
        playerCam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void Sprint()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 run = transform.right * x + transform.forward * z;
        rb.velocity = new Vector3(run.x * sprintSpeed, rb.velocity.y, run.z * sprintSpeed);
    }

    void HandleJump()
    {
        if (canJump && Input.GetKey(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpStrength, rb.velocity.z);
            canJump = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canJump = true;
        }
    }

    void FallMultiplier()
    {
        float fallMultiplier = 1.2f;
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    void sprintStamina(float maxStaminaSec = 2.5f)
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
            staminaCounter += Time.deltaTime;
        else
            staminaCounter -= Time.deltaTime;

        canSprint = staminaCounter <= maxStaminaSec;
    }
}
