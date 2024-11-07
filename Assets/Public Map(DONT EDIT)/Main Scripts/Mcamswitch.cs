using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mcamswitch : MonoBehaviour
{
    public Camera cam;
    public Camera cam2;
    bool iscam = false;
    void Start()
    {
        cam.gameObject.SetActive(true);
        cam2.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!iscam)
            {
                cam.gameObject.SetActive(false);
                cam2.gameObject.SetActive(true);
                iscam = true;
            }
            else if (iscam)
            {
                cam2.gameObject.SetActive(false);
                cam.gameObject.SetActive(true);
                iscam = false;
            }

        }

    }

}
