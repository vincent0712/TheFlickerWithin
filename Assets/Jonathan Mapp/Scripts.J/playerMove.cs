using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    [Header("Player Settings")]
    public float speed = 5f;
    public float mouseSensitivity = 2f;

    [Header("Gravity Settings")]
    public float gravity = -9.81f;
    private Vector3 velocity;


    private CharacterController controller;
    public Transform cameraTransform;
    public Transform camra2Transform;
    private float xRotation = 0f;

    void Start()
    {

        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;


        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        LookAround();
        MovePlayer();
    }


    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the camera up and down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply rotations to player and camera
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        camra2Transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }


    private void MovePlayer()
    {


        if (velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Get input for movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);




        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
