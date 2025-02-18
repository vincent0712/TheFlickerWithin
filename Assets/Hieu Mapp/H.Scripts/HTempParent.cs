using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTempParent : MonoBehaviour
{
    public static HTempParent Instance { get; private set; }

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
