using UnityEngine;

public class HWinMinigame : MonoBehaviour
{
    private Fusebox fuse;
    private bool CanGetPoint = true;
    private AudioSource fuseaudio;

    private void Start()
    {
        fuse = GameObject.FindGameObjectWithTag("fuse").GetComponent<Fusebox>();
        fuseaudio = GameObject.FindGameObjectWithTag("fuse").GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "RealWinHole" && CanGetPoint)
        {
            //Debug.Log("You win");
            fuse.PuzzlesCompleted++;
            fuseaudio.Play();
            CanGetPoint = false;
        }
    }
}
