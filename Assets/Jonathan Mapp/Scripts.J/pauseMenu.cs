using UnityEngine;

public class pauseMenu : MonoBehaviour
{

    public GameObject pausePanal;

    private GameObject player;
    private bool isPaused;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
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
        player.GetComponent<MPlayermovement>().canmove = false;
        isPaused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pausePanal.SetActive(false);
        player.GetComponent<MPlayermovement>().canmove = true;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
}
