using UnityEngine;

public class Key : MonoBehaviour, MInteractable
{
    public bool haskey = false;

    public void Interact()
    {
        haskey = true;
        gameObject.SetActive(false);
    }
}
