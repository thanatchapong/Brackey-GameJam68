using UnityEngine;
using UnityEngine.SceneManagement;
public class BacktoMenu : MonoBehaviour
{
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
