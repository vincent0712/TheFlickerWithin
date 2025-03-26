using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditButton : MonoBehaviour
{
    public void MoveToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
