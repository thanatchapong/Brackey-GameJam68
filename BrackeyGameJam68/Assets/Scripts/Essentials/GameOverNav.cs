using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverNav : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
    }
}
