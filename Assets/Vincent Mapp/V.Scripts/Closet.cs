using UnityEngine;

public class Closet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animation anim;
    private bool isopen = false;
    public AudioSource au;
    public BoxCollider col;
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        //au = gameObject.GetComponent<AudioSource>();
    }

    public void openclose()
    {
        if (!isopen)
        {
            anim.Play("opencloset");
            
            //col.enabled = false;
            col.center = new Vector3(-1.93f, -3f, -5.6f);

        }
        else if (isopen)
        {
            anim.Play("closecloset");
            //col.enabled = true;
            col.center = new Vector3(-1.93f, 1.36f, -5.6f);
        }
        au.pitch = Random.Range(0.85f, 1.15f);
        au.Play();
        isopen = !isopen;
    }
}
