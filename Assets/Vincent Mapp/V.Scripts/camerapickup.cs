using UnityEngine;

public class camerapickup : MonoBehaviour, MInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Mcamswitch cam;
    private Onscreentext onScreenText; // Reference to OnScreenText script
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("Player").GetComponent<Mcamswitch>();
        onScreenText = FindObjectOfType<Onscreentext>();
    }

    public void Interact()
    {
        cam.isCameraUnlocked = true;
        gameObject.SetActive(false);
        onScreenText.ShowText("Press Q To Toggle Camera", 4f);
    }
}
