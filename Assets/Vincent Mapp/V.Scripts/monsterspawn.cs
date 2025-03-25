using System.Collections;
using UnityEngine;

public class monsterspawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float spawnTimer = 15f;
    public GameObject monster;
    void Start()
    {
        StartCoroutine(spawndelay());
    }


    public IEnumerator spawndelay()
    {
        yield return new WaitForSeconds(1f);
        monster.SetActive(false);
        yield return new WaitForSeconds(spawnTimer);
        monster.SetActive(true);
    }
}


