using UnityEngine;

public class HWinMinigame : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "RealWinHole")
        {
            Debug.Log("You win");
        }
    }
}
