using UnityEngine;

public class HCameraChange : MonoBehaviour
{
    public GameObject camPos1;
    public GameObject camPos2;
    public GameObject camPos3;
    public Camera cam;
    public float transitionSpeed = 2.0f;

    private int count = 1;
    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = camPos1.transform.position;
        cam.transform.position = targetPosition;
    }

    void Update()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, Time.deltaTime * transitionSpeed);
        GoToRight();
        GoToLeft();
    }

    private void GoToRight()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (count == 1)
            {
                targetPosition = camPos2.transform.position;
                count++;
            }
            else if (count == 2)
            {
                targetPosition = camPos3.transform.position;
                count++;
            }
            else
            {
                targetPosition = camPos1.transform.position;
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
                targetPosition = camPos3.transform.position;
                count = 3;
            }
            else if (count == 2)
            {
                targetPosition = camPos1.transform.position;
                count--;
            }
            else
            {
                targetPosition = camPos2.transform.position;
                count--;
            }
        }
    }
}