using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HStick : MonoBehaviour
{
    public string stick;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.G))
        {
            Inventory.instance.AddToInventory(gameObject);
            Debug.Log($"{stick} added to inventory.");
        }
    }
}
