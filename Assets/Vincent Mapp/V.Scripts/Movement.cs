using UnityEngine;
using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2.5f;
    public float crouchSpeed = 1.5f;
    public float gravity = 9.8f;
    public float runspeed = 5f;
    public bool canmove = true;
    public float currentSpeed = 0f; // Tracks speed for smooth acceleration
    [SerializeField] private float acceleration = 8f; // Controls how quickly speed changes


    [Header("Sprint")]
    public float stamina = 10f;
    public float maxStamina = 10f;
    public float staminaDrain = 1f;
    public float staminaRegen = 1f;
    public bool isrunning = false;
    public Image staminabar;
    private bool canrun = true;
    


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
    public float laydownheight = 0.1f;

    public bool isCrouching = false;
    public bool isHidden = false;
    public bool isMoving = false;
    private bool canregenstamina = true;
    public bool isincloset = false;


    [Header("Fear Settings")]
    public float fear;
    public bool isSpotted = false;

    [Header("Headbob Settings")]
    public float bobFrequency = 10f;
    public float crouchBobFrequency = 5f;
    public float runfrequency = 10f;
    public float bobAmount = 0.05f;
    private float headbobTimer = 0;
    private Vector3 cameraStartPos;
    private bool footstepPlayed = false;

    [Header("Footstep Sounds")]
    public AudioClip[] footstepSounds;
    public AudioSource footstepAudioSource;
    private MonsterAI monster;

    [Header("Audio")]
    public AudioSource heartbeat;
    public AudioSource chaseMusic;

    private Vector3 moveDirection;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        
        monster = GameObject.FindGameObjectWithTag("monster").GetComponent<MonsterAI>();
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
        if (other.CompareTag("table") && isCrouching)
        {
            isHidden = true;

        }
        else if (other.CompareTag("closet"))
        {
            isincloset = true;
            isHidden = true;
        }
        else if(other.CompareTag("bed"))
        {
            if (crouchRoutine != null) StopCoroutine(crouchRoutine);
            crouchRoutine = StartCoroutine(CrouchTransition(true, false)); // Force crouch
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("table"))
        {
            isHidden = false;

        }
        else if (other.CompareTag("closet"))
        {
            isincloset = false;
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
        isMoving = characterController.velocity.magnitude > 0.1f && !isCrouching;

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && stamina > 0.1f && isMoving)
        {
            if (canrun)
                isrunning = true;
        }
        else
        {
            isrunning = false;
        }

        // Acceleration-based speed change
        float targetSpeed = isCrouching ? crouchSpeed : (isrunning ? runspeed : walkSpeed);
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 move = (transform.right * moveX + transform.forward * moveZ).normalized * currentSpeed * Time.deltaTime;

        if (characterController.isGrounded)
        {
            if (moveDirection.y < 0)
                moveDirection.y = -1f;

            moveDirection = move;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection);

        HandleStamina();
    }


    void HandleStamina()
    {
        if (isrunning && stamina > 0.1f && isMoving)
        {
            stamina -= staminaDrain * Time.deltaTime;
            if (stamina <= 0.1f)
            {
                stamina = 0.1f;
                isrunning = false;
                canrun = false;
                
                if (canregenstamina)
                {
                    StartCoroutine(runCD());
                }
            }
        }
        else if (!isrunning && stamina < maxStamina && canregenstamina)
        {
            stamina += staminaRegen * Time.deltaTime;
            stamina = Mathf.Min(stamina, maxStamina);
        }

        staminabar.fillAmount = stamina/maxStamina;
    }
    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {


            if (userCrouching && isHidden && !isincloset)
                return;

            if (!isCrouching)
            {
                characterController.height = 0.6f;
            }
            else if (isCrouching)
            {
                characterController.height = 1.15f;
            }
            userCrouching = !userCrouching;

            if (crouchRoutine != null)
                StopCoroutine(crouchRoutine);

            crouchRoutine = StartCoroutine(CrouchTransition(userCrouching, false));
        }
    }

    IEnumerator CrouchTransition(bool crouching, bool laydown)
    {
        isCrouching = crouching;
        float time = 0f;
        Vector3 startPos = playerCamera.localPosition;
        Vector3 targetPos = new Vector3(startPos.x, crouching ? crouchCameraHeight : standingCameraHeight, startPos.z);

        while (time < 1f)
        {
            time += Time.deltaTime * crouchTransitionSpeed;
            float t = Mathf.SmoothStep(0f, 1f, time);
            playerCamera.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        playerCamera.localPosition = targetPos;
        cameraStartPos = targetPos;
    }



    IEnumerator runCD()
    {
        canregenstamina = false;
        yield return new WaitForSeconds(3f);
        canregenstamina = true;
        canrun = true;
    }


    void ApplyHeadbob()
    {
        if (characterController.isGrounded && characterController.velocity.magnitude > 0.1f)
        {
            float currentBobFrequency = isCrouching ? crouchBobFrequency : bobFrequency;
            if (!isCrouching && isrunning)
                currentBobFrequency = runfrequency;

            headbobTimer += Time.deltaTime * currentBobFrequency;
            float bobOffset = Mathf.Sin(headbobTimer) * bobAmount;

            playerCamera.localPosition = new Vector3(cameraStartPos.x, cameraStartPos.y + bobOffset, cameraStartPos.z);

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
            float t = Mathf.SmoothStep(0f, 1f, Time.deltaTime * 5f); // Smooth transition
            playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, cameraStartPos, t);
        }
    }


    void PlayFootstepSound()
    {
        if (footstepSounds.Length > 0 && footstepAudioSource)
        {
            footstepAudioSource.pitch = Random.Range(0.5f, 0.55f);
            footstepAudioSource.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Length)]);
            if (isCrouching)
            {
                //monster.HearSound(gameObject.transform.position, 0.1f);

            }
            else if (!isCrouching)
            {
                if (isrunning)
                {
                    monster.HearSound(gameObject.transform.position, 0.55f);


                }
                else
                {
                    monster.HearSound(gameObject.transform.position, 0.45f);
                }
            }
        }
    }
}
