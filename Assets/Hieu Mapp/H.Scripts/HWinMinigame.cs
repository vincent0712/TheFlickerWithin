using UnityEngine;

public class HWinMinigame : MonoBehaviour
{
    //private Fusebox fuse;
    private bool CanGetPoint = true;
    private AudioSource fuseaudio;
    private Fusebox fuse;
    public ClockPuzzle door;

    private void Start()
    {
        
        fuse = GameObject.FindGameObjectWithTag("fuse").GetComponent<Fusebox>();
        fuseaudio = GameObject.FindGameObjectWithTag("fuse").GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "RealWinHole" && CanGetPoint)
        {
            
            Debug.Log("You win");
            fuse.PuzzlesCompleted++;
            fuseaudio.Play();
            CanGetPoint = false;
            door.Endgame();
            door.canopen = false;
        }
    }
}
