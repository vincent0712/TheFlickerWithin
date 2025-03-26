using UnityEngine;

public class HCameraChange : MonoBehaviour
{
    public GameObject camPos1;
    public GameObject camPos2;
    public GameObject camPos3;
    public Camera cam;

    private int count = 1;

    void Start()
    {
        cam.transform.position = camPos1.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        GoToRight();
        GoToLeft();
    }

    private void GoToRight()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (count == 1)
            {
                cam.transform.position = camPos2.transform.position;
                count++;
            }

            else if (count == 2)
            {
                cam.transform.position = camPos3.transform.position;
                count++;
            }

            else
            {
                cam.transform.position = camPos1.transform.position;
                count = 1;
            }
        }
    }

    private void GoToLeft()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (count == 1)
            {
                cam.transform.position = camPos3.transform.position;
                count = 3;
            }

            else if (count == 2)
            {
                cam.transform.position = camPos1.transform.position;
                count--;
            }

            else
            {
                cam.transform.position = camPos2.transform.position;
                count--;
            }
        }
    }
}
