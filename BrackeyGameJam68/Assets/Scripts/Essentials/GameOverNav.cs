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

            AudioManager.instance.ResetBGM();
            SceneManager.LoadScene(1);
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = 1;
            AudioManager.instance.StopBGM();
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
}
