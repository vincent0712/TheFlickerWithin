using TMPro; // Correct namespace for TextMeshPro
using UnityEngine;

public class Emf : MonoBehaviour
{
    private GameObject monster;
    public TextMeshPro textMesh; // Use TextMeshPro instead of legacy TextMesh

    void Start()
    {
        // Find the monster by tag (ensure the monster has this tag)
        monster = GameObject.FindGameObjectWithTag("monster");

        if (monster == null)
        {
            Debug.LogError("Monster not found! Make sure the monster has the correct tag.");
        }
    }

    void Update()
    {
        if (monster != null && textMesh != null)
        {
            float distance = Vector3.Distance(transform.position, monster.transform.position);
            textMesh.text = "Dis: " + distance.ToString("F0"); // Show only 2 decimal places
        }
    }
}
