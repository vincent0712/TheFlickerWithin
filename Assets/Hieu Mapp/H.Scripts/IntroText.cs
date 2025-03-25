using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroText : MonoBehaviour
{
    private Onscreentext onScreenText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        onScreenText = FindObjectOfType<Onscreentext>();

        StartCoroutine(startText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator startText()
    {
        onScreenText.ShowText("It was a stormy, rain-drenched night", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("The wind howled through the trees, rain lashed against the earth,", 4f);
        yield return new WaitForSeconds(5);
        onScreenText.ShowText("and thunder cracked through the darkened forest like a beast roaring in the distance", 4f);
        yield return new WaitForSeconds(5);
        onScreenText.ShowText("We stumbled through the trench, our boots sinking into pockets of wet mud and water,", 4f);
        yield return new WaitForSeconds(5);
        onScreenText.ShowText("tangled in the ancient, gnarled roots of towering pine trees", 4f);
        yield return new WaitForSeconds(5);
        onScreenText.ShowText("The cold bit at our skin as we tripped over ourselves, scratching our arms and legs on jagged branches", 5f);
        yield return new WaitForSeconds(6);
        onScreenText.ShowText("Then, in the distance, something emerged from the darkness—a structure,", 4f);
        yield return new WaitForSeconds(5);
        onScreenText.ShowText("barely visible through the relentless downpour", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("Hope surged through us", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("Shelter", 2f);
        yield return new WaitForSeconds(3);
        onScreenText.ShowText("Protection", 2f);
        yield return new WaitForSeconds(3);
        onScreenText.ShowText("Maybe even salvation", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("With the last reserves of strength left in our bodies, we pushed forward", 4f);
        yield return new WaitForSeconds(5);
        onScreenText.ShowText(" As we neared, the dim outline sharpened into an old wooden cabin,", 4f);
        yield return new WaitForSeconds(5);
        onScreenText.ShowText("its frame warped with age, abandoned for years—or so it seemed", 4f);
        yield return new WaitForSeconds(3);
        onScreenText.ShowText("Desperation took hold", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("We lunged at the heavy wooden door, pounding against it, our voices hoarse from screaming for help", 5f);
        yield return new WaitForSeconds(6);
        onScreenText.ShowText("Then movement", 2f);
        yield return new WaitForSeconds(3);
        onScreenText.ShowText("A flicker of light stirred in the window above", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("A click", 2f);
        yield return new WaitForSeconds(3);
        onScreenText.ShowText("The door unlocked", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("I stumbled inside, drenched and trembling,", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("slamming the door shut behind me as if to trap the storm outside", 4f);
        yield return new WaitForSeconds(5);
        onScreenText.ShowText("But as I stood there, catching my breath, an unsettling realization crept over me", 4f);
        yield return new WaitForSeconds(5);
        onScreenText.ShowText("The cabin was dark", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("Too dark", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("I pulled out my flashlight,", 3f);
        yield return new WaitForSeconds(4);
        onScreenText.ShowText("its flickering, water-damaged beam barely cutting through the suffocating blackness", 4f);
        yield return new WaitForSeconds(5);
        onScreenText.ShowText("My heart pounded", 3f);
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


        SceneManager.LoadScene("Main");
    }
}
