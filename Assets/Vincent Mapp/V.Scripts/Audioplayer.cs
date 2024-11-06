using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audioplayer : MonoBehaviour
{
    AudioSource au;
    bool isoncd = false;
    public AudioClip randoms1;

    void Start()
    {
        au = GetComponent<AudioSource>();
        
    }

    private void Update()
    {
        if(!isoncd)
            StartCoroutine(randomsound());
    }
    public IEnumerator randomsound()
    {
        isoncd = true;
        yield return new WaitForSeconds(15f);
        int random = Random.Range(1, 10);
        Debug.Log(random);
        if(random > 7)
        {
            Playsound(randoms1);
        }
        isoncd = false;
    }

    public void Playsound(AudioClip sound)
    {
        au.clip = sound;
        au.Play();
    }
}
