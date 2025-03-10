using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2.5f;
    public float crouchSpeed = 1.5f;
    public float gravity = 9.8f;
    public float runspeed = 5f;
    public bool canmove = true;
    public bool isrunning = false;
    public float stamina = 25f;
    public float staminaDrain = 1f;
    public float staminaRegen = 1f;


    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public Transform playerCamera;
    private float verticalRotation = 0f;

    [Header("Crouch Settings")]
    public float crouchCameraHeight = 0.5f; // Lowered camera height when crouching
    public float standingCameraHeight = 1.5f; // Default camera height
    public float crouchTransitionSpeed = 8f;
    private CharacterController characterController;
    private Coroutine crouchRoutine;
    private bool userCrouching = false; // Tracks manual crouch state

    public bool isCrouching = false;
    public bool isHidden = false;
    public bool isMoving = false;

    [Header("Fear Settings")]
    public float fear;
    public bool isSpotted = false;

    [Header("Headbob Settings")]
    public float bobFrequency = 10f;
    public float crouchBobFrequency = 5f;
    public float bobAmount = 0.05f;
    private float headbobTimer = 0;
    private Vector3 cameraStartPos;
    private bool footstepPlayed = false;

    [Header("Footstep Sounds")]
    public AudioClip[] footstepSounds;
    public AudioSource footstepAudioSource;

    [Header("Audio")]
    public AudioSource heartbeat;
    public AudioSource chaseMusic;

    private Vector3 moveDirection;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraStartPos = playerCamera.localPosition;
        Cursor.lockState = CursorLockMode.Locked;
        canmove = false;
    }

    void Update()
    {
        if (!canmove)
            return;

        HandleLook();
        HandleCrouch();
        Fear();
    }

    private void FixedUpdate()
    {
        if (!canmove)
            return;
        HandleMovement();
        ApplyHeadbob();
    }

    void Fear()
    {
        if (isSpotted && fear < 10f)
        {
            fear += 3.5f * Time.deltaTime;
        }
        if (!isSpotted && fear > 0f)
        {
            fear -= 2.5f * Time.deltaTime;
        }
        heartbeat.volume = fear / 10;
        chaseMusic.volume = fear / 10;
        if (!isSpotted && fear < 0.1f)
        {
            chaseMusic.volume = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("table"))
        {
            isHidden = true;
            if (crouchRoutine != null) StopCoroutine(crouchRoutine);
            crouchRoutine = StartCoroutine(CrouchTransition(true)); // Force crouch
        }
        else if (other.CompareTag("closet"))
        {
            isHidden = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("table"))
        {
            isHidden = false;
            if (crouchRoutine != null) StopCoroutine(crouchRoutine);
            crouchRoutine = StartCoroutine(CrouchTransition(userCrouching)); // Restore previous state
        }
        else if (other.CompareTag("closet"))
        {
            isHidden = false;
        }
    }


    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        if (characterController.velocity.magnitude > 0.15f && !isCrouching)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && stamina > 0.1f)
        {
            isrunning = true;
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {
            isrunning = false;
        }

        float speed = isCrouching ? crouchSpeed : walkSpeed;

        if(!isCrouching)
            speed = isrunning ? runspeed : walkSpeed;

        HandleStamina();
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");


        Vector3 move = (transform.right * moveX + transform.forward * moveZ).normalized * speed * Time.deltaTime;

        if (characterController.isGrounded)
        {
            moveDirection.y = 0f;
            moveDirection = move;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection);
    }

    void HandleStamina()
    {
        if (isrunning)
        {
            stamina -= staminaDrain * Time.deltaTime;
        }
        else if (!isrunning)
        {
            stamina += staminaRegen * Time.deltaTime;
        }
    }
    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isHidden)
        {
            userCrouching = !userCrouching;
            if (crouchRoutine != null) StopCoroutine(crouchRoutine);
            crouchRoutine = StartCoroutine(CrouchTransition(userCrouching));
        }
    }

    IEnumerator CrouchTransition(bool crouching)
    {
        isCrouching = crouching;
        float time = 0f;
        Vector3 startPos = playerCamera.localPosition;
        Vector3 targetPos = new Vector3(startPos.x, crouching ? crouchCameraHeight : standingCameraHeight, startPos.z);

        while (time < 1f)
        {
            time += Time.deltaTime * crouchTransitionSpeed;
            playerCamera.localPosition = Vector3.Lerp(startPos, targetPos, time);
            yield return null;
        }

        playerCamera.localPosition = targetPos; // Ensure exact position is set
        cameraStartPos = targetPos; // Fix for head bobbing interference
    }

    void ApplyHeadbob()
    {
        if (characterController.isGrounded && characterController.velocity.magnitude > 0.1f)
        {
            float currentBobFrequency = isCrouching ? crouchBobFrequency : bobFrequency;
            headbobTimer += Time.deltaTime * currentBobFrequency;
            float bobOffset = Mathf.Sin(headbobTimer) * bobAmount;
            playerCamera.localPosition = new Vector3(cameraStartPos.x, playerCamera.localPosition.y + bobOffset, cameraStartPos.z);

            if (Mathf.Sin(headbobTimer) < -0.99f && !footstepPlayed)
            {
                PlayFootstepSound();
                footstepPlayed = true;
            }
            else if (Mathf.Sin(headbobTimer) > 0.99f)
            {
                footstepPlayed = false;
            }
        }
        else
        {
            headbobTimer = 0;
            playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, cameraStartPos, Time.deltaTime * 5f);
        }
    }

    void PlayFootstepSound()
    {
        if (footstepSounds.Length > 0 && footstepAudioSource)
        {
            footstepAudioSource.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Length)]);
        }
    }
}
