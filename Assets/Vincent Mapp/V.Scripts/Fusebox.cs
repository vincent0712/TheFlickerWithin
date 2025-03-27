using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.SceneManagement;

public class Fusebox : MonoBehaviour, MInteractable
{
    private Onscreentext onScreenText; // Reference to OnScreenText script

    public int PuzzlesCompleted = 0;  // Integer value controlling the lights
    public Animation fade;
    public GameObject[] lightsonfusebox;
    public GameObject[] finnishlights;
    public Animation LeverAnimation;
    private bool canpull = true;

    private AudioSource au;

    private void Start()
    {

        UpdateLights(); // Ensure lights are in the correct state at the start
        onScreenText = FindObjectOfType<Onscreentext>();
        au = gameObject.GetComponent<AudioSource>();

        //StartCoroutine(startText());
    }

    public void SetLightLevel(int newLevel)
    {
        PuzzlesCompleted = Mathf.Clamp(newLevel, 0, lightsonfusebox.Length); // Clamp between 0 and max lights
        UpdateLights();
    }

    public IEnumerator startText()
    {
        onScreenText.ShowText("Complete Puzzles To Fix The Fuse Box", 4f);
        yield return new WaitForSeconds(6);
        onScreenText.ShowText("Fix The Fuse Box To Light Up The House", 4f);

    }
    

    public void Update()
    {
        UpdateLights();
    }
    private void UpdateLights()
    {
        for (int i = 0; i < lightsonfusebox.Length; i++)
        {
            lightsonfusebox[i].SetActive(i < PuzzlesCompleted); // Activate if index is within the level range
        }
    }
    public void Interact()
    {
        if (PuzzlesCompleted < 4)
        {
            onScreenText.ShowText("Fuze Box Needs More Power!", 2f);
            return; // Stop execution if conditions are not met
        }

        if (PuzzlesCompleted == 4 && canpull)
        {
            canpull = false;
            au.Play();
            onScreenText.ShowText("You Win!", 2f);

            // Turn on the lights
            for (int i = 0; i < finnishlights.Length; i++)
            {
                finnishlights[i].gameObject.SetActive(true); // Correct way to enable GameObjects
            }

            LeverAnimation.Play("levelpull");
            StartCoroutine(swapscene());
        }
    }

    private IEnumerator swapscene()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(wait("Outro"));
    }
    public IEnumerator wait(string name)
    {
        fade.Play("fadeout");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Outro");
    }



}
