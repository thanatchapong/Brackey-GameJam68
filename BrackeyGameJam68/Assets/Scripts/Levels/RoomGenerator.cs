using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomGenerator : MonoBehaviour
{

    public List<GameObject> templateList;
    public List<GameObject> obstacleList;
    public GameObject doorPrefab;
    private GameObject entranceDoor, targetDoor;
    private Door entranceDoorScript, targetDoorScript;
    public Transform player;
    private GameObject currentRoom = null;
    public GameObject level;
    private int roomNumber = 0;

    [Header("Door Clearance Settings")]
    [SerializeField] private float doorClearanceRadius = 1.25f;

    [Header("UI")]
    public TextMeshPro roomNumberText;

    void Start()
    {
        GenerateRoom();
    }

    public void GenerateRoom()
    {
        UpdateRoomNumberUI();

        if(currentRoom) Destroy(currentRoom);

        if(templateList == null || templateList.Count == 0) {
            Debug.LogError("Template list is empty or not assigned in RoomGenerator!");
            return;
        }

        int templateIdx = Random.Range(0, templateList.Count);
        GameObject selectedTemplate = templateList[templateIdx];
        currentRoom = Instantiate(selectedTemplate, Vector3.zero, Quaternion.identity);

        Transform[] allChildren = currentRoom.GetComponentsInChildren<Transform>();
        
        foreach(Transform spawnpoint in allChildren) {
            if(spawnpoint.gameObject.CompareTag("ObstacleSpawnpoint")) {
                if(Random.Range(0, 2) >= 1) {
                if(obstacleList == null || obstacleList.Count == 0) {
                    Debug.LogError("Obstacle list is empty or not assigned in RoomGenerator!");
                    return;
                }

                int obstacleIdx = Random.Range(0, obstacleList.Count);
                GameObject selectedObstacle = obstacleList[obstacleIdx];
                float randomZ = Random.Range(-180f, 180f);
                Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomZ);
                Instantiate(selectedObstacle, spawnpoint.position, randomRotation, currentRoom.transform);
            }
            }
        }
        GenerateDoor();

        if (entranceDoor != null) ClearObstacles(entranceDoor.transform.position);
        if (targetDoor != null) ClearObstacles(targetDoor.transform.position);
    }

    private void ClearObstacles(Vector3 doorPos) {
        Collider2D[] hits = Physics2D.OverlapBoxAll(doorPos, new Vector2(doorClearanceRadius, doorClearanceRadius), 0f);
        foreach (Collider2D hit in hits)
        {
            if (hit == null) continue;
            if (hit.CompareTag("Obstacle"))
            {
                Destroy(hit.gameObject);
            }
        }
    }

    private void GenerateDoor() {

        if (level == null) {
            Debug.LogError("Level GameObject is not assigned in RoomGenerator!");
            return;
        }

        Transform groundT = level.transform.Find("Ground");
        if (groundT == null) {
            Debug.LogError("No 'Ground' child found in level GameObject: " + level.name);
            return;
        }

        if(doorPrefab == null) {
            Debug.LogError("Door prefab is not assigned in RoomGenerator!");
            return;
        }

        if(entranceDoor) Destroy(entranceDoor);
        if(targetDoor) {
            entranceDoor = targetDoor;
            entranceDoorScript = targetDoorScript;
        }
        targetDoor = Instantiate(doorPrefab);
        targetDoorScript = targetDoor.GetComponent<Door>();

        if(targetDoorScript == null) {
            Debug.LogError("Door prefab does not have a Door component!");
            return;
        }

        targetDoorScript.roomGenerator = this;

        float doorWidth = targetDoor.transform.localScale.y;
        float minDoorX = groundT.position.x - groundT.localScale.x / 2;
        float maxDoorX = groundT.position.x + groundT.localScale.x / 2;
        float minDoorY = groundT.position.y - groundT.localScale.y / 2;
        float maxDoorY = groundT.position.y + groundT.localScale.y / 2;
        float randomX = Random.Range(minDoorX + doorWidth / 2, maxDoorX - doorWidth / 2);
        float randomY = Random.Range(minDoorY + doorWidth / 2, maxDoorY - doorWidth / 2);

        Door.Side side;
        do{
            side = (Door.Side)Random.Range(0, 4);
        } while(entranceDoorScript && side == entranceDoorScript.side);

        switch(side) {
            case Door.Side.top: // up
                targetDoorScript.SetPosAndRotate(Door.Side.top, new Vector3(randomX, maxDoorY, 0));
                break;
            case Door.Side.left: // left
                targetDoorScript.SetPosAndRotate(Door.Side.left, new Vector3(minDoorX, randomY, 0));
                break;
            case Door.Side.bottom: // down
                targetDoorScript.SetPosAndRotate(Door.Side.bottom, new Vector3(randomX, minDoorY, 0));
                break;
            case Door.Side.right: // right
                targetDoorScript.SetPosAndRotate(Door.Side.right, new Vector3(maxDoorX, randomY, 0));
                break;
        }
    }

    public void UpdateRoomNumberUI()
    {
        roomNumber++;
        Debug.Log("Room Number: " + roomNumber);
        roomNumberText.text = roomNumber.ToString();
    }
}
