using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class buttenMenu : MonoBehaviour
{

    public Animation fade;
    public AudioSource au;
    private void Awake()
    {
        
        Cursor.lockState = CursorLockMode.None;
    }
    public void MoveToScene(string scene)
    {
        au.Play();
        StartCoroutine(wait(scene));

    }


    public void QuitGame()
    {
        Application.Quit();
    }


    public IEnumerator wait(string name)
    {
        fade.Play("fadeout");
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;
        SceneManager.LoadScene(name);
    }

}
