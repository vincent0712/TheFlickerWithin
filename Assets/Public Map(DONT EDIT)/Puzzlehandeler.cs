using UnityEngine;

public class Puzzlehandeler : MonoBehaviour, MInteractable
{
    public GameObject game;

    public bool isgameon = false;

    public Camera playercamera;
    public Camera gamecamera;

    public Movement movememt;

    public Timeer timer;

    // Update is called once per frame


    public void Interact()
    {

        if (!isgameon && timer.win == false)
        {
            movememt.canmove = false;
            playercamera.gameObject.SetActive(false);
            game.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isgameon = true;
        }
        else if (isgameon)
        {
            movememt.canmove = true;
            playercamera.gameObject.SetActive(true);
            game.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isgameon = false;
        }


    }

    public void Turnoffgame()
    {
        movememt.canmove = true;
        playercamera.gameObject.SetActive(true);
        game.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        isgameon = false;
    }
}
