using System.Collections;
using TMPro;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light light;
    private bool isOn = true;
    public bool isFlickering = false; // Flickering state
    private AudioSource aud;
    private Transform flashlightPoint;
    public int battery = 100; // Battery as an integer
    public float followSpeed = 2f;
    public float rotateSpeed = 7f;
    public float intensity = 5f;
    public float range = 10f;
    private TextMeshProUGUI batteryLifeText;

    private void Start()
    {
        light.intensity = intensity;
        light.range = range;
        batteryLifeText = GetComponentInChildren<TextMeshProUGUI>();
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

        if (isFlickering && light.intensity > 0f && isOn)
        {
            StartCoroutine(FlickerLoop());
        }
        if (!isOn)
            isFlickering = false;
        batteryLifeText.text = "Battery: " + battery;
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
