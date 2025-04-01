
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Keypad : MonoBehaviour
{
    public AudioClip clickSound;
    public AudioClip successSound;
    public AudioClip unsuccessSound;
    public AudioClip music;
    AudioSource audioSource;
    public Transform[] pointsspawn;

    private MonsterAI monster;
    public TextMeshPro playerAnswer;
    public TextMeshPro tmpText;
    public float password;
    public bool canplay = true;

    private string code;
    private string playerInput;
    private Fusebox fuse;
    private bool CanGetPoint = true;

    private AudioSource fuseaudio;


    private void Start()
    {
        fuse = GameObject.FindGameObjectWithTag("fuse").GetComponent<Fusebox>();
        fuseaudio = GameObject.FindGameObjectWithTag("fuse").GetComponent<AudioSource>();
        monster = GameObject.FindGameObjectWithTag("monster").GetComponent<MonsterAI>();

        Transform chosenPoint = pointsspawn[Random.Range(0, pointsspawn.Length)];
        tmpText.transform.position = chosenPoint.transform.position;

        playerInput = "";

        while(true)
        {
            password = (int)UnityEngine.Random.Range(1000, 9999);

            if (password == 6969)
            {
                Debug.Log("Hihihih");
                password = (int)UnityEngine.Random.Range(1000, 9999);
                continue;
            }
            else
                break;
        }
        tmpText.text = password.ToString();
        code = password.ToString();

        audioSource = GetComponent<AudioSource>();
    }

    public void ButtonClicked(string number)
    {
        audioSource.PlayOneShot(clickSound);

        if (number != "10" && number != "11" && playerInput.Length < 4)
        {
            playerInput += number;
            playerAnswer.text = playerInput;
        }    

        if(number == "10")
        {
            if(playerInput == code && CanGetPoint)
            {
                if (playerInput == "6969")
                {
                    Debug.Log("RR");
                    audioSource.PlayOneShot(music);
                    monster.HearSound(gameObject.transform.position,1f);
                }

                fuse.PuzzlesCompleted++;
                CanGetPoint = false;
                fuseaudio.Play();
                monster.HearSound(gameObject.transform.position,1f);

                Debug.Log("Hehe");
                playerInput = "Success";
                audioSource.PlayOneShot(successSound);
                playerAnswer.text = playerInput;
                playerInput = "";
            }

            else
            {
                if (playerInput == "6969")
                {
                    Debug.Log("RR");
                    audioSource.PlayOneShot(music);
                }

                Debug.Log("Nah");
                playerInput = "Error";
                audioSource.PlayOneShot(unsuccessSound);
                playerAnswer.text = playerInput;
                playerInput = "";
            }
        }

        if (number == "11" && canplay)
        {

            audioSource.PlayOneShot(music);
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {

        canplay = false;
        yield return new WaitForSeconds(9f);
        canplay = true;

    }
}
