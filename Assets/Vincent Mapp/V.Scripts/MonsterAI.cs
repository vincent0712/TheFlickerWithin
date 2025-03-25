using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public enum State { Roaming, Chasing, Searching, Investigating }
    public State currentState = State.Roaming;

    [SerializeField] private Transform player;
    [SerializeField] private float baseVisionRange = 15f;
    [SerializeField] private float crouchingVisionRange = 5f;
    [SerializeField] private float fieldOfView = 60f;
    [SerializeField] private float hearingRange = 25f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float roamSpeed = 2f;
    [SerializeField] private float searchTime = 5f;
    [SerializeField] private float investigateTime = 3f;
    [SerializeField] private float roamRadius = 10f;
    [SerializeField] private float roamWaitTimeMin = 10f;
    [SerializeField] private float roamWaitTimeMax = 15f;
    [SerializeField] private Transform[] pointsOfInterest;

    private Movement movement;
    private NavMeshAgent agent;
    private Vector3 lastKnownPosition;
    public bool searching = false;
    private float currentVisionRange;
    public Animator anim;
    public float spawnTimer = 15f;


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
        float speed = agent.velocity.magnitude;
        anim.SetFloat("Speed", speed);
        CheckPlayer();

        if (currentState == State.Chasing && movement.isHidden)
        {
            Debug.Log("Player is hidden, starting search...");
            StartSearching();
        }
        else if (CanSeePlayer())
        {
            movement.isSpotted = true;
            currentState = State.Chasing;
            searching = false;  // Reset searching so it can trigger when losing sight
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
        }
        else if (currentState == State.Chasing)
        {
            movement.isSpotted = false;
            StartSearching();
        }

        if (CanSeePlayer())
        {
            movement.isSpotted = true;
            currentState = State.Chasing;
            searching = false;  // Reset searching so it can start again if needed
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
        }
        else if(!CanSeePlayer() && currentState == State.Chasing)
        {
            movement.isSpotted = false;
            if (currentState == State.Chasing)
            {
                StartSearching();
            }
        }
    }

    void CheckPlayer()
    {
        currentVisionRange = movement.isCrouching ? crouchingVisionRange : baseVisionRange;
    }



    public void HearSound(Vector3 soundPosition, float soundStrength)
    {
        float effectiveHearingRange = hearingRange * soundStrength;
        if (Vector3.Distance(transform.position, soundPosition) < effectiveHearingRange)
        {
            Debug.Log("Monster heard a noise and is investigating!");
            StopAllCoroutines(); // Stop Roaming Coroutine
            agent.ResetPath(); // Stop current movement
            currentState = State.Investigating;
            agent.SetDestination(soundPosition);
            StartCoroutine(Investigate());
        }
    }


    IEnumerator Investigate()
    {
        Debug.Log("Monster is investigating...");
        yield return new WaitForSeconds(investigateTime);

        if (currentState == State.Investigating) // Ensures the state hasn't changed to chasing
        {
            currentState = State.Roaming;
            StartCoroutine(Roam());
        }
    }


    IEnumerator Roam()
    {
        while (currentState == State.Roaming)
        {
            Vector3 destination;

            if (Random.value > 0.35f && pointsOfInterest.Length > 0)
            {
                List<Transform> farthestPoints = new List<Transform>();
                List<float> distances = new List<float>();

                foreach (Transform point in pointsOfInterest)
                {
                    float distance = Vector3.Distance(transform.position, point.position);
                    if (CanReachDestination(point.position))
                    {
                        farthestPoints.Add(point);
                        distances.Add(distance);
                    }
                }

                if (farthestPoints.Count > 0)
                {
                    // Sort by distance descending
                    var sortedPoints = farthestPoints.Zip(distances, (p, d) => new { Point = p, Distance = d })
                                                     .OrderByDescending(pd => pd.Distance)
                                                     .Take(3)
                                                     .ToList();

                    // Pick one randomly from the top 3
                    Transform chosenPoint = sortedPoints[Random.Range(0, sortedPoints.Count)].Point;
                    destination = chosenPoint.position;
                    Debug.Log("Going to: " + chosenPoint.name);
                    agent.SetDestination(destination);
                    agent.speed = roamSpeed;

                    yield return new WaitUntil(() => HasReachedDestination());
                    yield return new WaitForSeconds(Random.Range(roamWaitTimeMin, roamWaitTimeMax));
                }
                else
                {
                    continue; // No reachable point found, try again
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
                        agent.SetDestination(destination);
                        agent.speed = roamSpeed;

                        yield return new WaitUntil(() => HasReachedDestination());
                        yield return new WaitForSeconds(Random.Range(roamWaitTimeMin, roamWaitTimeMax));
                    }
                    else
                    {
                        continue; // Skip and try again
                    }
                }
                else
                {
                    continue; // Skip and try again
                }
            }
        }
    }

    void StartSearching()
    {
        if (!searching)
        {
            agent.speed = roamSpeed;
            searching = true;
            currentState = State.Searching;
            lastKnownPosition = player.position;
            agent.SetDestination(lastKnownPosition);
            StartCoroutine(Search());
        }
    }

    IEnumerator Search()
    {
        yield return new WaitForSeconds(searchTime);
        Transform randomPoint = pointsOfInterest[Random.Range(0, pointsOfInterest.Length)];
        Vector3 destination;
        destination = randomPoint.position;
        agent.SetDestination(destination);
        agent.speed = roamSpeed;
        //Debug.Log("Going to: " + randomPoint.name);

        currentState = State.Roaming;
        searching = false;

        StartCoroutine(Roam());
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
                if (hit.transform == player && !movement.isHidden)
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool CanReachDestination(Vector3 destination)
    {
        NavMeshPath path = new NavMeshPath();
        bool hasPath = agent.CalculatePath(destination, path);
        return hasPath && path.status == NavMeshPathStatus.PathComplete;
    }

    bool HasReachedDestination()
    {
        if(currentState == State.Chasing && movement.isHidden)
        {
            return true;
        }
        if (!agent.pathPending) // Make sure path calculation is done
        {
            if (agent.remainingDistance <= agent.stoppingDistance) // Check if the agent is at the destination
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) // Ensure agent has stopped moving
                {
                    return true;
                }
            }
        }
        return false;
    }



}