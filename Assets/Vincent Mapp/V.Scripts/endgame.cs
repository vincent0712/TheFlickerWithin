using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endgame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        StartCoroutine(endgameeee());
    }

    IEnumerator endgameeee()
    {
        yield return new WaitForSeconds(3f);

        yield return new WaitForSeconds(1);



        SceneManager.LoadScene("startmenuscene");

    }
}
