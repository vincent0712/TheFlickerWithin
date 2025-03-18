using System.Collections;
using System.Collections.Generic;
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
    private bool searching = false;
    private float currentVisionRange;

    void Start()
    {
        movement = player.GetComponent<Movement>();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Roam());
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
            currentState = State.Investigating;
            agent.SetDestination(soundPosition);
            StartCoroutine(Investigate());
        }
    }

    IEnumerator Investigate()
    {
        yield return new WaitForSeconds(investigateTime);
        currentState = State.Roaming;
        StartCoroutine(Roam());
    }

    IEnumerator Roam()
    {
        while (currentState == State.Roaming)
        {
            if (Random.value > 0.5f && pointsOfInterest.Length > 0)
            {
                Transform randomPoint = pointsOfInterest[Random.Range(0, pointsOfInterest.Length)];
                agent.SetDestination(randomPoint.position);
            }
            else
            {
                Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
                randomDirection += transform.position;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }
            }
            agent.speed = roamSpeed;
            yield return new WaitForSeconds(Random.Range(roamWaitTimeMin, roamWaitTimeMax));
        }
    }

    void StartSearching()
    {
        if (!searching)
        {
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
                if (hit.transform == player)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Vector3.Distance(transform.position, player.position) < hearingRange)
        {
            currentState = State.Chasing;
            agent.SetDestination(player.position);
        }
    }

    void DebugVisionAndHearing()
    {
        Debug.DrawRay(transform.position, transform.forward * currentVisionRange, Color.blue);

        Vector3 leftLimit = Quaternion.Euler(0, -fieldOfView / 2, 0) * transform.forward * currentVisionRange;
        Vector3 rightLimit = Quaternion.Euler(0, fieldOfView / 2, 0) * transform.forward * currentVisionRange;

        Debug.DrawRay(transform.position, leftLimit, Color.green);
        Debug.DrawRay(transform.position, rightLimit, Color.green);

        Debug.DrawRay(transform.position, Vector3.forward * hearingRange, Color.red);
        Debug.DrawRay(transform.position, Vector3.back * hearingRange, Color.red);
        Debug.DrawRay(transform.position, Vector3.left * hearingRange, Color.red);
        Debug.DrawRay(transform.position, Vector3.right * hearingRange, Color.red);
    }
}