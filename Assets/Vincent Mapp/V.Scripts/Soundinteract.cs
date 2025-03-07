using UnityEngine;

public class Soundinteract : MonoBehaviour, MInteractable
{
    private AudioSource au;
    public bool randompitch = true;

    void Start()
    {
        au = gameObject.GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (randompitch)
        {
            au.pitch = Random.Range(0.85f, 1.15f);
            au.Play();
        }
        else if (!randompitch)
        {

            au.Play();
        }
    }
}
