using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class WinorLose : MonoBehaviour
{
    public GameObject[] VadSkaViDöda;
    public Puzzlehandeler puzzlehandeler;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("loose"))
        {
            puzzlehandeler.Turnoffgame();
            Debug.LogError("Error: Trigger detected with " + Time.time);
        }
    }
}
