using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPlayerInteract : MonoBehaviour
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

    }

    public void Interact()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        float interactRage = 2f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRage);
        foreach (Collider collider in colliderArray)
        {
            if(collider.TryGetComponent(out NPCInteractable npcInteractable))
            {
                npcInteractable.Interact();
               
            }
        }

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                Debug.Log(interactable);
                interactable.Interact();
            }
        }
    }
}
