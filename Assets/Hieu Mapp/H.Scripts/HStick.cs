using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HStick : MonoBehaviour, HInteract
{
    private CapsuleCollider c;

    private void Start()
    {
        c = gameObject.GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            c.enabled = false;
        }
    }

    public void Interact()
    {
        CapsuleCollider c = gameObject.GetComponent<CapsuleCollider>();
        c.enabled = false;
        
    }

}
