using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    // Public fields
    public float speed;
    public GameObject target;
    public float wanderRadius = 5f;
    public float wanderInterval = 3f;
    public List<GameObject> rooms;

    // Private fields
    private NavMeshAgent nav;
    private bool canMakeDecision = true;
    private bool isdoingSomething = false;

    void Start()
    {
        nav = gameObject.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (nav == null)
            return;

        if (canMakeDecision)
            MakeDecision();

        CheckDestinationReached();
    }

    private void MakeDecision()
    {
        canMakeDecision = false;
        
        int decision = RandomNumber(1, 3);
        Debug.Log(decision);

        if (decision == 1 && !isdoingSomething)
            Roam();
        else if (decision == 2 && !isdoingSomething)
            GoToRoom();
    }

    private void Roam()
    {
        Vector3 newPos = GetRandomPoint(transform.position, wanderRadius);
        GoTo(newPos);
    }

    private void GoToRoom()
    {
        if (rooms.Count > 0)
        {
            Vector3 roomPosition = rooms[RandomNumber(0, rooms.Count)].transform.position;
            GoTo(roomPosition);
        }

    }

    private void GoTo(Vector3 pos)
    {
        if (!canMakeDecision)
            return;
        nav.destination = pos;
    }

    private void CheckDestinationReached()
    {
        // Check if the NavMeshAgent has reached its destination
        if (!nav.pathPending && nav.remainingDistance <= nav.stoppingDistance)
        {
            if (!nav.hasPath || nav.velocity.sqrMagnitude == 0f)
            {
                StartCoroutine(Wait(RandomNumber(2, 4))); // Wait before making the next decision
            }
        }
    }

    private Vector3 GetRandomPoint(Vector3 origin, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, radius, NavMesh.AllAreas);

        return navHit.position;
    }

    private int RandomNumber(int min, int max)
    {
        return Random.Range(min, max);
    }

    private IEnumerator Wait(float duration)
    {
        canMakeDecision = false;
        yield return new WaitForSeconds(duration);
        canMakeDecision = true;
    }
}
