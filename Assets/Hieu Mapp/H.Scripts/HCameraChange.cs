using UnityEngine;

public class HCameraChange : MonoBehaviour
{
    AudioSource audio;
    public AudioClip buttonClick;

    public GameObject camPos1;
    public GameObject camPos2;
    public GameObject camPos3;
    public GameObject camPos4;
    public Camera cam;
    public float transitionSpeed = 2.0f;

    private int count = 1;
    private Vector3 targetPosition;

    void Start()
    {
        audio = GetComponent<AudioSource>();

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
            audio.PlayOneShot(buttonClick);
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
            else if (count == 3)
            {
                targetPosition = camPos4.transform.position;
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
            audio.PlayOneShot(buttonClick);
            if (count == 1)
            {
                targetPosition = camPos4.transform.position;
                count = 4;
            }
            else if (count == 2)
            {
                targetPosition = camPos1.transform.position;
                count--;
            }
            else if(count == 3)
            {
                targetPosition = camPos2.transform.position;
                count--;
            }
            else
            {
                targetPosition = camPos3.transform.position;
                count--;
            }
        }
    }
}