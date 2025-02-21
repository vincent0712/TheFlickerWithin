
using TMPro;
using UnityEngine;

public class Keypad : MonoBehaviour
{
    public AudioClip clickSound;
    public AudioClip successSound;
    public AudioClip unsuccessSound;
    AudioSource audioSource;

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

        playerInput += number;
        if(playerInput.Length >= 4)
        {
            if(playerInput == code && CanGetPoint)
            {
                fuse.PuzzlesCompleted++;
                CanGetPoint = false;
                fuseaudio.Play();

                Debug.Log("Hehe");
                playerInput = "";
                audioSource.PlayOneShot(successSound);
            }
            else
            {
                Debug.Log("Nah");
                playerInput = "";
                audioSource.PlayOneShot(unsuccessSound);
            }
        }
    }
}
