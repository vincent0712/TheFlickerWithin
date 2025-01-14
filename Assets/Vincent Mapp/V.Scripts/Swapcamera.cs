using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swapcamera : MonoBehaviour
{
    public Camera cam;
    public Camera cam2;
    bool iscam = false;
    private MeshRenderer monster;
    void Start()
    {
        
        cam.gameObject.SetActive(true);
        cam2.gameObject.SetActive(false);

        monster = GameObject.FindGameObjectWithTag("monster").GetComponent<MeshRenderer>();
        monster.GetComponent<MeshRenderer>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!iscam)
            {
                cam.gameObject.SetActive(false);
                cam2.gameObject.SetActive(true);
                monster.GetComponent<MeshRenderer>().enabled = true;
                iscam = true;
            }
            else if (iscam)
            {
                cam2.gameObject.SetActive(false);
                cam.gameObject.SetActive(true);
                monster.GetComponent<MeshRenderer>().enabled = false;
                iscam = false;
            }

        }

    }
}
