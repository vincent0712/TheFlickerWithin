using UnityEngine;

public class Radio : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public bool isplaying = false;
    private AudioSource au;
    private bool canchange = true;
    void Start()
    {
        au = gameObject.GetComponent<AudioSource>();
    }



    public void Interact()
    {
        if (!canchange)
            return;

        isplaying = !isplaying;

        if (isplaying)
        {
            au.Play();
            StartCoroutine(Delay());
        }
        else if (!isplaying)
        {
            au.Stop();
        }
    }

    private System.Collections.IEnumerator Delay()
    {
        canchange = false;
        yield return new WaitForSeconds(1f);
        canchange = true;

    }
}
