using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Flashlight : MonoBehaviour
{
    public Light light;
    private bool ison = true;
    private AudioSource aud;
    private Transform flashlightpoint;
    public float followSpeed = 2f;
    public float rotateSpeed = 7f;

    private void Start()
    {
        aud = gameObject.GetComponent<AudioSource>();
        flashlightpoint = GameObject.FindGameObjectWithTag("fp").transform;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            aud.Play();
            if (ison)
            {
                light.intensity = 0f;
                ison = false;
            }
            else if (!ison)
            {
                light.intensity = 5f;
                ison = true;
            }
        }
        Gotopoint();

    }

    private void Gotopoint()
    {
        // Move towards the target's position
        transform.position = Vector3.MoveTowards(transform.position, flashlightpoint.position, followSpeed * Time.deltaTime);

        // Match the target's rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, flashlightpoint.rotation, rotateSpeed * Time.deltaTime);
    }
}
