using TMPro; // Korrekt namespace för TextMeshPro
using UnityEngine;
using UnityEngine.UI; // För att kunna använda UI Slider

public class Emf : MonoBehaviour
{
    private GameObject monster;
    public TextMeshPro textMesh; // Använd TextMeshPro för 3D-text
    public Slider emfSlider; // Referens till UI Slider

    public float maxMeterValue = 10f; // Maxvärde för mätaren
    public float frequency = 10f; // Hur snabbt mätaren vibrerar
    public float intensityMultiplier = 2f; // Hur mycket den påverkas av avståndet

    void Start()
    {
        // Hitta monstret genom dess tagg (se till att det har denna tagg)
        monster = GameObject.FindGameObjectWithTag("monster");

        if (monster == null)
        {
            Debug.LogError("Monster not found! Make sure the monster has the correct tag.");
        }

        if (emfSlider == null)
        {
            Debug.LogError("Slider reference is missing! Please assign it in the inspector.");
        }
    }

    void Update()
    {
        if (monster != null && textMesh != null && emfSlider != null)
        {
            float distance = Vector3.Distance(transform.position, monster.transform.position);

            // Beräkna intensitet baserat på avstånd (närmare = starkare rörelse)
            float intensity = Mathf.Clamp(maxMeterValue / (distance + 1), 0, maxMeterValue);

            // Skapa en oscillation som ändras över tid
            float vibration = Mathf.PerlinNoise(Time.time * frequency, 0) * intensityMultiplier;

            // Räkna ut det slutgiltiga mätarvärdet och begränsa det inom maxvärdet
            float meterValue = Mathf.Clamp(vibration * intensity, 0, maxMeterValue);

            // Uppdatera texten med värdet
            textMesh.text = "EMF: " + meterValue.ToString("F1");

            // Uppdatera sliderns värde baserat på EMF-värdet
            emfSlider.value = meterValue;
        }
    }
}
