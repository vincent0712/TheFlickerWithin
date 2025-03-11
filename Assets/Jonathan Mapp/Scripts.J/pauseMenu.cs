using UnityEngine;
using UnityEngine.WSA;

public class pauseMenu : MonoBehaviour
{

    public GameObject pausePanal;

    private bool isPaused;

    void Start()
    {
        pausePanal.SetActive(false);
        isPaused = false;
    }
    void Update()
    {
        if (Input.GetButtonDown("Escape"))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        pausePanal.SetActive(true);
        isPaused = true;
        UnityEngine.Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pausePanal.SetActive(false);
        isPaused = false;
        UnityEngine.Cursor.visible = false;
        Time.timeScale = 1;
    }
}
