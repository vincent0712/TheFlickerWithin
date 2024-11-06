using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject light;
    private bool ison = false;
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
                light.SetActive(false);
            }
            else if (!ison)
            {
                light.SetActive(true);
            }
        }


    }
}
