using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MickyMouse : MonoBehaviour
{
    public float detectionRange = 5f;
    public List<Transform> mouseHoles;
    public Transform player;

    private NavMeshAgent agent;
    private Vector3 randomTarget;
    private bool fleeing = false;
    private bool canPlaySound = true;
    private AudioSource au;
    public MonsterAI monster;
    private Transform lastHoleUsed;

    void Start()
    {
        au = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        SetRandomTarget();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange && !fleeing)
        {
            StartFleeing();
        }

        if (!fleeing && agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            SetRandomTarget();
        }
    }

    void StartFleeing()
    {
        fleeing = true;
        Transform nearestHole = FindNearestMouseHole();

        if (nearestHole != null)
        {
            agent.SetDestination(nearestHole.position);
        }

        if (canPlaySound)
        {
            PlayFleeSound();
            monster.HearSound(transform.position, 0.45f);
            canPlaySound = false;
        }
    }

    void PlayFleeSound()
    {
        au.pitch = Random.Range(0.85f, 1.15f);
        au.Play();
    }

    void SetRandomTarget()
    {
        Vector3 randomPoint;
        NavMeshHit hit;
        int attempts = 0;

        while (attempts < 10)
        {
            randomPoint = transform.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));

            if (NavMesh.SamplePosition(randomPoint, out hit, 5.0f, NavMesh.AllAreas))
            {
                randomTarget = hit.position;
                agent.SetDestination(randomTarget);
                return;
            }
            attempts++;
        }

        Debug.LogWarning("[MickyMouse] Failed to find a valid NavMesh position.");
    }

    Transform FindNearestMouseHole()
    {
        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform hole in mouseHoles)
        {
            float distance = Vector3.Distance(transform.position, hole.position);

            if (distance < minDistance && hole != lastHoleUsed) // Avoid using the same hole repeatedly
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
            StartCoroutine(TeleportToNewHole());
        }
    }

    IEnumerator TeleportToNewHole()
    {
        agent.isStopped = true; // Stop the NavMeshAgent before warping
        yield return new WaitForEndOfFrame(); // Wait a frame to let Unity process physics updates

        // Pick a different hole for the next escape
        Transform newHole = mouseHoles[Random.Range(0, mouseHoles.Count)];
        while (newHole == lastHoleUsed && mouseHoles.Count > 1)
        {
            newHole = mouseHoles[Random.Range(0, mouseHoles.Count)];
        }

        lastHoleUsed = newHole;
        agent.Warp(newHole.position); // Move the mouse instantly
        yield return new WaitForSeconds(0.1f); // Short delay to let Unity process the teleport

        agent.isStopped = false; // Resume movement
        fleeing = false;
        canPlaySound = true;
        SetRandomTarget();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
