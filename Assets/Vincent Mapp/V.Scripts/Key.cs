using UnityEngine;

public class Key : MonoBehaviour, MInteractable
{
    public bool haskey = false;
    public GameObject audioPrefab; // Assign a prefab with an AudioSource

    public void Interact()
    {
        haskey = true;
        DestroyWithSound();

        gameObject.SetActive(false);
    }

    void DestroyWithSound()
    {
        Instantiate(audioPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
