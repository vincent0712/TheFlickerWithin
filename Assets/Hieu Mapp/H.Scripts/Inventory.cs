using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<GameObject> inventory = new List<GameObject>();

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AddToInventory(GameObject item)
    {
        inventory.Add(item);
        item.SetActive(false);  // Hide the item in the scene
    }

    public void RemoveFromInventory(GameObject item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
            item.SetActive(true);
        }
    }
}
