using System.Collections;
using TMPro;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light flashlight;  // Renamed from "light" to avoid conflicts
    public bool isOn = true;
    public bool isreallyon = true;
    public bool isFlickering = false;
    private AudioSource aud;
    private Coroutine flickerCoroutine;
    private Transform flashlightPoint;

    public bool canflicker = true;
    public float distanceMultiplier = 1f;
    public float followSpeed = 5f;
    public float rotateSpeed = 5f;

    public float intensity = 5f;
    public float range = 10f;
    private Movement movement;

    public TextMeshProUGUI batteryLifeText;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        flashlight.intensity = intensity;  // Ensure light is initialized
        flashlight.range = range;
        movement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        aud = gameObject.GetComponent<AudioSource>();
        flashlightPoint = GameObject.FindGameObjectWithTag("fp").transform;
        aud.Play();
        isreallyon = !isreallyon;
        isOn = isreallyon;
        flashlight.intensity = isreallyon ? intensity : 0f;  // Correctly update light intensity
    }

    private void OnEnable()
    {
        flashlightPoint = GameObject.FindGameObjectWithTag("fp").transform;
        transform.position = flashlightPoint.position;
        transform.rotation = flashlightPoint.rotation;
    }

    private void FixedUpdate()
    {
        GoToPoint();
    }

    private void Update()
    {
        // Toggle flashlight when right mouse button is pressed
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            aud.Play();
            isreallyon = !isreallyon;
            isOn = isreallyon;
            flashlight.intensity = isreallyon ? intensity : 0f;  // Correctly update light intensity
        }

        // Handle flickering if spotted
        if (movement.isSpotted && isreallyon && canflicker && !isFlickering)
        {
            StartCoroutine(FlickerLoop());
        }
    }

    private void GoToPoint()
    {
        // Smoothly move towards the target position
        transform.position = Vector3.Lerp(transform.position, flashlightPoint.position, followSpeed * Time.deltaTime);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, flashlightPoint.rotation, rotateSpeed * Time.deltaTime);
    }

    private IEnumerator FlickerLoop()
    {
        isFlickering = true;
        float originalIntensity = intensity;

        while (movement.isSpotted && isreallyon)
        {
            float randomDelay = Random.Range(0.05f, 0.1f);
            flashlight.intensity = (Random.value > 0.5f) ? Random.Range(originalIntensity * 0.5f, originalIntensity) : 0f;
            yield return new WaitForSeconds(randomDelay);
        }

        flashlight.intensity = isreallyon ? intensity : 0f;  // Ensure flashlight returns to correct state
        isFlickering = false;
    }

    public void StartFlickering(bool forceFlicker)
    {
        if (!gameObject.activeInHierarchy) return;

        if (forceFlicker || (!isFlickering && isreallyon && canflicker))
        {
            if (flickerCoroutine == null)
            {
                flickerCoroutine = StartCoroutine(FlickerLoop());
            }
        }
    }


    public void StopFlickering()
    {
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flashlight.intensity = isreallyon ? intensity : 0f;
            isFlickering = false;
            flickerCoroutine = null;
        }
    }
}
