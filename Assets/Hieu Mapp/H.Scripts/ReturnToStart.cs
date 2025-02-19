using UnityEngine;

public class ReturnToStart : MonoBehaviour
{
    Vector3 startPosition;
    Quaternion startRotation;

    public GameObject ball;

    void Start()
    {
        startPosition = ball.transform.position;
        startRotation = ball.transform.rotation;

    }

    public void OnDisable()
    {
        ball.transform.position = startPosition;
        ball.transform.rotation = startRotation;

    }
}
