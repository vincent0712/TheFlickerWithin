using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2.5f;
    public float crouchSpeed = 1.5f;
    public float gravity = 9.8f;



    public bool canmove = true;
    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public Transform playerCamera;
    private float verticalRotation = 0f;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    public float crouchTransitionSpeed = 8f;
    private CharacterController characterController;
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
    }

    void Update()
    {
        if (!canmove)
            return;
        HandleLook();
        HandleMovement();
        HandleCrouch();
        ApplyHeadbob();
        Fear();
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
        if (!isSpotted && fear < 0.1)
        {
            chaseMusic.volume = 0;

        }


        







    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("table"))
        {
            isHidden = true;
            isCrouching = isHidden;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("table"))
        {
            isHidden = false;
            isCrouching = isHidden;
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
        if(characterController.velocity.magnitude > 0.15 && !isCrouching)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        float speed = isCrouching ? crouchSpeed : walkSpeed;
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        if (characterController.isGrounded)
        {
            moveDirection.y = 0f;
            moveDirection = move * speed;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isHidden)
        {
            isCrouching = !isCrouching;
        }

        float targetHeight = isCrouching ? crouchHeight : standingHeight;
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);
    }

    void ApplyHeadbob()
    {
        if (characterController.isGrounded && characterController.velocity.magnitude > 0.1f)
        {
            float currentBobFrequency = isCrouching ? crouchBobFrequency : bobFrequency;
            headbobTimer += Time.deltaTime * currentBobFrequency;
            float bobOffset = Mathf.Sin(headbobTimer) * bobAmount;
            playerCamera.localPosition = cameraStartPos + new Vector3(0, bobOffset, 0);

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
