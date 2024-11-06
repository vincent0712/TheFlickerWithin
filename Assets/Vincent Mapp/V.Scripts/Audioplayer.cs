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
        yield return new WaitForSeconds(1f);
        int random = Random.Range(1, 10);
        Debug.Log(random);
        if(random > 1)
        {
            
            Playsound(randoms1);
            StartCoroutine(Wait());
        }
        isoncd = false;
    }
    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(10f);
    }
    public void Playsound(AudioClip sound)
    {
        au.clip = sound;
        au.Play();
    }
}
