using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Fusebox : MonoBehaviour, MInteractable
{
    private Onscreentext onScreenText; // Reference to OnScreenText script

    public GameObject[] lights; // Assign your light GameObjects in the Inspector
    public int PuzzlesCompleted = 0;  // Integer value controlling the lights

    private void Start()
    {
        UpdateLights(); // Ensure lights are in the correct state at the start
    }

    public void SetLightLevel(int newLevel)
    {
        PuzzlesCompleted = Mathf.Clamp(newLevel, 0, lights.Length); // Clamp between 0 and max lights
        UpdateLights();
    }

    public void Update()
    {
        UpdateLights();
    }
    private void UpdateLights()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(i < PuzzlesCompleted); // Activate if index is within the level range
        }
    }
    public void Interact()
    {
        onScreenText.ShowText("Fuze Box Needs More Power!", 2f);
        onScreenText = FindObjectOfType<Onscreentext>();
    }


}
