using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{

    [SerializeField] RoomGenerator roomGenerator;
    [SerializeField] Transform cameraTransform;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.transform.position = new Vector3(0, 0, 0);
            cameraTransform.position = new Vector3(0, 0, cameraTransform.position.z);
            Debug.LogError("TELEPORT GENERATE ROOM");
            roomGenerator.GenerateRoom(false);
        }
    }
}
