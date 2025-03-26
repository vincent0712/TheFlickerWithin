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

    // Update is called once per frame
    void Update()
    {

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
        onScreenText.ShowText("I pounded on the door", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("A movement", 2f);
        yield return new WaitForSeconds(3);
        onScreenText.ShowText("A flicker of light", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("A click", 2f);
        audioSource.PlayOneShot(doorUnlocked);
        onScreenText.ShowText("I stumbled inside, drenched and trembling,", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("slamming the door shut behind me as if to trap the storm outside", 4f);
        audioSource.Stop();
        audioSource.PlayOneShot(doorClose);
        yield return new WaitForSeconds(3);
        onScreenText.ShowText("Inside, darkness swallowed me", 3f);
        yield return new WaitForSeconds(4);
        audioSource.PlayOneShot(flashlight);
        onScreenText.ShowText("My flashlight flickered", 3f);
        yield return new WaitForSeconds(4);
        audioSource.PlayOneShot(heartBeat);
        onScreenText.ShowText("Dread seized me", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("I was not alone", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("I needed to turn on the light", 3f);
        yield return new WaitForSeconds(4);
        
        
        /*onScreenText.ShowText("Desperation took hold", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("I lunged at the heavy wooden door, pounding against it, my voices hoarse from screaming for help", 5f);
        yield return new WaitForSeconds(6);
        onScreenText.ShowText("Then movement", 2f);
        yield return new WaitForSeconds(3);
        onScreenText.ShowText("A flicker of light stirred in the window above", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("A click", 2f);
        audioSource.PlayOneShot(doorUnlocked);
        yield return new WaitForSeconds(3);
        onScreenText.ShowText("The door unlocked", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("I stumbled inside, drenched and trembling,", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("slamming the door shut behind me as if to trap the storm outside", 4f);
        audioSource.Stop();
        audioSource.PlayOneShot(doorClose);
        yield return new WaitForSeconds(5);
        onScreenText.ShowText("But as I stood there, catching my breath, an unsettling realization crept over me", 4f);
        yield return new WaitForSeconds(5);
        onScreenText.ShowText("The cabin was dark", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("Too dark", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("I pulled out my flashlight,", 3f);
        yield return new WaitForSeconds(4);
        audioSource.PlayOneShot(flashlight);
        onScreenText.ShowText("its flickering, water-damaged beam barely cutting through the suffocating blackness", 4f);
        yield return new WaitForSeconds(5);
        onScreenText.ShowText("My heart pounded", 3f);
        audioSource.PlayOneShot(heartBeat);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("The hair on my soaked skin rose", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("My throat tightened as an icy sensation curled in my chest", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("And then—pure, unshakable dread", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("I WAS NOT ALONE", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("The storm outside had driven me here, but this place—this supposed refuge—might be far worse", 5f);
        yield return new WaitForSeconds(6);
        onScreenText.ShowText("One thought seized my mind, loud and urgent above all else", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("I NEED TO TURN ON THE LIGHT.", 3f);
        yield return new WaitForSeconds(4);
        */
        SceneManager.LoadScene("Main");
    }
}
