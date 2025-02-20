using UnityEngine;
using UnityEngine.Events;

public class HButton : MonoBehaviour, MInteractable
{
    public int keyNumber;

    public UnityEvent KeypadClicked;


    private void Update()
    {

    }

    public void Interact()
    {
        KeypadClicked.Invoke();
    }
}
