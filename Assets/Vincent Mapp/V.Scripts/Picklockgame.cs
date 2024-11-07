using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Picklockgame : MonoBehaviour
{
    // UI sliders for the minigame
    public Slider targetPinSlider3;
    public Slider targetPinSlider2;
    public Slider targetPinSlider;
    public Slider playerPinSlider;

    // Current pin stage (1, 2, or 3)
    private int currentPin = 1;

    // Completion status of each pin
    private bool pin1c = false;
    private bool pin2c = false;
    private bool pin3c = false;

    // Parameters
    public float targetMoveSpeed = 2f;
    public float targetRange = 0.05f;

    // Boolean to indicate if the lock picking is fully complete
    public bool complete = false;

    // Reference to the door script to unlock
    public Mdoor door;
    public MPlayermovement player;

    void Start()
    {
        
        SetRandomTargetPosition();
        playerPinSlider.value = 0.5f;
    }

    void Update()
    {
        if (complete) return;

        if (currentPin == 1)
            MoveTargetPin(targetPinSlider);
        else if (currentPin == 2)
            MoveTargetPin(targetPinSlider2);
        else if (currentPin == 3)
            MoveTargetPin(targetPinSlider3);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            MakeAttempt();
        }
    }

    void MoveTargetPin(Slider targetPin)
    {
        targetPin.value = Mathf.PingPong(Time.time * targetMoveSpeed, 1);
    }

    void SetRandomTargetPosition()
    {
        targetPinSlider.value = Random.Range(0f, 1f);
        targetPinSlider2.value = Random.Range(0f, 1f);
        targetPinSlider3.value = Random.Range(0f, 1f);
    }

    void MakeAttempt()
    {
        playerPinSlider.interactable = false;

        bool success = false;

        if (currentPin == 1)
        {
            success = Mathf.Abs(playerPinSlider.value - targetPinSlider.value) <= targetRange;
            pin1c = success;
        }
        else if (currentPin == 2)
        {
            success = Mathf.Abs(playerPinSlider.value - targetPinSlider2.value) <= targetRange;
            pin2c = success;
        }
        else if (currentPin == 3)
        {
            success = Mathf.Abs(playerPinSlider.value - targetPinSlider3.value) <= targetRange;
            pin3c = success;
        }

        if (success)
        {
            Debug.Log($"Pin {currentPin} picked successfully!");
            currentPin++;

            if (currentPin > 3)
            {
                complete = true;
                gameObject.SetActive(false);
                player.canmove = true;
                Debug.Log("All pins picked! Lock successfully picked!");

                // Unlock the door
                if (door != null)
                {
                    door.islocked = false;
                    Debug.Log("Door is now unlocked.");
                }
                else
                {
                    Debug.LogWarning("Door reference not set in Picklockgame script!");
                }
            }
            else
            {
                ResetPlayerPin();
            }
        }
        else
        {
            Debug.Log("Attempt failed. Resetting to Pin 1.");
            StartCoroutine(ResetAfterDelay());
        }
    }

    private IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        currentPin = 1;
        pin1c = pin2c = pin3c = false;
        SetRandomTargetPosition();
        ResetPlayerPin();
    }

    private void ResetPlayerPin()
    {
        playerPinSlider.interactable = true;
        playerPinSlider.value = 0.5f;
    }
}
