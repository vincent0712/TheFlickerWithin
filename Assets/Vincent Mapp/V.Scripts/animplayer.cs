using UnityEngine;

public class animplayer : MonoBehaviour
{
    public Animation anim;



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            anim.Play("Walk");
        }
        
    }
}
