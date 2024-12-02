using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{

    //Publics
    public float speed;
    public GameObject target;
    public float wanderRadius = 5f;
    public float wanderInterval = 3f;

    public List<GameObject> rooms;
    //Privates
    private NavMeshAgent nav;
    bool canMakedes = true;


    void Start()
    {
        nav = gameObject.GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (nav == null)
            return;


        if(canMakedes)
            Makedesition();



    }

    private void Makedesition()
    {
        canMakedes = false;
        int des = Randomnumber(1,3);
        Debug.Log(des);
        if (des == 1)
            Roam();
        else if(des == 2)
        {
            GoToRoom();
        }

    }
    private void GoTo(Vector3 pos)
    {
        nav.destination = pos;
    }

    private void Roam()
    {
        Vector3 newPos = GetRandomPoint(transform.position, wanderRadius);
        GoTo(newPos);
        StartCoroutine(Wait(5f));
        
        Vector3 GetRandomPoint(Vector3 origin, float radius)
        {

            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += origin;


            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDirection, out navHit, radius, NavMesh.AllAreas);

            return navHit.position;

        }
    }

    private int Randomnumber(int min, int max)
    {
        int desition = Random.Range(min, max);
        return desition;
    }

    IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);

        canMakedes = true;
    }
    private void GoToRoom()
    {
        nav.destination = rooms[Randomnumber(1, GameObject.FindGameObjectsWithTag("room").Length)].transform.position;
        Wait(5);
    }


}
