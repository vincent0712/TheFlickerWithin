using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class buttenMenu : MonoBehaviour
{

    public Animation fade;
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void MoveToScene(string scene)
    {
        StartCoroutine(wait(scene));

    }


    public void QuitGame()
    {
        Application.Quit();
    }


    public IEnumerator wait(string name)
    {
        fade.Play("fadeout");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(name);
    }
}
