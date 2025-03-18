using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPickup : MonoBehaviour, MInteractable
{
    [SerializeField] public Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRb;


    [SerializeField] private float pickupRange = 5f;


    private void Update()
    {
        
    }

    public void Interact()
    {
        if(heldObj == null)
        {
            if(transform.gameObject.tag == "Ring")
            {
                heldObj = gameObject;
                heldObjRb = gameObject.GetComponent<Rigidbody>();
                Debug.Log(heldObj);
                //heldObjRb.useGravity = false;
                //heldObjRb.drag = 10;
            }
        }
    }
}