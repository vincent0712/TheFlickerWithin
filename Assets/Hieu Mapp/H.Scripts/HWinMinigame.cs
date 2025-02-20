using UnityEngine;

public class HWinMinigame : MonoBehaviour
{
    private Fusebox fuse;
    private bool CanGetPoint = true;

    private void Start()
    {
        fuse = GameObject.FindGameObjectWithTag("fuse").GetComponent<Fusebox>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "RealWinHole" && CanGetPoint)
        {
            Debug.Log("You win");
        }
    }
}
