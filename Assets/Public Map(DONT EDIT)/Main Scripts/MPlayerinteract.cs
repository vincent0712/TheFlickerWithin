using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPlayerinteract : MonoBehaviour
{
    public Camera cam;

    public float interactDistance = 3f;
    private bool caninteract = true;

    public float rayDistance = 6f;
    public LayerMask interactableLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && caninteract)
        {
            
            Interact();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartDoorBalls();
        }
    }

    public void Interact()
    {
        
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;



        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            MInteractable interactable = hit.collider.GetComponent<MInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
    public void RestartDoorBalls()
    {
        float interactRage = 2f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRage);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out NPCInteractable npcInteractable))
            {
                npcInteractable.Interact();

            }
        }
    }

}
