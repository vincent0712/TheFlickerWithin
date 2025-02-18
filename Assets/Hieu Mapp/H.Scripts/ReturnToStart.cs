using UnityEngine;

public class ReturnToStart : MonoBehaviour
{
    Vector3 startPosition;
    Quaternion startRotation;

    public NPCInteractable npcInteractable;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

    }

    public void OnDisable()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;

    }
}
