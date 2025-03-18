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
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pausePanal.SetActive(false);
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
}
