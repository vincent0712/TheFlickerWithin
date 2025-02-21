using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timeer : MonoBehaviour
{
    public int duration = 25;
    public int timeRemaining;
    public bool isCountingDown = false;
    public TextMeshProUGUI timerTxT;
    public bool gameOver = false;
    private Fusebox fuse;
    public bool win = false;
    public Puzzlehandeler puzzlehandeler;
    private AudioSource fuseaudio;

    private void Start()
    {
        fuse = GameObject.FindGameObjectWithTag("fuse").GetComponent<Fusebox>();
        timerTxT.text = duration.ToString();
        fuseaudio = GameObject.FindGameObjectWithTag("fuse").GetComponent<AudioSource>();
    }
    private void Update()
    {

        if (!isCountingDown)

        {
            isCountingDown = true;
            timeRemaining = duration;
            Invoke("_tick", 1f);
        }
    }

    private void _tick()
    {
        timeRemaining--;
        //Debug.Log(timeRemaining);
        if (timeRemaining >= 0)
        {
            timerTxT.text = timeRemaining.ToString();
            Invoke("_tick", 1f);
        }
        else
        {
            isCountingDown = false;
            gameOver = true;
            win = true;
            fuse.PuzzlesCompleted++;
            fuseaudio.Play();
            puzzlehandeler.Turnoffgame();
        }
    }
}