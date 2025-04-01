using UnityEngine;

public class atticlock : MonoBehaviour, MInteractable
{
    public Key key;
    private bool isopen;
    public Door door;


    public void Interact()
    {
        if (isopen)
            return;
        if (key.haskey)
        {
            gameObject.active = false;
            door.isLocked = false;
        }
    }

}
