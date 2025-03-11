
using TMPro;
using UnityEngine;

public class Keypad : MonoBehaviour
{
    public AudioClip clickSound;
    public AudioClip successSound;
    public AudioClip unsuccessSound;
    public AudioClip music;
    AudioSource audioSource;

    public TextMeshPro playerAnswer;
    public TextMeshPro tmpText;
    public float password;

    private string code;
    private string playerInput;
    private Fusebox fuse;
    private bool CanGetPoint = true;

    private AudioSource fuseaudio;


    private void Start()
    {
        fuse = GameObject.FindGameObjectWithTag("fuse").GetComponent<Fusebox>();
        fuseaudio = GameObject.FindGameObjectWithTag("fuse").GetComponent<AudioSource>();

        playerInput = "";

        password = (int)UnityEngine.Random.Range(1000f, 9999f);
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
                fuse.PuzzlesCompleted++;
                CanGetPoint = false;
                fuseaudio.Play();

                Debug.Log("Hehe");
                playerInput = "Success";
                audioSource.PlayOneShot(successSound);
                playerAnswer.text = playerInput;
                playerInput = "";
            }
            else
            {
                Debug.Log("Nah");
                playerInput = "Error";
                audioSource.PlayOneShot(unsuccessSound);
                playerAnswer.text = playerInput;
                playerInput = "";
            }
        }

        if (number == "11")
        {
            audioSource.PlayOneShot(music);
        }
    }
}
