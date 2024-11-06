using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour, IInteractable
{
    public GameObject holder;
    bool isup = false;
    public void Interact()
    {
        if (isup)
            holder.SetActive(false);
        else if (!isup)
            holder.SetActive(true);
    }
}
