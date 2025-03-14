using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPuzzle : MonoBehaviour
{
    public Switch[] switches; // Array av switch-objekt
    public bool[] correctCombination = { true, false, false, true, false, true }; // Rätt kombination
    private bool victory;
    public GameObject BigLamp;
    public GameObject[] SmallLamps;
    public Material onMaterial;
    public Material offMaterial;
    public TMP_Text errorText; // Text UI-komponent

    void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // När användaren klickar
        {
            int wrongCount = 0; // Räkna antalet fel

            for (int i = 0; i < switches.Length; i++)
            {
                if (switches[i].IsOn != correctCombination[i]) // Om lampan är fel
                {
                    wrongCount++; // Öka felräknaren

                    // Uppdatera material på lamporna
                    Renderer lampRenderer = SmallLamps[i].GetComponent<Renderer>();
                    if (lampRenderer != null)
                    {
                        lampRenderer.material = switches[i].IsOn ? onMaterial : offMaterial;
                    }
                }
                else // Om lampan är korrekt
                {
                    // Uppdatera material på lamporna
                    Renderer lampRenderer = SmallLamps[i].GetComponent<Renderer>();
                    if (lampRenderer != null)
                    {
                        lampRenderer.material = switches[i].IsOn ? onMaterial : offMaterial;
                    }
                }
            }

            // Uppdatera texten för felaktiga lampor
            if (errorText != null)
            {
                errorText.text = wrongCount + " :are wrong"; // Visar antal felaktiga
            }

            // Kontrollera om spelet är klart
            int correctCount = 0;
            for (int i = 0; i < switches.Length; i++)
            {
                if (switches[i].IsOn == correctCombination[i]) // Om lampan är korrekt
                {
                    correctCount++;
                }
            }

            if (correctCount == switches.Length) // Om alla är korrekta
            {
                Debug.Log("U win");
                BigLamp.SetActive(true); // Tänd BigLamp
                victory = true;
            }
        }
    }
}
