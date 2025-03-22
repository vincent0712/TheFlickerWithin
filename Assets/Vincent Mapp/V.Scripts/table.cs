using UnityEngine;

public class table : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Movement movement;
    public BoxCollider collider;
    void Start()
    {
        movement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collider.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collider.enabled = true;
        }
    }


}
