using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{

    [SerializeField] RoomGenerator roomGenerator;
    [SerializeField] Transform cameraTransform;
    private bool hasTeleported = false;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (hasTeleported) return;
            col.gameObject.transform.position = new Vector3(0, -7.5f, 0);
            cameraTransform.position = new Vector3(0, 0, cameraTransform.position.z);
            Debug.LogError("TELEPORT GENERATE ROOM");
            roomGenerator.GenerateRoom(false);
            hasTeleported = true;
        }
    }
}
