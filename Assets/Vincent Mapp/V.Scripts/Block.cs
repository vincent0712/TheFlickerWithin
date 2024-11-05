using UnityEngine;
using UnityEngine.UIElements;

public class Block : MonoBehaviour , IInteractable
{
    public Color newColor = Color.red;
    public void Interact()
    {
        Debug.Log("Det funkade!");
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = newColor;
    }
}
