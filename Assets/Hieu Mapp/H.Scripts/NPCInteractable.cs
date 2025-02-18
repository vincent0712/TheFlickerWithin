using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    public GameObject ball;
    Vector3 startPosition;
    Quaternion startRotation;

    private void Start()
    {
        startPosition = ball.transform.position;
        startRotation = ball.transform.rotation;
    }
    public void Interact()
    {
        ball.transform.position = startPosition;
        ball.transform.rotation = startRotation;
    }
}
