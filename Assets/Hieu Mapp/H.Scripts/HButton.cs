using UnityEngine;
using UnityEngine.Events;

public class HButton : MonoBehaviour, IInteractable
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
