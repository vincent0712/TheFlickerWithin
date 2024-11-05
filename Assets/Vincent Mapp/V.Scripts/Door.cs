// Script: Door.cs
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public float openAngle = 90f;          // Angle to open the door
    public float openSpeed = 2f;           // Speed at which the door opens
    public bool isOpen = false;            // State of the door

    private Quaternion closedRotation;     // Initial closed rotation
    private Quaternion openRotation;       // Target open rotation
    private bool isAnimating = false;      // Whether the door is currently rotating

    void Start()
    {
        // Save the closed rotation and calculate the target open rotation
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, openAngle, 0f));
    }

    public void Interact()
    {
        if (!isAnimating)
        {
            isOpen = !isOpen; // Toggle the door state
            StartCoroutine(RotateDoor());
        }
    }

    private System.Collections.IEnumerator RotateDoor()
    {
        isAnimating = true; // Start the animation

        Quaternion targetRotation = isOpen ? openRotation : closedRotation;

        // Smoothly rotate the door towards the target rotation
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }

        transform.rotation = targetRotation; // Snap to the final rotation
        isAnimating = false; // End the animation
    }
}
