using UnityEngine;

public class atticlock : MonoBehaviour, MInteractable
{
    public Key key;
    private bool isopen;
    public Door door;


    private Onscreentext onScreenText; // Reference to OnScreenText script


    public void Start()
    {
        onScreenText = FindObjectOfType<Onscreentext>();
    }
    public void Interact()
    {

        if (key.haskey)
        {
            gameObject.active = false;
            door.isLocked = false;
        }
        else if (!key.haskey)
        {
            onScreenText.ShowText("I Need To Find A Way To Get Inside", 2f);
        }
    }

}
