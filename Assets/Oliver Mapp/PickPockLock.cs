using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;

public class PickPockLock : MonoBehaviour
{
    // References to the UI sliders
    public Slider targetPinSlider;    // Slider that moves automatically
    public Slider playerPinSlider;    // Slider controlled by player for a single attempt

    // Parameters
    public float targetMoveSpeed = 2f;   // Speed of target's random movement
    public float targetRange = 0.05f;    // Range within which player must align with target to "pick" the lock

    private bool isLockPicked = false;
    private bool hasAttempted = false;   // Whether the player has made their one-time attempt

    void Start()
    {
        // Initialize the target to a random position and reset the player pin
        SetRandomTargetPosition();
        playerPinSlider.value = 0.5f;   // Start player pin in the center
    }

    void Update()
    {
        if (isLockPicked || hasAttempted) return;

        // Move the target pin back and forth automatically
        MoveTargetPin();

        // Listen for player's attempt to align the pin
        if (Input.GetKeyDown(KeyCode.Space))   // Use Space key for one-time positioning attempt
        {
            MakeAttempt();
        }
    }

    // Move the target slider randomly
    void MoveTargetPin()
    {
        // Oscillate target position back and forth
        targetPinSlider.value = Mathf.PingPong(Time.time * targetMoveSpeed, 1);
    }

    // Randomly sets the target slider to start at a new position
    void SetRandomTargetPosition()
    {
        targetPinSlider.value = Random.Range(0f, 1f);
    }

    // Executes the player's attempt to align with the target pin
    void MakeAttempt()
    {
        // Set player's pin to a fixed position based on their current slider value
        playerPinSlider.interactable = false;  // Prevent further adjustment

        // Check if player's pin is within the success range of the target pin
        if (Mathf.Abs(playerPinSlider.value - targetPinSlider.value) <= targetRange)
        {
            isLockPicked = true;
            Debug.Log("Lock picked successfully!");
            // Optionally, trigger success actions
        }
        else
        {
            Debug.Log("Attempt failed. Try again.");
            // Optional: Trigger failure actions, like shaking the lock
        }

        hasAttempted = true;
    }

    // Reset for a new attempt (if you want to allow retries)
    public void ResetAttempt()
    {
        isLockPicked = false;
        hasAttempted = false;
        playerPinSlider.interactable = true;
        playerPinSlider.value = 0.5f;     // Reset to center or wherever you want the player pin to start
        SetRandomTargetPosition();        // Set a new random position for target pin
    }
}
