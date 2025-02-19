using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    Vector3 startPosition;
    Quaternion startRotation;

    public GameObject ball;

    void Start()
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
