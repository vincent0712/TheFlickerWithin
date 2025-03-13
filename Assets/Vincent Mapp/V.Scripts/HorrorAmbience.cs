using UnityEngine;
using System.Collections;

public class HorrorAmbience : MonoBehaviour
{
    [Header("Sound Settings")]
    public AudioClip[] scarySounds; // Array of scary sound clips
    public int soundSourceCount = 4; // Number of sound sources
    public float minDelay = 5f; // Minimum time between sounds
    public float maxDelay = 15f; // Maximum time between sounds
    public bool randomizeVolume = true; // Should volume vary per sound?
    public Vector2 volumeRange = new Vector2(0.5f, 1f); // Min and max volume

    [Header("Position Settings")]
    public float minSoundDistance = 2f; // Minimum distance from the player
    public float maxSoundDistance = 5f; // Maximum distance from the player
    public bool lockRotation = true; // Should the sources follow the player but not rotate?

    private Transform[] soundPositions;
    private AudioSource audioSource;

    void Start()
    {
        // Create dynamic sound sources
        soundPositions = new Transform[soundSourceCount];
        for (int i = 0; i < soundSourceCount; i++)
        {
            GameObject soundPoint = new GameObject("SoundPoint" + i);
            soundPoint.transform.parent = transform;
            soundPoint.transform.localPosition = GetRandomOffset();
            soundPositions[i] = soundPoint.transform;
        }

        // Add AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f; // Ensure 3D sound
        audioSource.playOnAwake = false;

        // Start playing random sounds
        StartCoroutine(PlayRandomScarySounds());
    }

    void Update()
    {
        // Update sound positions to follow the player
        foreach (Transform soundPoint in soundPositions)
        {
            soundPoint.position = transform.position + soundPoint.localPosition;

            if (lockRotation)
            {
                soundPoint.rotation = Quaternion.identity; // Prevent rotation
            }
        }
    }

    private IEnumerator PlayRandomScarySounds()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

            // Select a random sound position and sound clip
            Transform randomPosition = soundPositions[Random.Range(0, soundPositions.Length)];
            AudioClip randomClip = scarySounds[Random.Range(0, scarySounds.Length)];

            // Play the sound at the chosen position
            PlaySoundAt(randomPosition, randomClip);
        }
    }

    private void PlaySoundAt(Transform position, AudioClip clip)
    {
        GameObject soundObject = new GameObject("TempSound");
        soundObject.transform.position = position.position; // Keeps directional sound intact
        AudioSource tempAudio = soundObject.AddComponent<AudioSource>();

        tempAudio.spatialBlend = 1f; // Ensure 3D sound
        tempAudio.clip = clip;
        tempAudio.volume = randomizeVolume ? Random.Range(volumeRange.x, volumeRange.y) : 1f;

        // Set linear rolloff for sound falloff
        tempAudio.rolloffMode = AudioRolloffMode.Linear;
        tempAudio.minDistance = 10f; // Adjust to fit your game needs
        tempAudio.maxDistance = 20f; // Set the max distance before sound fully fades

        tempAudio.Play();
        Destroy(soundObject, clip.length + 0.5f); // Destroy after playback
    }

    private Vector3 GetRandomOffset()
    {
        // Define random positions around the player
        float distance = Random.Range(minSoundDistance, maxSoundDistance);
        float angle = Random.Range(0f, 360f);
        return new Vector3(
            Mathf.Cos(angle) * distance,
            0,
            Mathf.Sin(angle) * distance
        );
    }
}
