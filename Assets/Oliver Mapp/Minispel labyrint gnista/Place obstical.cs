using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Placeobstical : MonoBehaviour
{
    public GameObject cut;
    public Camera mainCamera;
    public Sprite[] cuters;

    public string targetTag = "cut";
    public GameObject cuters1;
    public GameObject cuters2;

    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        cuters1.SetActive(false);
        CheckAndHandleObjects();
        if (Input.GetMouseButtonDown(0)) 
        {
                placeObstical();
        }
    }
    void placeObstical()
    {
        // Kolla hur många objekt med taggen "cut" som finns
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("cut");
        if (obstacles.Length >= 2)
        {
            
            //Debug.Log("Max 2 objekt är redan placerade!");
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //Debug.Log("Place obstical funktion activ");

        if (Physics.Raycast(ray, out hit))
        {
            Instantiate(cut, hit.point, cut.GetComponent<Transform>().rotation);
            //Debug.Log("Placerar cut");
            //Debug.Log("Placerade objektet. Nuvarande antal: " + (obstacles.Length + 1));
        }
        GameObject[] objects = GameObject.FindGameObjectsWithTag("cut");
        int count = objects.Length;

        if (count == 2)
        {
            cuters2.SetActive(false);
            cuters1.SetActive(false);
            //Debug.Log("Hej1");

        }
        else if (count == 1)
        {
            cuters2.SetActive(true);
            cuters1.SetActive(false);
            //Debug.Log("Hej2");
        }
    }

    void CheckAndHandleObjects()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("cut");
        int count = objects.Length;

        if(count == 0)
        {
            cuters1.SetActive(true);
            cuters2.SetActive(true);
            //Debug.Log("Reset cuters");
        }
        else if (count == 1)
        {
            cuters2.SetActive(true);
            cuters1.SetActive(false);
            //Debug.Log("Hej2");
        }
    }
}
