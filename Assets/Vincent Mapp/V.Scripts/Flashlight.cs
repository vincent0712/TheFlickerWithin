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


    public float fieldOfViewAngle = 90f; // Field of view angle
    public float maxViewDistance = 15f; // Maximum viewing distance
    public LayerMask obstacleLayer; // LayerMask for obstacles
    public Transform observer; // The observer (e.g., enemy or camera)
    public Transform target; // The target (e.g., player)

    private bool isTargetInSight = false; // Track if the target is in line of sight

    public int battery = 100; // Battery as an integer
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
        StartCoroutine(DrainBattery());
    }

    private void OnEnable()
    {
        flashlightPoint = GameObject.FindGameObjectWithTag("fp").transform;
        transform.position = flashlightPoint.position;
        transform.rotation = flashlightPoint.rotation;
    }

    private void Update()
    {
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
            isOn = !isOn && battery > 0;
            
            
        }

        if (Input.GetKeyDown(KeyCode.M)) // Toggle flicker for testing
        {
            isFlickering = !isFlickering; // Start or stop flickering

        }

        GoToPoint();
    }
    private bool IsInLineOfSight(Vector3 observerPosition, Vector3 observerForward, Vector3 targetPosition)
    {
        // Calculate the direction from the observer to the target
        Vector3 directionToTarget = (targetPosition - observerPosition).normalized;

        // Check if the target is within the field of view
        float angleToTarget = Vector3.Angle(observerForward, directionToTarget);
        if (angleToTarget > fieldOfViewAngle / 2)
        {
            return false; // Target is outside the field of view
        }

        // Calculate the distance to the target
        float distanceToTarget = Vector3.Distance(observerPosition, targetPosition);
        if (distanceToTarget > maxViewDistance)
        {
            return false; // Target is too far
        }

        // Perform the raycast to check for obstacles
        if (Physics.Raycast(observerPosition, directionToTarget, out RaycastHit hit, distanceToTarget, obstacleLayer))
        {
            return false; // The ray hit an obstacle before reaching the target
        }

        // If we passed all checks, the target is in line of sight
        return true;
    }

    private IEnumerator DrainBattery()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second
            if (isOn && battery > 0)
            {
                battery -= 1;
                if (battery <= 0)
                {
                    battery = 0;
                    isOn = false;
                }
            }
        }
    }

    private void GoToPoint()
    {
        // Move towards the target's position
        transform.position = Vector3.MoveTowards(transform.position, flashlightPoint.position, followSpeed * Time.deltaTime);

        // Match the target's rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, flashlightPoint.rotation, rotateSpeed * Time.deltaTime);
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
