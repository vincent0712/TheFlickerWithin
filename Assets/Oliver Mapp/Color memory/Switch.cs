using UnityEngine;

public class Switch : MonoBehaviour, MInteractable
{
    public bool IsOn = false; 
    public SwitchPuzzle puzzle; 

    public delegate void SwitchToggled();
    public event SwitchToggled OnSwitchToggled;



    public void Interact()
    {
        if (puzzle.victory)
            return;
        ToggleSwitch();
        puzzle.Interactwithpuzzle();
    }

    void ToggleSwitch()
    {
        IsOn = !IsOn;
        Debug.Log(gameObject.name + " state changed to: " + (IsOn ? "ON" : "OFF"));

        // Ensure the puzzle reference exists
        if (puzzle == null || puzzle.switches == null)
        {
            Debug.LogError("Puzzle reference or switches array is missing for " + gameObject.name);
            return;
        }

        // Check if player has won
        OnSwitchToggled?.Invoke();
    }

    public void ToggleWithoutAffectingOthers()
    {
        IsOn = !IsOn;
        Debug.Log(gameObject.name + " state changed to (indirectly): " + (IsOn ? "ON" : "OFF"));
    }
}

