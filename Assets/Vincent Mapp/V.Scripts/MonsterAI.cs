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

    }



    void Update()
    {

        float speed = agent.velocity.magnitude;
        anim.SetFloat("Speed", speed);
        CheckPlayer();

        if (currentState != lastState) // Detect when state changes
        {
            HandleSounds(); // Restart sounds when state changes
            lastState = currentState; // Update last known state
        }

        if (currentState == State.Chasing && movement.isHidden)
        {
            Debug.Log("Player is hidden, starting search...");
            StartSearching();
        }
        else if (CanSeePlayer())
        {
            movement.isSpotted = true;
            currentState = State.Chasing;
            searching = false;
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
        }
        else if (!CanSeePlayer() && currentState == State.Chasing)
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
        while (true) // Keep checking state
        {
            if (!au.isPlaying) // Play a new sound only when audio stops
            {
                AudioClip[] soundArray = null;

                if (currentState == State.Roaming || currentState == State.Searching || currentState == State.Investigating)
                {
                    soundArray = monsterroamsounds;
                    au.pitch = 1f;
                }
                else if (currentState == State.Chasing)
                {
                    soundArray = monsterchasesound;
                    au.pitch = 0.85f;
                }

                if (soundArray != null && soundArray.Length > 0)
                {
                    int randomIndex = Random.Range(0, soundArray.Length);
                    au.clip = soundArray[randomIndex];
                    au.Play();
                }
            }

            yield return new WaitForSeconds(1f); // Keep checking state changes
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