using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Door : MonoBehaviour, MInteractable
{
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public bool isOpen = false;
    public bool isLocked = false;
    public AudioClip Doorshake;

    private AudioSource audioSource;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isAnimating = false;
    private NavMeshObstacle navObstacle;
    private Animation anim;

    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        navObstacle = GetComponent<NavMeshObstacle>();
        audioSource = GetComponent<AudioSource>();
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, openAngle, 0f));

        if (isLocked)
        {
            navObstacle.enabled = true;
            
        }
        else if (!isLocked)
        {
            navObstacle.enabled = false;
        }
    }

    public void Update()
    {
        if (isLocked)
        {
            navObstacle.enabled = true;
        }
        else if (!isLocked)
        {
            navObstacle.enabled = false;
        }
    }
    public void Interact()
    {
        if (isLocked)
        {
            navObstacle.enabled = true;
            if (!anim.isPlaying)
            {
                anim.Play("doorshake");
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.PlayOneShot(Doorshake);
            }

            return;
        }

        navObstacle.enabled = false;
        if (!isAnimating)
        {
            ToggleDoor();
        }
    }

    private void ToggleDoor()
    {
        if (isOpen)
            CloseDoor();
        else
            OpenDoor();
    }

    private void OpenDoor()
    {
        isOpen = true;
        PlaySound();
        StartCoroutine(RotateDoor(openRotation));
    }

    private void CloseDoor()
    {
        isOpen = false;
        PlaySound();
        StartCoroutine(RotateDoor(closedRotation));
    }

    private void PlaySound()
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }

    private IEnumerator RotateDoor(Quaternion targetRotation)
    {
        isAnimating = true;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }

        transform.rotation = targetRotation;
        isAnimating = false;
    }

    private void OnTriggerEnter(Collider other)
    {


        Debug.Log("ads");
        if (other.CompareTag("monster") && !isOpen && !isAnimating && !isLocked)
        {
            OpenDoor();
        }
    }
}