using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

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
    public AudioClip[] monsterroamsounds;
    public AudioClip[] monsterchasesound;
    public AudioClip[] monsterscreams;
    public Transform visionPoint;
    public BoxCollider chasecollider;

    private List<Vector3> lastVisitedPositions = new List<Vector3>();
    private int maxMemory = 5; // Monster remembers last 5 locations



    private Movement movement;
    private NavMeshAgent agent;
    private Vector3 lastKnownPosition;
    public bool searching = false;
    private float currentVisionRange;
    public Animator anim;
    public float spawnTimer = 15f;
    public AudioSource au;
    public Flashlight flashlight;

    private Coroutine soundCoroutine;
    private State lastState; // Keep track of the last state
    private bool started = false;



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


        HandleSounds();

        started = true;
    }

    private void OnEnable()
    {
        if (started)
            StartCoroutine(Roam());
    }




    void Update()
    {
        float speed = agent.velocity.magnitude;
        anim.SetFloat("Speed", speed);

        bool canSeePlayer = CanSeePlayer(); // Avoid redundant calls
        CheckPlayer();

        if (currentState != lastState)
        {
            HandleSounds();
            lastState = currentState;
        }

        if (currentState == State.Chasing && movement.isHidden)
        {
            Debug.Log("Player is hidden, starting search...");
            StartSearching();
        }
        else if (canSeePlayer)
        {
            movement.isSpotted = true;
            currentState = State.Chasing;
            searching = false;
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
        }
        else if (!canSeePlayer && currentState == State.Chasing)
        {
            movement.isSpotted = false;
            StartSearching();
        }
    }


    void HandleSounds()
    {
        if (soundCoroutine != null)
        {
            StopCoroutine(soundCoroutine); // Stop any ongoing sound coroutine
        }

        soundCoroutine = StartCoroutine(PlaySound());
    }

    IEnumerator PlaySound()
    {
        while (true)
        {
            if (!au.isPlaying)
            {
                AudioClip[] soundArray = currentState switch
                {
                    State.Roaming or State.Searching or State.Investigating => monsterroamsounds,
                    State.Chasing => monsterchasesound,
                    _ => null
                };

                if (soundArray?.Length > 0)
                {
                    au.clip = soundArray[Random.Range(0, soundArray.Length)];
                    au.pitch = (currentState == State.Chasing) ? 0.85f : 1f;
                    au.Play();
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }



    void CheckPlayer()
    {
        currentVisionRange = movement.isCrouching ? crouchingVisionRange : baseVisionRange;

        if (movement.isHidden)
        {
            movement.isSpotted = false;
        }

        if (currentState == State.Chasing)
        {
            chasecollider.enabled = true;
            if (flashlight != null && flashlight.gameObject.activeInHierarchy)
            {
                flashlight.StartFlickering(true);  // Force flicker
            }
        }
        else
        {
            chasecollider.enabled = false;
            if (flashlight != null && flashlight.gameObject.activeInHierarchy)
            {
                flashlight.StopFlickering();
            }
        }
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
            Vector3 destination = Vector3.zero;

            // 20% chance to pick a completely random location (wandering mode)
            if (Random.value < 0.2f)
            {
                destination = GetRandomRoamPosition();
                Debug.Log("[Roam] Choosing completely random exploration!");
            }
            else
            {
                // Normal targeted roaming behavior
                if (pointsOfInterest.Length > 0)
                {
                    var reachablePoints = pointsOfInterest
                        .Where(p => CanReachDestination(p.position))
                        .OrderByDescending(p => Vector3.Distance(transform.position, p.position)) // Prioritize farther points
                        .Take(4) // Select top 4 farthest points
                        .ToList();

                    if (reachablePoints.Count > 0)
                    {
                        // Avoid recently visited locations
                        var filteredPoints = reachablePoints
                            .Where(p => !lastVisitedPositions.Contains(p.position))
                            .ToList();

                        if (filteredPoints.Count > 0)
                        {
                            destination = filteredPoints[Random.Range(0, filteredPoints.Count)].position;
                        }
                        else
                        {
                            destination = reachablePoints[Random.Range(0, reachablePoints.Count)].position;
                        }

                        // Store last visited positions
                        lastVisitedPositions.Add(destination);
                        if (lastVisitedPositions.Count > maxMemory)
                        {
                            lastVisitedPositions.RemoveAt(0);
                        }
                    }
                }
            }

            if (destination != Vector3.zero)
            {
                agent.SetDestination(destination);
                agent.speed = roamSpeed;

                yield return new WaitUntil(HasReachedDestination);
                yield return new WaitForSeconds(Random.Range(roamWaitTimeMin, roamWaitTimeMax));
            }
            else
            {
                Debug.LogWarning("[Roam] No valid destination found! Retrying...");
                yield return null; // Small delay before retrying
            }
        }
    }




    Vector3 GetRandomRoamPosition()
    {
        for (int i = 0; i < 10; i++) // Try up to 10 times
        {
            Vector3 randomDirection = Random.insideUnitSphere * roamRadius * 3f; // Increase roam radius
            randomDirection += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius * 3f, NavMesh.AllAreas))
            {
                if (CanReachDestination(hit.position))
                {
                    return hit.position;
                }
            }
        }
        return Vector3.zero; // Fallback if no valid point is found
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

        // Use visionPoint instead of monsterEyes
        Vector3 visionOrigin = visionPoint.position;
        Vector3 directionToPlayer = (player.position - visionOrigin).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (Vector3.Distance(visionOrigin, player.position) < currentVisionRange && angle < fieldOfView / 2)
        {
            RaycastHit hit;
            if (Physics.Raycast(visionOrigin, directionToPlayer, out hit, currentVisionRange))
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

        if (!hasPath || path.status != NavMeshPathStatus.PathComplete)
        {
            Debug.LogWarning($"[CanReachDestination] Can't reach {destination}. Path status: {path.status}");
            return false;
        }
        return true;
    }

    bool HasReachedDestination()
    {
        if (currentState == State.Chasing && movement.isHidden)
            return true;

        if (agent.pathPending) return false;

        return (agent.remainingDistance <= agent.stoppingDistance) &&
               (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }




}