using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class WinorLose : MonoBehaviour
{
    public GameObject[] VadSkaViDöda;
    public Puzzlehandeler puzzlehandeler;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Debug.Log("Lossé");
            Thread.Sleep(1000);

            foreach (GameObject obj in VadSkaViDöda)
            {
                puzzlehandeler.game.SetActive(false);
            }
        }
    }
}
