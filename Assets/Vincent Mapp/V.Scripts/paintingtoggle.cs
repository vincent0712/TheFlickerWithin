using System.Collections.Generic;
using UnityEngine;

public class paintingtoggle : MonoBehaviour
{
    public List<Painting> paintings = new List<Painting>(); // List of paintings
    private bool useFirstMaterial = true; // Toggle state

    public void TogglePaintings(bool useFirst)
    {
        useFirstMaterial = useFirst;
        foreach (var painting in paintings)
        {
            painting.ToggleMaterial(useFirstMaterial);
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
