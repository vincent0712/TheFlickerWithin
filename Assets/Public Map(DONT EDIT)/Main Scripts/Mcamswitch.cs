using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mcamswitch : MonoBehaviour
{
    public Camera cam; // Main camera
    public Camera cam2; // Camera we swap to

    public GameObject camera;
    public GameObject monster;

    public Transform camStartingPoint;
    public Transform camTargetpoint;

    private float moveTime = 0.5f; // Time to move camera from startPoint to targetPoint
    private float elapsedTime = 0f; // Track time
    private bool iscam = false; // Treack activ camera
    private bool ismove = false;

    void Start()
    {
        cam.gameObject.SetActive(true);
        cam2.gameObject.SetActive(false);

        camera.transform.position = camStartingPoint.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !ismove)
        {
            ismove = true;
            elapsedTime = 0f;
            iscam = !iscam;

        }

        if (ismove)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / moveTime;

            if (iscam)
            {
                camera.transform.position = Vector3.Lerp(camStartingPoint.position, camTargetpoint.position, progress);
                
            }
            else
            {
                cam.gameObject.SetActive(!iscam);
                cam2.gameObject.SetActive(iscam);
                

                camera.SetActive(true);

                camera.transform.position = Vector3.Lerp(camTargetpoint.position, camStartingPoint.position, progress);
                monster.SetActive(iscam);
            }

            if (progress >= 1)
            {
                ismove = false;

                cam.gameObject.SetActive(!iscam);
                cam2.gameObject.SetActive(iscam);

                if (iscam == true)
                {
                    camera.SetActive(false);
                    monster.SetActive(iscam);

                }
                else
                {
                    camera.SetActive(true);
                    
                }
                    

            }
        }

    }


}
