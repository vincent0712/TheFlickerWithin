using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class suicide : MonoBehaviour
{
    // Start is called before the first frame update
    public float timetilldeath = 10f;
    void Start()
    {
        Destroy(gameObject, timetilldeath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
