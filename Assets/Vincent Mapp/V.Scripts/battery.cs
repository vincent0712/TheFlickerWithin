using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class battery : MonoBehaviour, MInteractable
{
    private Flashlight flashlight;

    private void Start()
    {
        flashlight = GameObject.FindGameObjectWithTag("fl").GetComponent<Flashlight>();
    }

    public void Interact()
    {
        flashlight.battery = 100;
        gameObject.SetActive(false);
    }
}
