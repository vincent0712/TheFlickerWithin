using UnityEngine;

public class tutorial : MonoBehaviour
{

    public Animation anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public void playanim()
    {
        anim.Play("tutorialfadeout");
    }
}
