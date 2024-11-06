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

        Ray ray = new Ray(cam.transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}
