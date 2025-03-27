using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class monstersounds : MonoBehaviour
{

    public AudioSource au;
    public float delay = 2f;

    private void Start()
    {
        StartCoroutine(endgameeee());
    }


    IEnumerator endgameeee()
    {
        yield return new WaitForSeconds(delay);
        au.Play();

    }



}
