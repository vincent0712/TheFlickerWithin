using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Mcamswitch : MonoBehaviour
{
    public Camera cam; 
    public Camera cam2; 

    public GameObject camera; 
    public GameObject monster;

    public Transform camStartingPoint;
    public Transform camTargetpoint;

    private float moveTime = 0.5f; 
    private float elapsedTime = 0f; 
    private bool iscam = false; 
    private bool ismove = false;

    // Head bob settings
    public bool enableHeadBob = true; 
    public float bobSpeed = 5f; 
    public float bobAmount = 0.05f; 

    private float bobTimer = 0f; 
    private Vector3 defaultCamLocalPosition; 
    private bool hasPlayedFootstep = false;
    private GameObject flashlight;
    private bool go = false;
    

    // Footstep sound
    public AudioSource footstepAudio;
    public AudioSource cameraAu;
    public List<AudioClip> footsteps = new List<AudioClip>();


    void Start()
    {
        flashlight = GameObject.FindGameObjectWithTag("fl");
        cam.gameObject.SetActive(true);
        cam2.gameObject.SetActive(false);

        camera.transform.localPosition = camStartingPoint.localPosition;
        defaultCamLocalPosition = cam.transform.localPosition; 
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

                cam.transform.localPosition = defaultCamLocalPosition + new Vector3(bobOffsetX, bobOffsetY, 0f);


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
            footstepAudio.pitch = UnityEngine.Random.Range((float)0.85, (float)1.15);
            footstepAudio.Play();
        }
        else
        {
            Debug.LogWarning("Footstep AudioSource is not assigned!");
        }
    }
}
