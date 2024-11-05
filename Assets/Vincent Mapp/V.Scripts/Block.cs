using UnityEngine;

public class Block : MonoBehaviour , IInteractable
{
    public void Interact()
    {
        Debug.Log("Det funkade!");
    }
}
