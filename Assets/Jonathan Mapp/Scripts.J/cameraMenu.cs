using UnityEngine;

public class cameraMenu : MonoBehaviour
{

    private float xRotation;
    private float yRotation;
    private float mouseSensitivity = 10f;
    private Transform playerCamera;

    void Start()
    {
        playerCamera = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Confined;
    }


    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -10f, 10f);
        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, -10f, 10f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, yRotation -25f, 0f);
    }
}
