using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Lock : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    public GameObject minigame;
    public Playermoveing player;
    public void Interact()
    {
        //minigame = GameObject.FindGameObjectWithTag("Lockgame");
        minigame.SetActive(true);
        player.canmove = false;
        gameObject.SetActive(false);
        

    }
}
