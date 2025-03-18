using UnityEngine;
using UnityEngine.SceneManagement;

public class HPlayerDead : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "monster")
        {
            SceneManager.LoadScene("StartMenu");
        }
    }
}
