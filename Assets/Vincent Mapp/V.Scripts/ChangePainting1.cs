using System.Collections.Generic;
using UnityEngine;

public class ChangePainting1 : MonoBehaviour
{
    public Material NocamMaterial;
    public Material Cammaterial;
    public MeshRenderer mesh;
    public bool iscamup;

    // Static list to keep track of all ChangePainting instances
    public static List<ChangePainting1> allPaintings = new List<ChangePainting1>();

    private void Awake()
    {
        // Add this instance to the list
        allPaintings.Add(this);
    }

    private void OnDestroy()
    {
        // Remove this instance from the list when destroyed
        allPaintings.Remove(this);
    }

    public void TurnOff()
    {
        mesh.material = NocamMaterial;
    }

    public void TurnOn()
    {
        mesh.material = Cammaterial;
    }

    // Static method to trigger changes for all paintings
    public void ToggleAllPaintings(bool turnOn)
    {
        foreach (var painting in allPaintings)
        {
            if (turnOn)
                painting.TurnOn();
            else
                painting.TurnOff();
        }
    }
}
