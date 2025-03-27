using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroText : MonoBehaviour
{
    private Onscreentext onScreenText;

    public AudioClip rain;
    public AudioClip doorUnlocked;
    public AudioClip doorClose;
    public AudioClip flashlight;
    public AudioClip pound;
    AudioSource audioSource;

    private int timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        onScreenText = FindObjectOfType<Onscreentext>();
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(startText());
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("Main");
        }
    }
    public IEnumerator startText()
    {
        audioSource.PlayOneShot(rain);
        onScreenText.ShowText("A storm raged as I stumbled through mud and roots, cold biting my skin", 4f);
        yield return new WaitForSeconds(5);
        onScreenText.ShowText("Then—through the downpour", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("I see", 2f);
        yield return new WaitForSeconds(3);
        onScreenText.ShowText("A cabin", 2f);
        yield return new WaitForSeconds(3);
        onScreenText.ShowText("A hope", 2f);
        yield return new WaitForSeconds(3);
        onScreenText.ShowText("A shelter", 2f);
        yield return new WaitForSeconds(3);
        audioSource.PlayOneShot(pound);
        onScreenText.ShowText("I pounded on the door", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("A movement", 2f);
        yield return new WaitForSeconds(3);
        onScreenText.ShowText("A flicker of light", 3f);
        yield return new WaitForSeconds(4);
        audioSource.PlayOneShot(doorUnlocked);
        onScreenText.ShowText("A click", 2f);
        yield return new WaitForSeconds(3);
        onScreenText.ShowText("I stumbled inside, drenched and trembling,", 3f);
        audioSource.Stop();
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("slamming the door shut behind me as if to trap the storm outside", 4f);
        audioSource.PlayOneShot(doorClose);
        yield return new WaitForSeconds(3);
        onScreenText.ShowText("Inside, darkness swallowed me", 3f);
        yield return new WaitForSeconds(4);
        audioSource.PlayOneShot(flashlight);
        onScreenText.ShowText("My flashlight flickered", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("Dread seized me", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("I was not alone", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("I needed to turn on the light", 3f);
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("Main");
    }
}
