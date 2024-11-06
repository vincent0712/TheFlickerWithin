using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class swapCam : MonoBehaviour
{
    public Camera cam;
    private Volume camVolume;
    void Start()
    {
        camVolume = GetComponent<Volume>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void switchCam()
    {
        gameObject.SetActive(camVolume);
    }
}
