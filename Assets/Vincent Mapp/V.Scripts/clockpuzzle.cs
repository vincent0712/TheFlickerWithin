using UnityEngine;

public class ClockPuzzle : MonoBehaviour, MInteractable
{
    public GameObject interiar;
    public bool isopen = false;
    public bool canopen = true;
    public Animation anim;
    private AudioSource au;
    public GameObject text;


    private void Start()
    {
        interiar.SetActive(false);
        anim.Play("clockclose");
        au = gameObject.GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (!canopen)
            return;
        if (!isopen && !anim.isPlaying)
        {
            anim.Play("clockopen");
            au.pitch = Random.Range(0.85f, 1.15f);
            au.Play();
            interiar.SetActive(true);
        }
        else if (isopen && !anim.isPlaying)
        {
            anim.Play("clockclose");
            au.pitch = Random.Range(0.85f, 1.15f);
            au.Play();
            StartCoroutine(WaitForAnimation("clockclose"));
        }

        isopen = !isopen;
    }

    public void Endgame()
    {
        anim.Play("clockclose");
        au.pitch = Random.Range(0.85f, 1.15f);
        au.Play();
        StartCoroutine(WaitForAnimation("clockclose"));
        Destroy(GetComponent<Outline>());
        text.SetActive(false);
    }
    private System.Collections.IEnumerator WaitForAnimation(string animationName)
    {
        while (anim.isPlaying)
        {
            yield return null;
        }
        interiar.SetActive(false);
    }
}