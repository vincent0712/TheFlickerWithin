using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermoveing : MonoBehaviour
{
    public int movespeed;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
