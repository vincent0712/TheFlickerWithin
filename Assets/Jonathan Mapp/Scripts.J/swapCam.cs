using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class swapCam : MonoBehaviour
{
    public Camera cam; // Main camera
    public Camera cam2; // Camera we swap to

    public GameObject camera;

    public Transform camStartingPoint; 
    public Transform camTargetpoint;

    private float moveTime = 1f; // Time to move camera from startPoint to targetPoint
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
            Debug.Log(iscam);
        }

        if (ismove)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / moveTime;
            bool moveback;

            if (iscam)
            {
                camera.transform.position = Vector3.Lerp(camStartingPoint.position, camTargetpoint.position, progress);
                moveback = false;

            }
            else
            {
                camera.transform.position = Vector3.Lerp(camTargetpoint.position, camStartingPoint.position, progress);

                moveback = true;
            }

            if (progress >= 1 && moveback)
            {
                ismove = false;

                cam.gameObject.SetActive(!iscam);
                cam2.gameObject.SetActive(iscam);
            }
            else if(progress >=1 && !moveback)
            {
                ismove = false;
            }
        }
        
    }
}
