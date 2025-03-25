using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class battery : MonoBehaviour, MInteractable
{
    private Mcamswitch flashlight;
    private Onscreentext onScreenText;


    private void Start()
    {

        flashlight = GameObject.FindGameObjectWithTag("Player").GetComponent<Mcamswitch>();
        onScreenText = FindObjectOfType<Onscreentext>();
    }

    public void Interact()
    {

        if (!flashlight.isCameraUnlocked)
        {
            onScreenText.ShowText("I Must Find A Use For These First", 4f);
        }
        else if (flashlight.isCameraUnlocked)
        {
            flashlight.battery = 100;
            gameObject.SetActive(false);
        }


    }
}
