using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPickUp : MonoBehaviour
{
    public static HPickUp Instance { get; private set; }

    public void Awake()
    {
       if(Instance == null)
        {
            Instance = this;
        }
       else
        {
            Destroy(this);
        }
    }
}
