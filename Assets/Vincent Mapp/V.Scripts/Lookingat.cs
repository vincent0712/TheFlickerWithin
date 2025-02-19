using UnityEngine;

public class Lookingat : MonoBehaviour
{
    public float maxDistance = 10f; // Maximum distance to detect objects
    public LayerMask layerMask; // Set this to the layers that contain objects with the Outline script

    private Outline currentOutline;
    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main; // Assumes the main camera is the player's camera
    }

    void Update()
    {
        CheckForOutlineObject();
    }

    void CheckForOutlineObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, maxDistance, layerMask))
        {
            Outline outline = hit.collider.GetComponent<Outline>();
            if (outline != null)
            {
                if (currentOutline != outline)
                {
                    DisableCurrentOutline();
                    EnableOutline(outline);
                }
                return;
            }
        }
        DisableCurrentOutline();
    }

    void EnableOutline(Outline outline)
    {
        currentOutline = outline;
        currentOutline.enabled = true;
    }

    void DisableCurrentOutline()
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }
    }
}
