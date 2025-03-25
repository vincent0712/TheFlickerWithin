using UnityEngine;

public class monsterfootsteps : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private AudioSource au;
    public AudioClip[] stompsounds;
    void Start()
    {
        au = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void playfootstep()
    {
        if (stompsounds.Length > 0 && au)
        {
            au.pitch = Random.Range(0.9f, 1.1f);

            au.PlayOneShot(stompsounds[Random.Range(0, stompsounds.Length)]);


        }
    }
}
