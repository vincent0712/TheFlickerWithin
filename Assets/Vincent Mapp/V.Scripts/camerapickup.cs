using System.Runtime.CompilerServices;
using UnityEngine;

public class camerapickup : MonoBehaviour, MInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Mcamswitch cam;
    private Onscreentext onScreenText; // Reference to OnScreenText script
    public AudioSource au;
    public GameObject audioPrefab; // Assign a prefab with an AudioSource

    void Start()
    {
        
        cam = GameObject.FindGameObjectWithTag("Player").GetComponent<Mcamswitch>();
        onScreenText = FindObjectOfType<Onscreentext>();
        
    }

    public void Interact()
    {
        cam.isCameraUnlocked = true;
        onScreenText.ShowText("Press Q To Toggle Camera", 4f);

        DestroyWithSound();
        gameObject.SetActive(false);
    }


    void DestroyWithSound()
    {
        Instantiate(audioPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
