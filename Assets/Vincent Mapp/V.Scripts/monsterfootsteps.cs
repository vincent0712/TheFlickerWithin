using EZCameraShake;
using UnityEngine;

public class monsterfootsteps : MonoBehaviour
{
    private AudioSource au;
    public AudioClip[] stompsounds;

    public Transform player;  // The player’s transform (to calculate the distance)
    public Transform monster; // The monster’s transform (or this GameObject itself if the script is on the monster)

    public float maxShakeMagnitude = 2f; // Maximum shake intensity
    public float maxDistance = 20f;      // Max distance for scaling the shake
    public float minDistance = 5f;       // Minimum distance for maximum shake intensity

    void Start()
    {
        au = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void playfootstep()
    {
        if (stompsounds.Length > 0 && au)
        {
            // Calculate the distance between the monster and the player
            float distance = Vector3.Distance(player.position, monster.position);

            // Calculate shake magnitude based on distance
            float shakeMagnitude = Mathf.Clamp01(1 - (distance - minDistance) / (maxDistance - minDistance)) * maxShakeMagnitude;

            // Play the stomp sound with a random pitch
            au.pitch = Random.Range(0.9f, 1.1f);
            au.PlayOneShot(stompsounds[Random.Range(0, stompsounds.Length)]);

            // Apply the camera shake with the calculated intensity
            CameraShaker.Instance.ShakeOnce(shakeMagnitude, 3f, 0, 2f);
        }
    }
}
