using UnityEngine;

public class puzzlebutton : MonoBehaviour, MInteractable
{
    public int buttonIndex; // Assign a unique index (0-3) in the Inspector
    public bool isEnter = false; // Set to true if this is the enter button
    private colorgame puzzleManager;
    private AudioSource au;


    void Start()
    {
        puzzleManager = FindObjectOfType<colorgame>();
        au = gameObject.GetComponent<AudioSource>();
 

    }

    public void Interact()
    {
        au.Play();
        if (isEnter)
        {
            puzzleManager.EnterPressed();
            Debug.Log("Enter Button Pressed");
        }
        else
        {
            puzzleManager.ButtonPressed(buttonIndex);
            Debug.Log("Pressed Button: " + buttonIndex);
        }
    }
}
