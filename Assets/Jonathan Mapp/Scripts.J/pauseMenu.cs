using UnityEngine;

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
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        pausePanal.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
    }
}
