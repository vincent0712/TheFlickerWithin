using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.Rendering.DebugUI;

public class Mcamswitch : MonoBehaviour
{
    // Cameras
    public Camera cam;
    public Camera cam2;
    public GameObject camera;
    public GameObject monster;

    // Transforms
    public Transform camStartingPoint;
    public Transform camTargetpoint;
    public Transform observer;          // The object checking for the line of sight
    public Transform target;            // The object to check visibility for

    // Visibility and Field of View Settings
    public float maxViewDistance = 10f; // Maximum distance for visibility
    public float fieldOfViewAngle = 90f; // Field of view in degrees (45° left/right)
    public LayerMask obstacleLayer;     // Layer for objects blocking line of sight

    // Movement and Timing
    private float moveTime = 0.5f;
    private float elapsedTime = 0f;
    private bool iscam = false;
    private bool ismove = false;
    private bool go = false;

    // Head Bob Settings
    public bool enableHeadBob = true;
    public float bobSpeed = 5f;
    public float bobAmount = 0.05f;
    private float bobTimer = 0f;
    private Vector3 defaultCamLocalPosition;
    private Vector3 defaultCam2LocalPosition; // Added for cam2

    // Flashlight
    private GameObject flashlight;
    public Flashlight fl;

    // Footstep Sound Settings
    public AudioSource footstepAudio;
    public AudioSource heartbeat;
    public AudioSource cameraAu;
    public List<AudioClip> footsteps = new List<AudioClip>();
    private bool hasPlayedFootstep = false;

    void Start()
    {
        flashlight = GameObject.FindGameObjectWithTag("fl");
        cam.gameObject.SetActive(true);
        cam2.gameObject.SetActive(false);

        camera.transform.localPosition = camStartingPoint.localPosition;
        defaultCamLocalPosition = cam.transform.localPosition;
        defaultCam2LocalPosition = cam2.transform.localPosition; // Initialize cam2 default position
    }

    private void Sound()
    {
        if(fl.isFlickering == true)
        {
            if (heartbeat.volume > 0.9)
                return;
            heartbeat.volume += 1.5f * Time.deltaTime;
            
        }
        else if (!fl.isFlickering)
        {
            if (heartbeat.volume < 0.3)
                return;
            heartbeat.volume -= 1.5f * Time.deltaTime;
        }

    }

    void Update()
    {
        
        fl.isFlickering = IsInLineOfSight(observer.position, observer.forward, target.position);
        Sound();
        if (Input.GetKeyDown(KeyCode.Q) && !ismove)
        {
            ismove = true;
            elapsedTime = 0f;
            iscam = !iscam;
        }

        if (ismove)
        {

            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / moveTime;

            if (iscam)
            {
                camera.transform.localPosition = Vector3.Lerp(camStartingPoint.localPosition, camTargetpoint.localPosition, progress);
            }
            else
            {
                cam.gameObject.SetActive(!iscam);
                cam2.gameObject.SetActive(iscam);

                camera.SetActive(true);

                camera.transform.localPosition = Vector3.Lerp(camTargetpoint.localPosition, camStartingPoint.localPosition, progress);
                monster.SetActive(iscam);

                flashlight.gameObject.SetActive(!iscam);
            }

            if (progress >= 1)
            {
                ismove = false;

                cam.gameObject.SetActive(!iscam);
                cam2.gameObject.SetActive(iscam);

                if (iscam == true)
                {
                    camera.SetActive(false);
                    monster.SetActive(iscam);
                    cameraAu.Play();

                    flashlight.gameObject.SetActive(!iscam);
                }
                else
                {
                    camera.SetActive(true);
                }
            }
        }

        if (enableHeadBob && !ismove)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                bobTimer += Time.deltaTime * bobSpeed;
                float bobOffsetY = Mathf.Sin(bobTimer) * bobAmount;
                float bobOffsetX = Mathf.Cos(bobTimer * 0.5f) * bobAmount;

                // Apply head bobbing to the currently active camera
                if (cam.gameObject.activeSelf)
                {
                    cam.transform.localPosition = defaultCamLocalPosition + new Vector3(bobOffsetX, bobOffsetY, 0f);
                }
                else if (cam2.gameObject.activeSelf)
                {
                    cam2.transform.localPosition = defaultCam2LocalPosition + new Vector3(bobOffsetX, bobOffsetY, 0f);
                }

                // Footstep sound logic
                if (bobOffsetY < 0 && !hasPlayedFootstep)
                {
                    PlayFootstepSound();
                    hasPlayedFootstep = true;
                }

                if (bobOffsetY >= 0)
                {
                    hasPlayedFootstep = false;
                }
            }
            else
            {
                bobTimer = 0f;

                // Reset camera positions when not moving
                if (cam.gameObject.activeSelf)
                {
                    cam.transform.localPosition = defaultCamLocalPosition;
                }
                else if (cam2.gameObject.activeSelf)
                {
                    cam2.transform.localPosition = defaultCam2LocalPosition;
                }

                hasPlayedFootstep = false;
            }
        }
    }
    bool IsInLineOfSight(Vector3 observerPosition, Vector3 observerForward, Vector3 targetPosition)
    {
        // Calculate the direction from the observer to the target
        Vector3 directionToTarget = (targetPosition - observerPosition).normalized;

        // Check if the target is within the field of view
        float angleToTarget = Vector3.Angle(observerForward, directionToTarget);
        if (angleToTarget > fieldOfViewAngle / 2)
        {
            // Target is outside the field of view
            
            return false;
        }

        // Calculate the distance to the target
        float distanceToTarget = Vector3.Distance(observerPosition, targetPosition);
        if (distanceToTarget > maxViewDistance)
        {
            // Target is too far
            
            return false;
        }

        // Perform the raycast to check for obstacles
        if (Physics.Raycast(observerPosition, directionToTarget, out RaycastHit hit, distanceToTarget, obstacleLayer))
        {
            // The ray hit an obstacle before reaching the target
            
            return false;
        }

        // If we passed all checks, the target is in line of sight

        return true;
    }
    private void PlayFootstepSound()
    {
        if (footstepAudio != null)
        {
            System.Random random = new System.Random();
            int randomNumber = random.Next(0, footsteps.Count);
            footstepAudio.clip = footsteps[randomNumber];
            footstepAudio.pitch = UnityEngine.Random.Range(0.85f, 1.15f);
            footstepAudio.Play();
        }
        else
        {
            Debug.LogWarning("Footstep AudioSource is not assigned!");
        }
    }
}
