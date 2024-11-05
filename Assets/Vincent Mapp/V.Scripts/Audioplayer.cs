using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audioplayer : MonoBehaviour
{
    AudioSource au;

    void Start()
    {
        au = GetComponent<AudioSource>();
    }

    public void Playsound(AudioClip sound)
    {
        au.clip = sound;
        au.Play();
    }
}
