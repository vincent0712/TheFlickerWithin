using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class lookAtPlayer : MonoBehaviour
{
    public Transform player;
    public Transform head;

    private float viewDistans = 10f;
    private float turnSpeed = 2f;

    void Start()
    {
    }

    
    void Update()
    {
        if(player != null)
        {
            // Find the distans to player
            float distansToPlayer = Vector3.Distance(head.position,player.position);

            if(distansToPlayer <= viewDistans)
            {
                // Find direction of player
                Vector3 direction = player.position - head.position;

                // Find player rotaion
                Quaternion playerRotation = Quaternion.LookRotation(direction);

                // Turn to face player
                head.rotation = Quaternion.Lerp(head.rotation, playerRotation, turnSpeed * Time.deltaTime);
            }
        }
    }
}
