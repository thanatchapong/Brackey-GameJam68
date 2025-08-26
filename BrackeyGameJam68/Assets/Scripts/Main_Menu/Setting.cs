using UnityEngine;
using UnityEngine.SceneManagement;

public class Setting : MonoBehaviour
{
    public void OpenSetting()
    {
        Debug.Log("Setting Button Clicked!");
        SceneManager.LoadScene("Pun_Setting");
    }
}

