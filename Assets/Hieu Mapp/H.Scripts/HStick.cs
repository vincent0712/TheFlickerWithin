using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HStick : MonoBehaviour, IInteractable
{
    //private CapsuleCollider c;
    private MeshRenderer mesh;

    private CapsuleCollider cap;
    public Color color = Color.red;
    public Color orgCol = Color.green;
    bool isvis = true;
    private Renderer rend;

    public void Start()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();
        cap = gameObject.GetComponent<CapsuleCollider>();
        rend = gameObject.GetComponent<Renderer>();
    }



    public void Interact()
    {
        if (isvis)
        {
            cap.enabled = false;
            rend.material.color = color;
            //mesh.enabled = false;
            isvis = false;
            
        }
            
        else if (!isvis)
        {
            cap.enabled = true;
            rend.material.color = orgCol;
            //mesh.enabled = true;
            isvis = true;
        }
            

    }

}
