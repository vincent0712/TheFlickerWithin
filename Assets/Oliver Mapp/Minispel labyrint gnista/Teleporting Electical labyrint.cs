using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Electrical_Labyrint : MonoBehaviour
{
    public Transform target; 
    public LayerMask obsticallayers; 
    public GameObject particleEffect; 
    public Vector3[] teleportPositions; 
    private bool isTeleporting = false;
    protected NavMeshAgent nav;
    private bool canMove = true;
    public GameObject gnistaSpawn;
    public Timeer timer;
    public Puzzlehandeler puzzlehandler;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();

        if (nav == null)
        {
            //Debug.LogError("NavMeshAgent är inte tillagd på objektet!");
        }
    }

    private void OnEnable()
    {
        gameObject.transform.position = gnistaSpawn.transform.position;
        timer.timeRemaining = timer.duration;
        
    }

    void Update()
    {
        if (canMove && !isTeleporting)
        {
            if (!nav.pathPending && nav.pathStatus != NavMeshPathStatus.PathComplete)
            {
                StartCoroutine(TeleportAfterDelay());
            }
            else
            {
                // Annars fortsätt röra dig mot målet
                MoveTo(target.position);
            }
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance <= 20f)
            puzzlehandler.Turnoffgame();
            

    }

    // Funktion för att röra sig till målet
    public void MoveTo(Vector3 position)
    {
        if (nav != null)
        {
            nav.SetDestination(position);
        }
    }

    // Funktion för att teleportera objektet om ingen väg hittades
    IEnumerator TeleportAfterDelay()
    {
        isTeleporting = true;
        canMove = false;
        yield return new WaitForSeconds(1f);

        if (particleEffect != null)
        {
            Instantiate(particleEffect, transform.position, Quaternion.identity);
        }

        // Välj en slumpmässig teleportposition från teleportPositions
        Vector3 teleportPosition = teleportPositions[Random.Range(0, teleportPositions.Length)];
        transform.position = teleportPosition;

        nav.enabled = false;
        nav.enabled = true;

        isTeleporting = false;
        canMove = true;
    }
}