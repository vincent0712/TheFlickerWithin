using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light light;
    private bool ison = true;
    private AudioSource aud;

    private void Start()
    {
        aud = gameObject.GetComponent<AudioSource>();
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


    }
}
