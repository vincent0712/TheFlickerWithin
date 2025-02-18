using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public float visionRange = 10f;
    public float hearingRange = 15f;
    public float normalSpeed = 2f;
    public float chaseSpeed = 3.5f;
    public LayerMask playerLayer;

    private Movement playermovementscript;
    private Flashlight flashlight;

    private enum MonsterState { Patrolling, Searching, Chasing }
    private MonsterState currentState = MonsterState.Patrolling;

    private Vector3 lastKnownPosition;

    private void Start()
    {
        playermovementscript = player.GetComponent<Movement>();
        flashlight = GameObject.FindGameObjectWithTag("fl").GetComponent<Flashlight>();
    }

    void Update()
    {
        AdjustVisionAndHearing();

        // Check if the player is moving within hearing range
        if (Vector3.Distance(transform.position, player.position) < hearingRange && playermovementscript.isMoving)
        {
            HearNoise(player.position);
        }

        switch (currentState)
        {
            case MonsterState.Patrolling:
                Patrol();
                break;
            case MonsterState.Searching:
                Search();
                break;
            case MonsterState.Chasing:
                Chase();
                break;
        }

        CheckPlayerVisibility();
    }

    void AdjustVisionAndHearing()
    {
        if (playermovementscript.isHidden)
        {
            visionRange = 0f;
            hearingRange = 0f;
            return;
        }

        if (playermovementscript.isCrouching && !flashlight.isOn)
        {
            visionRange = 6.5f;
            hearingRange = 16f;
        }
        else if (playermovementscript.isCrouching && flashlight.isOn)
        {
            visionRange = 13f;
            hearingRange = 16f;
        }
        else if (!playermovementscript.isCrouching && !flashlight.isOn)
        {
            visionRange = 13f;
            hearingRange = 16f;
        }
    }

    void Patrol()
    {
        playermovementscript.isSpotted = false;
        if (!agent.hasPath)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * 10f;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    void Search()
    {
        agent.speed = normalSpeed;
        agent.SetDestination(lastKnownPosition);
        playermovementscript.isSpotted = false;

        if (Vector3.Distance(transform.position, lastKnownPosition) < 2f)
        {
            currentState = MonsterState.Patrolling;
        }
    }

    void Chase()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
        playermovementscript.isSpotted = true;
    }

    void CheckPlayerVisibility()
    {
        if (Vector3.Distance(transform.position, player.position) < visionRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit, visionRange))
            {
                if (hit.collider.CompareTag("Player") && !playermovementscript.isHidden)
                {
                    currentState = MonsterState.Chasing;
                    lastKnownPosition = player.position;
                    playermovementscript.isSpotted = true;
                    return;
                }
            }
        }

        if (currentState == MonsterState.Chasing)
        {
            currentState = MonsterState.Searching;
            playermovementscript.isSpotted = false;
        }
    }

    public void HearNoise(Vector3 position)
    {
        if (currentState == MonsterState.Chasing) return; // Don't switch if already chasing

        lastKnownPosition = position;
        currentState = MonsterState.Searching;

        Debug.Log("Monster heard player moving at: " + position);
    }

    void OnDrawGizmos()
    {
        if (player == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hearingRange);

        Gizmos.color = Color.red;
        Vector3 direction = (player.position - transform.position).normalized;
        Gizmos.DrawRay(transform.position, direction * visionRange);

        if (currentState == MonsterState.Searching)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(lastKnownPosition, 0.5f);
        }
    }
}
