using System.Collections.Generic;
using UnityEngine;

public class MickyMouse : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectionRange = 5f;
    public List<Transform> mouseHoles;
    public Transform player;

    private Vector3 randomTarget;
    private bool fleeing = false;

    void Start()
    {
        SetRandomTarget();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            fleeing = true;
            Transform nearestHole = FindNearestMouseHole();
            MoveTowards(nearestHole.position);
        }
        else if (!fleeing)
        {
            MoveTowards(randomTarget);

            if (Vector3.Distance(transform.position, randomTarget) < 0.5f)
            {
                SetRandomTarget();
            }
        }
    }

    void MoveTowards(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    void SetRandomTarget()
    {
        randomTarget = new Vector3(Random.Range(-10, 10), transform.position.y, Random.Range(-10, 10));
    }

    Transform FindNearestMouseHole()
    {
        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform hole in mouseHoles)
        {
            float distance = Vector3.Distance(transform.position, hole.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = hole;
            }
        }
        return nearest;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (mouseHoles.Contains(other.transform))
        {
            Transform newHole = mouseHoles[Random.Range(0, mouseHoles.Count)];
            transform.position = newHole.position;
            fleeing = false;
        }
    }
}
