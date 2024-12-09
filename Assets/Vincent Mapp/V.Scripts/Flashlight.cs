using System.Collections;
using TMPro;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light light;
    private bool isOn = true;
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
        batteryLifeText.text = "Battery: " + battery;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            aud.Play();
            if (isOn)
            {
                light.intensity = 0f;
                isOn = false;
            }
            else if (!isOn && battery > 0)
            {
                light.intensity = intensity;
                isOn = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            battery += 3;
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
                    light.intensity = 0f; // Turn off the light when battery is dead
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
}
