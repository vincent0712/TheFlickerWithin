using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class colorgame : MonoBehaviour
{
    public GameObject[] buttons; // Assign the 4 button GameObjects
    public TextMeshPro[] clues;  // Assign the 4 TextMeshPro clues in the scene
    public GameObject enterButton; // Assign the enter button

    private List<int> correctSequence = new List<int>();
    private List<int> playerInput = new List<int>();
    private Color[] colors = { Color.red, Color.blue, Color.green, Color.yellow };
    private string[] colorNames = { "Red", "Blue", "Green", "Yellow" };

    private bool cangetpoint = true;

    private Fusebox fuse;
    private AudioSource fuseaudio;
    void Start()
    {
        GeneratePuzzle();

        fuse = GameObject.FindGameObjectWithTag("fuse").GetComponent<Fusebox>();
        fuseaudio = GameObject.FindGameObjectWithTag("fuse").GetComponent<AudioSource>();
    }

    void GeneratePuzzle()
    {
        List<int> availableIndexes = new List<int> { 0, 1, 2, 3 };
        correctSequence.Clear();

        for (int i = 0; i < 4; i++)
        {
            int randomIndex = availableIndexes[Random.Range(0, availableIndexes.Count)];
            availableIndexes.Remove(randomIndex);
            correctSequence.Add(randomIndex);

            // Set the clues with the corresponding color and number
            clues[i].text = (i + 1).ToString();
            clues[i].color = colors[randomIndex];
        }
    }

    public void ButtonPressed(int buttonIndex)
    {
        if (playerInput.Count < 4)
        {
            playerInput.Add(buttonIndex);
        }
    }

    public void EnterPressed()
    {
        if (playerInput.Count != 4) return;

        if (!cangetpoint)
            return;

        for (int i = 0; i < 4; i++)
        {
            if (playerInput[i] != correctSequence[i])
            {
                Debug.Log("Incorrect Sequence! Try Again.");
                playerInput.Clear();
                return;
            }
        }

        Debug.Log("Correct Sequence! Puzzle Solved.");

        fuse.PuzzlesCompleted++;
        cangetpoint = false;
        fuseaudio.Play();
    }
}
