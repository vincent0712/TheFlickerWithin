using System.Collections;
using TMPro;
using UnityEngine;

public class Timeer : MonoBehaviour
{
    public int duration = 25;
    public int timeRemaining;
    private bool isCountingDown = false;
    public TextMeshProUGUI timerTxT;
    public bool gameOver = false;
    private Fusebox fuse;
    public bool win = false;
    public Puzzlehandeler puzzlehandeler;
    private AudioSource fuseaudio;
    private Coroutine countdownCoroutine;

    private void Start()
    {
        fuse = GameObject.FindGameObjectWithTag("fuse").GetComponent<Fusebox>();
        fuseaudio = GameObject.FindGameObjectWithTag("fuse").GetComponent<AudioSource>();
        timerTxT.text = duration.ToString();
    }

    private void OnEnable()
    {
        // Reset the timer when the minigame is enabled
        timeRemaining = duration;
        timerTxT.text = timeRemaining.ToString();
        isCountingDown = true;

        // Start the timer
        if (countdownCoroutine == null)
        {
            countdownCoroutine = StartCoroutine(Countdown());
        }
    }

    private void OnDisable()
    {
        // Stop the timer when the minigame is disabled
        isCountingDown = false;
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
    }

    private IEnumerator Countdown()
    {
        while (timeRemaining > 0 && isCountingDown)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining--;
            timerTxT.text = timeRemaining.ToString();
        }

        if (isCountingDown) // Ensure game logic triggers only if the game wasn't turned off
        {
            isCountingDown = false;
            gameOver = true;
            win = true;
            fuse.PuzzlesCompleted++;
            fuseaudio.Play();
            puzzlehandeler.Turnoffgame();
        }

        // Reset coroutine reference when done
        countdownCoroutine = null;
    }
}
