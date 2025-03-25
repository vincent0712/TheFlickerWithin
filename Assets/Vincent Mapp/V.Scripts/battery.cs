using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class battery : MonoBehaviour, MInteractable
{
    private Mcamswitch flashlight;

    private void Start()
    {
        flashlight = GameObject.FindGameObjectWithTag("Player").GetComponent<Mcamswitch>();
    }

    public void Interact()
    {
        flashlight.battery = 100;
        gameObject.SetActive(false);
    }
}
