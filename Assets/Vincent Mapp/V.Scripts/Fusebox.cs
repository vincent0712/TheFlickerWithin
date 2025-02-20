using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.SceneManagement;

public class Fusebox : MonoBehaviour, MInteractable
{
    private Onscreentext onScreenText; // Reference to OnScreenText script

    public GameObject[] lights; // Assign your light GameObjects in the Inspector
    public int PuzzlesCompleted = 0;  // Integer value controlling the lights
    public Animation fade;

    private void Start()
    {
        UpdateLights(); // Ensure lights are in the correct state at the start
        onScreenText = FindObjectOfType<Onscreentext>();
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
        if(PuzzlesCompleted < 4)
            onScreenText.ShowText("Fuze Box Needs More Power!", 2f);

        if(PuzzlesCompleted == 4)
        {
            onScreenText.ShowText("You Win!", 2f);
            StartCoroutine(swapscene());
        }



    }
    private IEnumerator swapscene()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(wait("StartMenu"));
    }
    public IEnumerator wait(string name)
    {
        fade.Play("fadeout");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("StartMenu");
    }



}
