using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    // Public fields
    public float speed;
    public float wanderRadius = 5f;
    public float wanderInterval = 3f;
    public List<GameObject> rooms;

    // Private fields
    private NavMeshAgent nav;
    private bool isWaiting = false;
    private bool isLooking = false;
    private List<GameObject> unvisitedRooms;
    public float rotationSpeed = 5f;
    public float lookAroundDuration = 2f;

    void Start()
    {
        nav = gameObject.GetComponent<NavMeshAgent>();
        if (nav == null)
        {
            Debug.LogError("NavMeshAgent is missing!");
            return;
        }

        nav.speed = speed;

        // Initialize unvisited rooms list
        unvisitedRooms = new List<GameObject>(rooms);
    }

    void Update()
    {
        if (nav == null || isWaiting)
            return;

        if (!nav.pathPending && nav.remainingDistance <= nav.stoppingDistance)
        {
            if (!nav.hasPath || nav.velocity.sqrMagnitude == 0f)
            {

                StartCoroutine(WaitBeforeNextDecision());
            }
        }
    }


    private void MakeDecision()
    {
        int decision = Random.Range(1, 6); // Updated to include look-around behavior
        Debug.Log($"Decision: {decision}");

        if (decision == 2 || decision == 3 || decision == 4)
            Roam();
        else if (decision == 1)
            GoToRoom();
        else if (decision >= 5)
            StartCoroutine(LookAround());
    }

    private void Roam()
    {
        Vector3 newPos = GetRandomPoint(transform.position, wanderRadius);
        Debug.Log($"Roaming to {newPos}");
        GoTo(newPos);
    }

    private void GoToRoom()
    {
        if (unvisitedRooms.Count == 0)
        {
            // Reset the list if all rooms have been visited
            unvisitedRooms = new List<GameObject>(rooms);
            Debug.Log("All rooms visited. Resetting the list.");
        }

        if (unvisitedRooms.Count > 0)
        {
            GameObject targetRoom = unvisitedRooms[Random.Range(0, unvisitedRooms.Count)];
            unvisitedRooms.Remove(targetRoom); // Mark the room as visited
            Debug.Log($"Going to room: {targetRoom.name}");
            GoTo(targetRoom.transform.position);
        }
    }

    private IEnumerator LookAround()
    {
        isLooking = true;
        Debug.Log("Looking around...");

        float elapsedTime = 0f;
        while (elapsedTime < lookAroundDuration)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime * Random.Range(120,300) / lookAroundDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Finished looking around.");
        isLooking = false;
    }

    private void GoTo(Vector3 pos)
    {
        NavMeshHit hit;
        // Check if the position is on the NavMesh within a certain distance
        if (NavMesh.SamplePosition(pos, out hit, 1.0f, NavMesh.AllAreas))
        {
            nav.SetDestination(hit.position); // Set destination to the valid NavMesh point
            Debug.Log($"Moving to valid position on NavMesh: {hit.position}");
        }
        else
        {
            Debug.LogWarning($"Target position {pos} is not on the NavMesh. Ignoring move command.");
        }
    }

    private Vector3 GetRandomPoint(Vector3 origin, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += origin;

        NavMeshHit navHit;
        // Sample position to ensure it's on the NavMesh
        if (NavMesh.SamplePosition(randomDirection, out navHit, radius, NavMesh.AllAreas))
        {
            return navHit.position; // Return a valid point
        }

        // If no valid position found, fallback to the origin
        Debug.LogWarning("Failed to find a valid random point on the NavMesh.");
        return origin;
    }

    private IEnumerator WaitBeforeNextDecision()
    {
        isWaiting = true;

        float waitTime = Random.Range(2f, 4f);

        yield return new WaitForSeconds(waitTime);
        isWaiting = false;

        MakeDecision();
    }
}
