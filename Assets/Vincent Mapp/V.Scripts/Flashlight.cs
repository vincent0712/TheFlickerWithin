using System.Collections;
using TMPro;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light light;
    public bool isOn = true;
    public bool isFlickering = false; // Flickering state
    private AudioSource aud;
    private Transform flashlightPoint;

    public bool canflicker = true;
    public float distanceMultiplier = 1f;
    public float smoothSpeed = 5f;


    public float fieldOfViewAngle = 90f; // Field of view angle
    public float maxViewDistance = 15f; // Maximum viewing distance
    public LayerMask obstacleLayer; // LayerMask for obstacles
    public Transform observer; // The observer (e.g., enemy or camera)
    public Transform target; // The target (e.g., player)

    private bool isTargetInSight = false; // Track if the target is in line of sight


    //public int battery = 100; // Battery as an integer
    public float followSpeed = 2f;
    public float rotateSpeed = 7f;
    public float intensity = 5f;
    public float range = 10f;
    private Movement movement;

    public TextMeshProUGUI batteryLifeText;
    private Camera cam;
    
    private void Start()
    {

        cam = Camera.main;
        light.intensity = intensity;
        light.range = range;
        movement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        //batteryLifeText = GetComponentInChildren<TextMeshProUGUI>();
        aud = gameObject.GetComponent<AudioSource>();
        flashlightPoint = GameObject.FindGameObjectWithTag("fp").transform;

        // Start the battery drain coroutine
        //StartCoroutine(DrainBattery());
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
        //GoToPoint();
        if (movement.isSpotted)
        {
            StartCoroutine(FlickerLoop());
        }
            


        if (isFlickering && light.intensity > 0f && isOn)
        {
            if (!canflicker)
                return;
            StartCoroutine(FlickerLoop());
        }
        if (!isOn)
            isFlickering = false;
        //batteryLifeText.text = "Battery: " + battery;
        light.intensity = isOn ? intensity : 0f;


        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            aud.Play();
            isOn = !isOn; //&& battery > 0;
            
            
        }

        if (Input.GetKeyDown(KeyCode.M)) // Toggle flicker for testing
        {
            isFlickering = !isFlickering; // Start or stop flickering

        }

    }

    private void GoToPoint()
    {
        // Smoothly move towards the target position
        transform.position = Vector3.Lerp(transform.position, flashlightPoint.position, followSpeed * Time.deltaTime);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, flashlightPoint.rotation, rotateSpeed * Time.deltaTime);
    }




    private float Rnd(float min, float max)
    {
        return Random.Range(min, max);
    }

    private IEnumerator FlickerLoop()
    {


        float originalIntensity = intensity;

        while (isFlickering) // Loop as long as isFlickering is true
        {
            float randomDelay = Rnd(0.05f, 0.3f); // Random delay between flickers

            // Randomize intensity or turn off completely
            if (Random.value > 0.5f)
            {
                light.intensity = Random.Range(originalIntensity * 0.5f, originalIntensity); // Dimmed light
            }
            else
            {
                light.intensity = 0f; // Light off
            }

            yield return new WaitForSeconds(randomDelay); // Wait for the random delay
        }

        // Restore flashlight to original state when flickering stops
        light.intensity = originalIntensity;
    }
}
