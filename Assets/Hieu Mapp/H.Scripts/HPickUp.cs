using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPickup : MonoBehaviour
{
    bool isHolding = false;
    [SerializeField]
    float throwForce = 0f;
    [SerializeField]
    float maxDistance = 3f;
    float distance;

    HTempParent hTempParent;
    Rigidbody rb;

    Vector3 objectPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hTempParent = HTempParent.Instance;

    }

    // Update is called once per frame
    void Update()
    {

    }
}