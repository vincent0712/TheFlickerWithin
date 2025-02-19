using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Fusebox : MonoBehaviour, MInteractable
{
    private Onscreentext onScreenText; // Reference to OnScreenText script

    private int PuzzlesCompleted = 0;
    public int PuzzlesIngame = 4;

    void Start()
    {
        // Find the OnScreenText component in the scene
        onScreenText = FindObjectOfType<Onscreentext>();

        if (onScreenText == null)
        {
            Debug.LogError("OnScreenText not found in the scene!");

            onScreenText.ShowText("Hello, Player!", 3f);
        }
    }


    public void Interact()
    {
        if(PuzzlesCompleted < PuzzlesIngame)
        {
            onScreenText.ShowText("Fuze Box Needs More Power!", 2f);
        }
    }
}
