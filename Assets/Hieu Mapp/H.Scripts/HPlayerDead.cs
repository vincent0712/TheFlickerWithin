using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HPlayerDead : MonoBehaviour
{
    public GameObject endgamemonster;
    private GameObject monster;
    public GameObject player;
    public GameObject maincam;
    public GameObject endcam;


    public void Start()
    {
        monster = GameObject.FindGameObjectWithTag("monster");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "monster")
        {

            StartCoroutine(endgame());
            

            

        }
    }
    IEnumerator endgame()
    {
        Instantiate(endgamemonster, monster.transform.position, monster.transform.rotation);
        player.SetActive(false);
        monster.SetActive(false);


        //maincam.active = false;
        //endcam.active = true;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("StartMenu");

    }
}
