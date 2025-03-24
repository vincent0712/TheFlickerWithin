using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MickyMouse : MonoBehaviour
{
    public enum State { Roaming, Chasing, Searching, Investigating }
    public State currentState = State.Roaming;

    [SerializeField] private Transform player;
    [SerializeField] private float baseVisionRange = 15f;
    [SerializeField] private float fieldOfView = 60f;
    [SerializeField] private float chaseSpeed = -5f;
    [SerializeField] private float roamSpeed = 2f;
    [SerializeField] private float roamRadius = 10f;
    [SerializeField] private float roamWaitTimeMin = 10f;
    [SerializeField] private float roamWaitTimeMax = 15f;
    [SerializeField] private Transform[] pointsOfInterest;

    private Movement movement;
    private NavMeshAgent agent;
    private Vector3 lastKnownPosition;
    private float currentVisionRange;

    void Start()
    {
        movement = player.GetComponent<Movement>();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Roam());

        GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("points");

        // Convert the GameObjects array to a Transforms array
        pointsOfInterest = new Transform[gameObjectsWithTag.Length];
        for (int i = 0; i < gameObjectsWithTag.Length; i++)
        {
            pointsOfInterest[i] = gameObjectsWithTag[i].transform;
        }
    }

    void Update()
    {
        DebugVisionAndHearing();
        CheckPlayer();

        if (CanSeePlayer())
        {
            movement.isSpotted = true;
            currentState = State.Chasing;
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
        }
        else
        {
            currentState = State.Roaming;
        }
    }

    void CheckPlayer()
    {
        currentVisionRange =  baseVisionRange;
    }
    IEnumerator Roam()
    {
        while (currentState == State.Roaming)
        {
            Vector3 destination;

            if (Random.value > 0.35f && pointsOfInterest.Length > 0)
            {
                Transform randomPoint = pointsOfInterest[Random.Range(0, pointsOfInterest.Length)];
                if (CanReachDestination(randomPoint.position))
                {
                    destination = randomPoint.position;
                    Debug.Log("Going to: " + randomPoint.name);
                }
                else
                {
                    Debug.Log("Cannot reach " + randomPoint.name + ", choosing a new point.");
                    continue; // Skip this iteration and try again
                }
            }
            else
            {
                Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
                randomDirection += transform.position;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas))
                {
                    if (CanReachDestination(hit.position))
                    {
                        destination = hit.position;
                    }
                    else
                    {
                        Debug.Log("Random point unreachable, trying again.");
                        continue; // Skip this iteration and try again
                    }
                }
                else
                {
                    Debug.Log("Failed to sample NavMesh position, trying again.");
                    continue; // Skip this iteration and try again
                }
            }

            agent.SetDestination(destination);
            agent.speed = roamSpeed;

            // Wait before selecting a new destination
            yield return new WaitForSeconds(Random.Range(roamWaitTimeMin, roamWaitTimeMax));
        }
    }

    bool CanReachDestination(Vector3 destination)
    {
        NavMeshPath path = new NavMeshPath();
        bool hasPath = agent.CalculatePath(destination, path);
        return hasPath && path.status == NavMeshPathStatus.PathComplete;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentState = State.Chasing;
            agent.SetDestination(player.position);
        }
    }

    bool CanSeePlayer()
    {
        if (movement.isHidden)
            return false;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (Vector3.Distance(transform.position, player.position) < currentVisionRange && angle < fieldOfView / 2)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, currentVisionRange))
            {
                if (hit.transform == player)
                {
                    return true;
                }
            }
        }
        return false;
    }
    void DebugVisionAndHearing()
    {
        Debug.DrawRay(transform.position, transform.forward * currentVisionRange, Color.blue);

        Vector3 leftLimit = Quaternion.Euler(0, -fieldOfView / 2, 0) * transform.forward * currentVisionRange;
        Vector3 rightLimit = Quaternion.Euler(0, fieldOfView / 2, 0) * transform.forward * currentVisionRange;

        Debug.DrawRay(transform.position, leftLimit, Color.green);
        Debug.DrawRay(transform.position, rightLimit, Color.green);
    }
}
