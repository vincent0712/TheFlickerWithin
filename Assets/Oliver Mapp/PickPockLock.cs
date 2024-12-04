using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickPockLock : MonoBehaviour
{
    // References to the UI sliders
    public Slider targetPinSlider3;     // Slider that moves automatically3
    public Slider targetPinSlider2;    // Slider that moves automatically2
    public Slider targetPinSlider;    // Slider that moves automatically
    public Slider playerPinSlider;    // Slider controlled by player for a single attempt
    public GameObject locklockSprite;  // Target object to move upon completion


    // Current pin stage (1, 2, or 3)
    private int currentPin = 1;

    // Booleans to track each pin's completion
    private bool pin1c = false;
    private bool pin2c = false;
    private bool pin3c = false;

    // Parameters
    public float targetMoveSpeed = 2f;   // Speed of target's random movement
    public float targetRange = 0.05f;    // Range within which player must align with target to "pick" the lock

    // Boolean to indicate if the lock picking is fully complete
    public bool complete = false;

    void Start()
    {
        SetRandomTargetPosition();       // Initialize the first pin position
        playerPinSlider.value = 0.5f;    // Start player pin in the center
    }

    void Update()
    {
        if (complete) return;  // Exit if the lock has already been fully picked

        // Move only the active target pin
        if (currentPin == 1)
            MoveTargetPin(targetPinSlider);
        else if (currentPin == 2)
            MoveTargetPin(targetPinSlider2);
        else if (currentPin == 3)
            MoveTargetPin(targetPinSlider3);

        // Listen for player's attempt to align the pin
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MakeAttempt();
        }
    }

    // Move the target slider randomly (back and forth)
    void MoveTargetPin(Slider targetPin)
    {
        targetPin.value = Mathf.PingPong(Time.time * targetMoveSpeed, 1);
    }
  

    // Sets each target slider to start at a new random position
    void SetRandomTargetPosition()
    {
        targetPinSlider.value = Random.Range(0f, 1f);
        targetPinSlider2.value = Random.Range(0f, 1f);
        targetPinSlider3.value = Random.Range(0f, 1f);
    }

    // Executes the player's attempt to align with the current target pin
    void MakeAttempt()
    {
        playerPinSlider.interactable = false;  // Prevent further adjustment during attempt

        bool success = false;
        Slider currentTarget = null;

        // Check the current pin attempt and verify alignment
        if (currentPin == 1)
        {
            currentTarget = targetPinSlider;
            success = Mathf.Abs(playerPinSlider.value - targetPinSlider.value) <= targetRange;
            pin1c = success;
        }
        else if (currentPin == 2)
        {
            currentTarget = targetPinSlider2;
            success = Mathf.Abs(playerPinSlider.value - targetPinSlider2.value) <= targetRange;
            pin2c = success;
        }
        else if (currentPin == 3)
        {
            currentTarget = targetPinSlider3;
            success = Mathf.Abs(playerPinSlider.value - targetPinSlider3.value) <= targetRange;
            pin3c = success;

        }

        if (success)
        {
            Debug.Log($"Pin {currentPin} picked successfully!");
            currentPin++;

            // Check if all pins have been picked
            if (currentPin > 3)
            {
                complete = true;
                MoveLockSpriteUpward();

                Debug.Log("All pins picked! Lock successfully picked!");
            }
            else
            {
                // Move to the next pin
                ResetPlayerPin();
            }
        }
        else
        {
            Debug.Log("Attempt failed. Resetting to Pin 1.");
            StartCoroutine(ResetAfterDelay());
        }
    }
    // Move locklockSprite upward upon completion
    private void MoveLockSpriteUpward()
    {
        if (locklockSprite != null)
        {
            Vector3 newPosition = locklockSprite.transform.position + new Vector3(0, 20f, 0); // Adjust the '5f' as needed
            locklockSprite.transform.position = newPosition;
        }
        else
        {
            Debug.LogWarning("locklockSprite is not assigned.");
        }
    }
    // Resets the lock attempt back to Pin 1 if a pin fails
    private IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);  // Wait 2 seconds before resetting
        currentPin = 1;
        pin1c = pin2c = pin3c = false;
        SetRandomTargetPosition();           // Randomize target positions for all pins
        ResetPlayerPin();
    }

    // Reset player slider for new attempt
    private void ResetPlayerPin()
    {
        playerPinSlider.interactable = true;
        playerPinSlider.value = 0.5f;    // Reset to center
    }
}
