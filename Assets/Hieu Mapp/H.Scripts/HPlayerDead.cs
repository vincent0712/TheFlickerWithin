using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HPlayerDead : MonoBehaviour
{
    public Animator anim;
    public MonsterAI monster;
    public Movement movement;
    public GameObject monstercordinate;

    public GameObject maincam;
    public GameObject endcam;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "monster")
        {
            SceneManager.LoadScene("Outro");
            StartCoroutine(kill());
            

            

        }
    }

    public void Update()
    {
        if (!monster.isendgame)
            return;
        monstercordinate.transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
    }
    IEnumerator kill()
    {
        monster.isendgame = true;
        //movement.canmove = false;
        anim.SetTrigger("jumpscare");

        


        //maincam.active = false;
        //endcam.active = true;
        yield return new WaitForSeconds(3f);
        //SceneManager.LoadScene("StartMenu");

    }
}
