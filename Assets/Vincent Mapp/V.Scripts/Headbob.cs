using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headbob : MonoBehaviour
{
    [Header("Head Bob Settings")]
    public float bobFrequency = 2.0f;      // Speed of the bobbing
    public float bobHorizontalAmplitude = 0.05f; // Side-to-side bobbing amount
    public float bobVerticalAmplitude = 0.1f;    // Up-and-down bobbing amount
    public float smoothTransition = 8f;    // Smooth transition back to original position
    public float minMoveSpeed = 0.1f;      // Minimum speed to trigger bobbing

    private Vector3 initialPosition;       // Starting position for the camera
    private float bobTimer = 0.0f;         // Timer to control the bobbing

    public CharacterController characterController; // Reference to the CharacterController component

    void Start()
    {
        initialPosition = transform.localPosition;
        
    }

    void Update()
    {
        // Get the player's movement speed
        float speed = characterController != null ? characterController.velocity.magnitude : 0.0f;
        Debug.Log(speed);
        // If the player is moving above the minimum speed, apply head bobbing
        if (speed > minMoveSpeed)
        {
            bobTimer += Time.deltaTime * bobFrequency;

            // Calculate head bobbing position offsets
            float bobX = Mathf.Sin(bobTimer) * bobHorizontalAmplitude;
            float bobY = Mathf.Cos(bobTimer * 2) * bobVerticalAmplitude;

            // Apply the calculated bobbing offset to the camera's position
            transform.localPosition = initialPosition + new Vector3(bobX, bobY, 0);
        }
        else
        {
            // Smoothly transition back to the original position when player stops moving
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * smoothTransition);
            bobTimer = 0.0f; // Reset the timer when not moving to start fresh on next movement
        }
    }
}
