using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HStick : MonoBehaviour, IInteractable
{
    //private CapsuleCollider c;
    private MeshRenderer mesh;
    private Renderer rend;
    private CapsuleCollider cap;

    public Color color = Color.red;
    public Color orgCol = Color.green;

    bool isvis = true;
    private int count;
    


    public void Start()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();
        cap = gameObject.GetComponent<CapsuleCollider>();
        rend = gameObject.GetComponent<Renderer>();
    }


    public void Update()
    {
        count = CountDisabledColliders("MinigameObject");
    }


    public void Interact()
    {
        if (isvis && count < 3)
        {
            cap.enabled = false;
            rend.material.color = color;
            //mesh.enabled = false;
            isvis = !isvis;
        }
            
        else if (!isvis)
        {
            cap.enabled = true;
            rend.material.color = orgCol;
            //mesh.enabled = true;
            isvis = !isvis;
        }
            

    }

    public int CountDisabledColliders(string tag)
    {
        int count = 0;
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in objects)
        {
            Collider col = obj.GetComponent<Collider>();
            if (col != null && !col.enabled)
            {
                count++;
            }
        }

        return count;
    }

}
