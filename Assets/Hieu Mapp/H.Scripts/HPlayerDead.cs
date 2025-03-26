using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HPlayerDead : MonoBehaviour
{
    public GameObject endgamemonster;
    private GameObject monster;
    public GameObject player;
    public GameObject endgame;
    public Transform point;


    public void Start()
    {
        monster = GameObject.FindGameObjectWithTag("monster");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "monster")
        {

            Instantiate(endgamemonster, point.transform.position, point.transform.rotation);
            player.SetActive(false);
            monster.SetActive(false);

            Instantiate(endgame);




        }
    }



}
