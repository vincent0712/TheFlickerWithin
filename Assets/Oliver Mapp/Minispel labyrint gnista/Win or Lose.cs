using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class WinorLose : MonoBehaviour
{
    public GameObject[] VadSkaViD�da;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Debug.Log("Losse");
            Thread.Sleep(1000);

            foreach (GameObject obj in VadSkaViD�da)
            {
                Destroy(obj); // F�rst�r objekten i arrayen
            }
        }
    }
}
