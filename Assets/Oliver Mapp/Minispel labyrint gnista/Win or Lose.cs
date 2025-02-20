using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class WinorLose : MonoBehaviour
{
    public GameObject[] VadSkaViD�da;
    public Puzzlehandeler puzzlehandeler;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Debug.Log("Loss�");
            Thread.Sleep(1000);

            foreach (GameObject obj in VadSkaViD�da)
            {
                puzzlehandeler.game.SetActive(false);
            }
        }
    }
}
