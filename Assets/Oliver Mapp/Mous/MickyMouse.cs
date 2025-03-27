using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MickyMouse : MonoBehaviour
{
    public float detectionRange = 5f;
    public List<Transform> mouseHoles;
    public Transform player;

    private NavMeshAgent agent;
    private Vector3 randomTarget;
    private bool fleeing = false;
    private bool canplaysound = true;
    private AudioSource au;
    public MonsterAI monster;

    void Start()
    {
        au = gameObject.GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        SetRandomTarget();
    }

    void Update()
    {


        if(fleeing && canplaysound)
        {

            au.pitch = Random.Range(0.85f, 1.15f);
            au.Play();
            monster.HearSound(gameObject.transform.position, 0.45f);
            canplaysound = false;
        }

        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            fleeing = true;
            Transform nearestHole = FindNearestMouseHole();
            agent.SetDestination(nearestHole.position);
        }
        else if (!fleeing && agent.remainingDistance < 0.5f)
        {

            SetRandomTarget();
            agent.SetDestination(randomTarget);
        }
    }

    void SetRandomTarget()
    {
        Vector3 randomPoint;
        NavMeshHit hit;
        int attempts = 0;

        while (attempts < 10) // Försök upp till 10 gånger
        {
            randomPoint = transform.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));

            if (NavMesh.SamplePosition(randomPoint, out hit, 5.0f, NavMesh.AllAreas))
            {
                randomTarget = hit.position;
                agent.SetDestination(randomTarget); // Flytta hit direkt
                return;
            }
            attempts++;
        }

        Debug.LogWarning("Misslyckades att hitta en bra NavMesh-position.");
    }



    Transform FindNearestMouseHole()
    {
        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform hole in mouseHoles)
        {
            float distance = Vector3.Distance(transform.position, hole.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = hole;
            }
        }
        return nearest;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (mouseHoles.Contains(other.transform))
        {
            Transform newHole = mouseHoles[Random.Range(0, mouseHoles.Count)];
            agent.Warp(newHole.position);
            fleeing = false;
            canplaysound = true;
            SetRandomTarget();
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}