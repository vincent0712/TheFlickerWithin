using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Mcamswitch : MonoBehaviour
{
    public Camera cam; // Main camera
    public Camera cam2; // Camera we swap to

    public GameObject camera; // The object that moves
    public GameObject monster;

    public Transform camStartingPoint;
    public Transform camTargetpoint;

    private float moveTime = 0.5f; // Time to move camera from startPoint to targetPoint
    private float elapsedTime = 0f; // Track time
    private bool iscam = false; // Track active camera
    private bool ismove = false;

    // Head bob settings
    public bool enableHeadBob = true; // Toggle head bob effect
    public float bobSpeed = 5f; // Speed of the bobbing
    public float bobAmount = 0.05f; // Amount of the bobbing

    private float bobTimer = 0f; // Timer for head bob calculation
    private Vector3 defaultCamLocalPosition; // Default local position of the camera
    private bool hasPlayedFootstep = false; // To ensure sound plays only once per cycle

    // Footstep sound
    public AudioSource footstepAudio; // Assign an AudioSource for the footstep sound
    public List<AudioClip> footsteps = new List<AudioClip>();


    void Start()
    {

        cam.gameObject.SetActive(true);
        cam2.gameObject.SetActive(false);

        camera.transform.localPosition = camStartingPoint.localPosition;
        defaultCamLocalPosition = cam.transform.localPosition; // Store initial local position
    }

    void Update()
    {
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
                }
                else
                {
                    camera.SetActive(true);
                }
            }
        }

        // Apply head bobbing effect
        if (enableHeadBob && !ismove)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                // Simulate head bobbing
                bobTimer += Time.deltaTime * bobSpeed;
                float bobOffsetY = Mathf.Sin(bobTimer) * bobAmount;
                float bobOffsetX = Mathf.Cos(bobTimer * 0.5f) * bobAmount;

                cam.transform.localPosition = defaultCamLocalPosition + new Vector3(bobOffsetX, bobOffsetY, 0f);

                // Check for the lowest point of the bobbing cycle
                if (bobOffsetY < 0 && !hasPlayedFootstep)
                {
                    PlayFootstepSound();
                    hasPlayedFootstep = true;
                }

                // Reset the flag when the bobbing cycle moves upward
                if (bobOffsetY >= 0)
                {
                    hasPlayedFootstep = false;
                }
            }
            else
            {
                // Reset camera position when not moving
                bobTimer = 0f;
                cam.transform.localPosition = defaultCamLocalPosition;
                hasPlayedFootstep = false;
            }
        }
    }

    private void PlayFootstepSound()
    {
        if (footstepAudio != null)
        {

            System.Random random = new System.Random();
            int randomNumber = random.Next(0, 3);
            footstepAudio.clip = footsteps[randomNumber];
            footstepAudio.pitch = UnityEngine.Random.Range((float)0.8, (float)1.2);
            footstepAudio.Play();
        }
        else
        {
            Debug.LogWarning("Footstep AudioSource is not assigned!");
        }
    }
}
