using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mdoor : MonoBehaviour, MInteractable
{
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public bool isOpen = false;
    private AudioSource au;
    public bool islocked = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isAnimating = false;

    void Start()
    {

        au = gameObject.GetComponent<AudioSource>();
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, openAngle, 0f));
    }

    public void Interact()
    {
        if (!isAnimating)
        {

            isOpen = !isOpen;
            au.Play();
            StartCoroutine(RotateDoor());
        }
    }

    private System.Collections.IEnumerator RotateDoor()
    {
        isAnimating = true;

        Quaternion targetRotation = isOpen ? openRotation : closedRotation;


        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }

        transform.rotation = targetRotation;
        isAnimating = false;
    }

}
