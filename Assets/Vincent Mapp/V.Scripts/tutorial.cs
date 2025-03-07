using UnityEngine;

public class tutorial : MonoBehaviour
{

    public Animation anim;
    private bool canplay = true;
    private Movement move;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canplay)
            playanim();
    }
    public void playanim()
    {
        anim.Play("tutorialfadeout");
        move = GameObject.FindAnyObjectByType<Movement>();
        move.canmove = true;
        canplay = false;

    }
}
