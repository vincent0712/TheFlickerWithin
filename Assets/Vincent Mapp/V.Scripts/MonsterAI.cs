using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public enum State { Roaming, Chasing, Searching, Investigating }
    public State currentState = State.Roaming;

    public Transform player;
    public float visionRange = 15f;
    public float fieldOfView = 60f;
    public float hearingRange = 25f;
    public float chaseSpeed = 5f;
    public float roamSpeed = 2f;
    public float searchTime = 5f;
    public float investigateTime = 3f;

    private Movement movement;

    public Transform[] pointsOfInterest;
    private NavMeshAgent agent;
    private Vector3 lastKnownPosition;
    private bool searching = false;

    void Start()
    {
        movement = player.GetComponent<Movement>();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Roam());
    }

    void Update()
    {
        DebugVisionAndHearing();
        checkplayer();

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

    void checkplayer()
    {
        visionRange = movement.isCrouching ? 5f : 15f;
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
                Vector3 randomDirection = Random.insideUnitSphere * 10f;
                randomDirection += transform.position;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }
            }
            agent.speed = roamSpeed;
            yield return new WaitForSeconds(Random.Range(5, 10));
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

        if (Vector3.Distance(transform.position, player.position) < visionRange && angle < fieldOfView / 2)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, visionRange))
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
        Debug.DrawRay(transform.position, transform.forward * visionRange, Color.blue);

        Vector3 leftLimit = Quaternion.Euler(0, -fieldOfView / 2, 0) * transform.forward * visionRange;
        Vector3 rightLimit = Quaternion.Euler(0, fieldOfView / 2, 0) * transform.forward * visionRange;

        Debug.DrawRay(transform.position, leftLimit, Color.green);
        Debug.DrawRay(transform.position, rightLimit, Color.green);

        Debug.DrawRay(transform.position, Vector3.forward * hearingRange, Color.red);
        Debug.DrawRay(transform.position, Vector3.back * hearingRange, Color.red);
        Debug.DrawRay(transform.position, Vector3.left * hearingRange, Color.red);
        Debug.DrawRay(transform.position, Vector3.right * hearingRange, Color.red);
    }
}
