using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;

public class recordplayer : MonoBehaviour, MInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public bool isplaying = false;
    private Animation anim;
    private AudioSource au;
    private bool canchange = true;
    private bool first = true;
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        au = gameObject.GetComponent<AudioSource>();
    }



    public void Interact()
    {
        if (!canchange)
            return;

        if (first)
        {
            anim.Play("recordpla");
            first = false;
        }

        isplaying = !isplaying;

        if (isplaying)
        {

            anim["recordpla"].speed = 1f;
            au.Play();
            StartCoroutine(Delay());
        }
        else if (!isplaying)
        {
            anim["recordpla"].speed = 0f;
            au.Stop();
        }
    }

    private IEnumerator Delay()
    {
        canchange = false;
        yield return new WaitForSeconds(1f);
        canchange = true;

    }
}
