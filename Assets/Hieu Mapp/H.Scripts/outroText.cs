using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class outroText : MonoBehaviour
{
    private Onscreentext onScreenText;

    public AudioClip rain;
    public AudioClip doorUnlocked;
    public AudioClip doorClose;
    public AudioClip flashlight;
    public AudioClip heartBeat;
    AudioSource audioSource;

    private int timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        onScreenText = FindObjectOfType<Onscreentext>();
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(startText());
    }

    public IEnumerator startText()
    {
        audioSource.PlayOneShot(rain);
        onScreenText.ShowText("\"And God said, Let there be light: and there was light.\"", 4f);
        yield return new WaitForSeconds(5);
        onScreenText.ShowText("Genesis 1:3 \r\n", 2f);
        yield return new WaitForSeconds(3);
        onScreenText.ShowText("With those words,", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("darkness and evil were cast away,", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("and in the cabin,", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("the light flickered to life once more.", 3f);
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("Main");
    }
}
