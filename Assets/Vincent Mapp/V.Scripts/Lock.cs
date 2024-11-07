using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Lock : MonoBehaviour, MInteractable
{
    // Start is called before the first frame update
    public GameObject minigame;
    public MPlayermovement player;
    public void Interact()
    {
        
        minigame.SetActive(true);
        player.canmove = false;
        gameObject.SetActive(false);
        

    }
}
