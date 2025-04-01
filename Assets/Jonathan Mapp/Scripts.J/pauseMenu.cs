using UnityEngine;

public class pauseMenu : MonoBehaviour
{

    public GameObject pausePanal;

    public Movement player;
    private bool isPaused;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        pausePanal.SetActive(false);
        isPaused = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        player.canmove = false;
        isPaused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pausePanal.SetActive(false);
        player.canmove = true;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
}
