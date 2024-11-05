using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Transform targetPoint; 
    public float followSpeed = 5f; 
    public float swingAmount = 0.1f; 
    public float swingSpeed = 3f; 

    private Vector3 offset;

    void Start()
    {
        
        offset = Vector3.zero;
    }

    void Update()
    {
        
        offset.x = Mathf.Sin(Time.time * swingSpeed) * swingAmount;
        offset.y = Mathf.Cos(Time.time * swingSpeed * 0.5f) * swingAmount;

        
        Vector3 targetPosition = targetPoint.position + offset;

        
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);


        transform.LookAt(targetPosition);
    }
}
