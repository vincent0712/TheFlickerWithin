using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class monstersounds : MonoBehaviour
{

    public AudioSource au;
    public float delay = 2f;
    public GameObject blood;

    private void Start()
    {
        StartCoroutine(endgameeee());
    }


    IEnumerator endgameeee()
    {
        yield return new WaitForSeconds(delay);
        blood.SetActive(true);
        au.Play();

    }



}
