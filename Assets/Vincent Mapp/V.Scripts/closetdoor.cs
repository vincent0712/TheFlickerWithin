using UnityEngine;

public class closetdoor : MonoBehaviour, MInteractable
{
    private Closet closet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closet = gameObject.GetComponentInParent<Closet>();
    }

    public void Interact()
    {
        closet.openclose();
    }
}
