using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.transform.position = new Vector3(0, 0, 0);
        }
    }
}
