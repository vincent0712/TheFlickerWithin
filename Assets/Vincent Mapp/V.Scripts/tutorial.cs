using UnityEngine;

public class tutorial : MonoBehaviour
{

    public Animation anim;
    private bool canplay = true;
    private Movement move;
    private bool isshowing = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !anim.isPlaying && isshowing)
        {
            fadeout();

        }

        if (Input.GetKeyDown(KeyCode.T) && !anim.isPlaying)
        {
            fadein();
        }
    }
    public void fadeout()
    {
        anim.Play("tutorialfadeout");
        isshowing = false;
        move = GameObject.FindAnyObjectByType<Movement>();
        move.canmove = true;
        isshowing = false;
        

    }
    public void fadein()
    {
        anim.Play("tutorialfadein");
        move = GameObject.FindAnyObjectByType<Movement>();
        move.canmove = false;
        isshowing = true;
        

    }
}
