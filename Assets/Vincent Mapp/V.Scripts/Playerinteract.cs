using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerinteract : MonoBehaviour
{
    public Camera cam;
    public Camera cam2;
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

        Ray ray2 = new Ray(cam2.transform.position, transform.forward);
        RaycastHit hit2;

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer) || Physics.Raycast(ray2, out hit2, rayDistance, interactableLayer))
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
