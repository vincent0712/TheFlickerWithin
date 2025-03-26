using System.Collections.Generic;
using UnityEngine;

public class PaintingToggle : MonoBehaviour
{
    public List<Painting> paintings = new List<Painting>(); // List of paintings
    public List<GameObject> objectsToToggle = new List<GameObject>(); // List of objects to toggle
    private bool useFirstMaterial = true; // Toggle state

    public void TogglePaintings(bool useFirst)
    {
        useFirstMaterial = useFirst;

        // Toggle paintings
        foreach (var painting in paintings)
        {
            painting.ToggleMaterial(useFirstMaterial);
        }

        // Toggle additional objects
        foreach (var obj in objectsToToggle)
        {
            if (obj != null)
            {
                obj.SetActive(!useFirstMaterial);
            }
        }
    }
}

[System.Serializable]
public class Painting
{
    public Renderer paintingRenderer; // The renderer of the painting (assign in Inspector)
    public Material NormalMaterial; // First material
    public Material Cameramaterial; // Second material

    public void ToggleMaterial(bool useFirst)
    {
        if (paintingRenderer != null)
        {
            paintingRenderer.material = useFirst ? Cameramaterial : NormalMaterial;
        }
    }
}
