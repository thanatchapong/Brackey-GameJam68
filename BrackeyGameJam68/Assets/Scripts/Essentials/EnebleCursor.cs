using UnityEngine;

public class EnebleCursor : MonoBehaviour
{
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
