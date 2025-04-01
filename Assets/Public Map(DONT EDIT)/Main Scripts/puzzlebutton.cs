using UnityEngine;

public class puzzlebutton : MonoBehaviour, MInteractable
{
    public int buttonIndex; // Assign a unique index (0-3) in the Inspector
    public bool isEnter = false; // Set to true if this is the enter button
    private colorgame puzzleManager;
    private Renderer buttonRenderer;
    private Color originalColor;

    void Start()
    {
        puzzleManager = FindObjectOfType<colorgame>();
        buttonRenderer = GetComponent<Renderer>();
        originalColor = buttonRenderer.material.color;
    }

    void Update()
    {
        CheckForInteraction();
    }

    void CheckForInteraction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.transform == transform)
        {
            buttonRenderer.material.color = Color.white; // Highlight when looked at
        }
        else
        {
            buttonRenderer.material.color = originalColor; // Reset color
        }
    }

    public void Interact()
    {
        if (isEnter)
        {
            puzzleManager.EnterPressed();
            Debug.Log("Enter Button Pressed");
        }
        else
        {
            puzzleManager.ButtonPressed(buttonIndex);
            Debug.Log("Pressed Button: " + buttonIndex);
        }
    }
}
