using UnityEngine;

public class changepainting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Material NocamMaterial;
    public Material Cammaterial;

    public MeshRenderer mesh;
    public bool iscamup;



    public void tunoff()
    {
        mesh.material = NocamMaterial;
    }

    public void turnon()
    {
        mesh.material = Cammaterial;
    }

}
