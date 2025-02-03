using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class Mcamswitch : MonoBehaviour
{
    // Cameras
    public GameObject videocamera; // The visual camera to toggle
    public Camera playerCamera;    // The player's camera (stationary)
    public GameObject flashlight;
    public MeshRenderer monster;
    // Transforms
    public Transform camStartingPoint; // Starting position for the visual camera
    public Transform camTargetPoint;   // Target position for the visual camera

    // Post-Processing Effects
    public Volume vol; // Post-processing volume for effects like night vision


    // Night Vision Effects
    public AudioSource nightVisionToggleSound;
    private bool isNightVision = false; // Tracks if night vision is active
    public Light nightvisionlight;

    // Movement and Timing
    public float moveTime = 1f; // Time it takes to move the camera
    private bool isCamAtTarget = false; // Tracks camera's current position
    private bool isTransitioning = false; // Prevents multiple transitions at once

    // Camera Wobble
    public float wobbleAmountX = 0.1f; // Amplitude of the wobble on X-axis
    public float wobbleAmountY = 0.05f; // Amplitude of the wobble on Y-axis
    public float wobbleSpeed = 5f;    // Frequency of the wobble
    private Vector3 originalCameraPosition;
    private bool isPlayerMoving = false;
    private float wobbleTimer = 0f;
    private bool hasPlayedFootstepOnWobble = false;

    // Footstep Sounds
    public AudioSource footstepAudioSource;
    public AudioClip[] footstepClips; // Array of footstep audio clips

    void Start()
    {
        // Set the initial position of the visual camera
        videocamera.transform.position = camStartingPoint.position;


        // Ensure night vision starts disabled
        DisableNightVision();

        // Store the original camera position for wobble calculations
        originalCameraPosition = playerCamera.transform.localPosition;
    }

    void Update()
    {
        // Toggle camera movement and effects on key press
        if (Input.GetKeyDown(KeyCode.Q) && !isTransitioning)
        {
            StartCoroutine(SwapCameraWithEffects());
        }

        // Handle camera wobble and footsteps
        HandlePlayerMovementEffects();
    }

    private IEnumerator SwapCameraWithEffects()
    {
        isTransitioning = true;


        // Disable night vision immediately when transitioning
        DisableNightVision();

        // Move the camera
        yield return StartCoroutine(MoveCamera());


        // Re-enable night vision only if the camera is at the target point
        if (isCamAtTarget)
        {
            EnableNightVision();
        }

        isTransitioning = false;
    }

    private IEnumerator MoveCamera()
    {
        Vector3 start = isCamAtTarget ? camTargetPoint.position : camStartingPoint.position;
        Vector3 end = isCamAtTarget ? camStartingPoint.position : camTargetPoint.position;

        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / moveTime;

            videocamera.transform.position = Vector3.Lerp(start, end, progress);

            yield return null;
        }

        // Snap to the target position to ensure precision
        videocamera.transform.position = end;

        // Toggle the current position flag
        isCamAtTarget = !isCamAtTarget;
    }

    private void EnableNightVision()
    {
        isNightVision = true;
        videocamera.SetActive(false);
        monster.enabled = !isNightVision;

        if (nightvisionlight != null)
            nightvisionlight.intensity = 15f;

        if (vol != null)
        {
            vol.enabled = true;
            flashlight.SetActive(false);
        }

        // Play toggle sound
        if (nightVisionToggleSound)
        {
            nightVisionToggleSound.Play();
        }
    }

    private void DisableNightVision()
    {
        videocamera.SetActive(true);
        isNightVision = false;
        monster.enabled = !isNightVision;

        if (nightvisionlight != null)
            nightvisionlight.intensity = 0f;

        if (vol != null)
        {
            vol.enabled = false;
            flashlight.SetActive(true);
        }
    }



    private void HandlePlayerMovementEffects()
    {
        // Check for player movement (example: WASD keys or arrow keys)
        isPlayerMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

        // Apply camera wobble if the player is moving
        if (isPlayerMoving)
        {
            wobbleTimer += Time.deltaTime * wobbleSpeed;
            float wobbleX = Mathf.Sin(wobbleTimer) * wobbleAmountX;
            float wobbleY = Mathf.Sin(wobbleTimer * 0.5f) * wobbleAmountY;
            playerCamera.transform.localPosition = originalCameraPosition + new Vector3(wobbleX, wobbleY, 0);

            // Play footstep sound when the wobble is at its lowest point on the Y-axis
            if (Mathf.Sin(wobbleTimer * 0.5f) < -0.99f && !hasPlayedFootstepOnWobble)
            {
                PlayRandomFootstep();
                hasPlayedFootstepOnWobble = true;
            }

            // Reset flag when wobble moves back up
            if (Mathf.Sin(wobbleTimer * 0.5f) > 0f)
            {
                hasPlayedFootstepOnWobble = false;
            }
        }
        else
        {
            // Reset camera position if the player is not moving
            playerCamera.transform.localPosition = originalCameraPosition;
            wobbleTimer = 0f; // Reset wobble timer for smooth restart
        }
    }

    private void PlayRandomFootstep()
    {
        if (footstepClips.Length > 0 && footstepAudioSource != null)
        {
            AudioClip randomClip = footstepClips[Random.Range(0, footstepClips.Length)];
            footstepAudioSource.PlayOneShot(randomClip);
        }
    }
}
