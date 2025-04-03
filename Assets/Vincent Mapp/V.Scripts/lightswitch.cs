using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class lightswitch : MonoBehaviour, MInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Animation anim;
    private bool ison = false;
    //public string line;

    private Onscreentext onScreenText; // Reference to OnScreenText script

    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        onScreenText = FindObjectOfType<Onscreentext>();
    }

    public void Interact()
    {
        if(!ison && !anim.isPlaying)
        {
            anim.Play("flickon");
            onScreenText.ShowText("How Could I Get These To Work", 2f);

            ison = true;
        }
        else if(ison && !anim.isPlaying)
        {
            anim.Play("flickoff");
            ison = false;
        }
    }
}
