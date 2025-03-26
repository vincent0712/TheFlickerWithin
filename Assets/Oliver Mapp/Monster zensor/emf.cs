using TMPro; // TextMeshPro namespace
using UnityEngine;
using UnityEngine.UI; // För att kunna använda UI Slider

public class Emf : MonoBehaviour
{
    private GameObject monster;
    public TextMeshPro textMesh; // Använd TextMeshPro för 3D-text
    public Slider emfSlider; // Referens till UI Slider
    public  AudioClip emfSound; // Ljudkälla för EMF-mätaren
    private AudioSource AU;

    public float maxMeterValue = 10f; // Maxvärde för mätaren
    public float frequency = 10f; // Hur snabbt mätaren vibrerar
    public float intensityMultiplier = 2f; // Hur mycket den påverkas av avståndet

    public float minPitch = 0.5f; // Lägsta tonhöjd
    public float maxPitch = 2.0f; // Högsta tonhöjd
    public float minVolume = 0.1f; // Lägsta volym
    public float maxVolume = 1.0f; // Högsta volym

    void Start()
    {
        AU = gameObject.GetComponent<AudioSource>();
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

        if (emfSound == null)
        {
            Debug.LogError("AudioSource reference is missing! Please assign it in the inspector.");
        }
        else
        {
            AU.loop = true; // Se till att ljudet loopar
            AU.Play(); // Starta ljudet direkt men vi styr volym och pitch
        }
    }

    void Update()
    {
        if (monster != null && textMesh != null && emfSlider != null && emfSound != null)
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

            // Justera ljudets volym och pitch baserat på EMF-värdet
            AU.volume = Mathf.Lerp(minVolume, maxVolume, meterValue / maxMeterValue);
            AU.pitch = Mathf.Lerp(minPitch, maxPitch, meterValue / maxMeterValue);
        }
    }
}
