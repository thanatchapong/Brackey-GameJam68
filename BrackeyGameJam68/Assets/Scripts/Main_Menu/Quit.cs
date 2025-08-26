using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Quit Button Pressed!"); // เช็คว่ากดปุ่มแล้ว
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // ออกจาก Play Mode
#else
        Application.Quit(); // ปิดเกมจริงตอน Build
#endif
    }
}