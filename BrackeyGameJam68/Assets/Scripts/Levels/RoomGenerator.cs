using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] List<GameObject> templateList;
    [SerializeField] List<GameObject> hardTemplateList;
    [SerializeField] List<GameObject> obstacleList;
    [SerializeField] GameObject doorPrefab;
    [SerializeField] GameObject wallPrefab;
    private GameObject entranceDoor, targetDoor1, targetDoor2;
    private Door entranceDoorScript, targetDoorScript1, targetDoorScript2;
    [SerializeField] TopDownController playerController;
    private GameObject currentRoom = null;
    public GameObject level;
    private int roomNumber = 0;

    [Header("Door Clearance Settings")]
    [SerializeField] private float doorClearanceRadius = 1.25f;

    [Header("UI")]
    public TextMeshPro roomNumberText;
    public DialogueTrigger dialogueTrigger;
    [SerializeField] private UpgradeSystem upgradeSystem;

    [SerializeField] private Transform cameraTransform;

    [Header("Upgrades")]
    [SerializeField] private UpgradeObject RICOCHET;
    [SerializeField] private UpgradeObject RNG_ROOM;
    public bool rngRoomTrigger = false;
    [SerializeField] EnemyGenerator enemyGen;

    public void SetDoorActive(bool door1Active, bool door2Active) {
        if (targetDoor1 != null) targetDoor1.SetActive(door1Active);
        if (targetDoor2 != null) targetDoor2.SetActive(door2Active);
    }

    public void GenerateRoom(bool isHard)
    {
        enemyGen.SpawnWave(roomNumber);

        Debug.Log($"[GenerateRoom] start | isHard={isHard} | currentRoomNumber(before++)={roomNumber}");
        int rngRoomCount = 0;
        if (upgradeSystem == null)
        {
            Debug.LogWarning("[GenerateRoom] UpgradeSystem is null. RNG room will not trigger.");
        }
        else if (upgradeSystem.upgInUse == null)
        {
            Debug.LogWarning("[GenerateRoom] upgradeSystem.upgInUse is null. RNG room will not trigger.");
        }
        else
        {
            rngRoomCount = upgradeSystem.upgInUse.Count(u => u == RNG_ROOM);
        }

        int roll = Random.Range(0, 5);
        Debug.Log($"[GenerateRoom] RNG roll={roll}, rngRoomCount={rngRoomCount}");
        if (roll < rngRoomCount)
        {
            Debug.LogError("RNG Room Triggered");
            rngRoomTrigger = true;
        }
        else
        {
            Debug.LogError("RNG Room Not Triggered");
            rngRoomTrigger = false;
        }
        UpdateRoomNumberUI();
        LevelColorChanger colorChanger = level.GetComponent<LevelColorChanger>();
        if (colorChanger) colorChanger.ApplyColors(roomNumber);

        if (currentRoom) Destroy(currentRoom);

        if (templateList == null || templateList.Count == 0)
        {
            Debug.LogError("Template list is empty or not assigned in RoomGenerator!");
            return;
        }

        GameObject selectedTemplate;
        if (isHard)
        {
            selectedTemplate = hardTemplateList[Random.Range(0, hardTemplateList.Count)];
        }
        else
        {
            selectedTemplate = templateList[Random.Range(0, templateList.Count)];
        }
        currentRoom = Instantiate(selectedTemplate, Vector3.zero, Quaternion.identity);

        if (currentRoom.CompareTag("SpawnRandomWalls"))
        {
            GenerateWalls();
        }

        ColorRoomWalls();

        Transform[] allChildren = currentRoom.GetComponentsInChildren<Transform>();

        foreach (Transform spawnpoint in allChildren)
        {
            if (!spawnpoint.gameObject.CompareTag("ObstacleSpawnpoint")) continue;
            int ricochetCount = upgradeSystem.upgInUse.Count(u => u == RICOCHET);
            for (int i = 0; i < (ricochetCount + 1) * 3; i++)
            {
                if (Random.Range(0, 2) >= 1) continue;
                int obstacleIdx = Random.Range(0, obstacleList.Count);
                GameObject selectedObstacle = obstacleList[obstacleIdx];
                float randomZ = Random.Range(-180f, 180f);
                Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomZ);
                GameObject obstacle = Instantiate(selectedObstacle, spawnpoint.position, randomRotation, currentRoom.transform);
                obstacle.TryGetComponent(out Destructibles destructibles);
                if (destructibles) destructibles.playerController = playerController;
            }
        }
        GenerateDoor(isHard);

        if (entranceDoor != null) ClearObstacles(entranceDoor.transform.position);
        if (targetDoor1 != null) ClearObstacles(targetDoor1.transform.position);
        if (targetDoor2 != null) ClearObstacles(targetDoor2.transform.position);

        
        dialogueTrigger.TriggerDialogue(roomNumber - 1);
    }

    private void ColorRoomWalls()
    {
        var wallT = level.transform.Find("WallL");
        var wallSR = wallT.GetComponent<SpriteRenderer>();
        Color wallColor = wallSR.color;

        Transform[] children = currentRoom.GetComponentsInChildren<Transform>(true);
        foreach (var childT in children)
        {
            if (childT == null) continue;
            if (childT.name.Contains("ObstacleWall"))
            {
                var childSR = childT.GetComponent<SpriteRenderer>();
                childSR.color = wallColor;
            }
        }
    }

    private void GenerateWalls()
    {
        Debug.Log("Generating walls...");
        Transform groundT = level.transform.Find("Ground");
        float minX = groundT.position.x - groundT.localScale.x / 2 + 3;
        float maxX = groundT.position.x + groundT.localScale.x / 2 - 3;
        float minY = groundT.position.y - groundT.localScale.y / 2 + 3;
        float maxY = groundT.position.y + groundT.localScale.y / 2 - 3;

        int desiredWalls = Random.Range(3, 6);
        int placed = 0;
        int attempts = 0;
        int maxAttempts = 50;

        List<Collider2D> placedWallColliders = new List<Collider2D>();

        while (placed < desiredWalls && attempts < maxAttempts)
        {
            attempts++;

            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            float randomRotation = Random.Range(0, 360);
            float randomLength = Random.Range(4, 7);

            GameObject wall = Instantiate(
                wallPrefab,
                new Vector3(randomX, randomY, 0),
                Quaternion.Euler(0, 0, randomRotation),
                currentRoom.transform
            );
            wall.transform.localScale = new Vector3(0.75f, randomLength, 1);

            Collider2D newCol = wall.GetComponent<Collider2D>();

            bool overlaps = false;
            for (int i = 0; i < placedWallColliders.Count; i++)
            {
                var otherCol = placedWallColliders[i];
                if (newCol.bounds.Intersects(otherCol.bounds))
                {
                    overlaps = true;
                }
            }

            if (overlaps)
            {
                Destroy(wall);
                continue;
            }

            placedWallColliders.Add(newCol);
            placed++;
        }
    }

    private void ClearObstacles(Vector3 doorPos)
    {
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

    public void GenerateDoor(bool isHard)
    {
        if (level == null)
        {
            Debug.LogError("Level GameObject is not assigned in RoomGenerator!");
            return;
        }

        Transform groundT = level.transform.Find("Ground");

        if (groundT == null)
        {
            Debug.LogError("No 'Ground' child found in level GameObject: " + level.name);
            return;
        }

        if (doorPrefab == null)
        {
            Debug.LogError("Door prefab is not assigned in RoomGenerator!");
            return;
        }

        if (entranceDoor) Destroy(entranceDoor);
        if (isHard)
        {
            Destroy(targetDoor1);
            targetDoor1 = targetDoor2;
            targetDoorScript1 = targetDoorScript2;
        }
        else
        {
            Destroy(targetDoor2);
        }
        entranceDoor = targetDoor1;
        entranceDoorScript = targetDoorScript1;

        targetDoor1 = Instantiate(doorPrefab);
        targetDoorScript1 = targetDoor1.GetComponent<Door>();

        if (targetDoorScript1 == null)
        {
            Debug.LogError("Door prefab does not have a Door component!");
            return;
        }

        targetDoorScript1.roomGenerator = this;
        targetDoorScript1.cameraTransform = cameraTransform;

        float doorWidth = targetDoor1.transform.localScale.y;
        float minDoorX = groundT.position.x - groundT.localScale.x / 2;
        float maxDoorX = groundT.position.x + groundT.localScale.x / 2;
        float minDoorY = groundT.position.y - groundT.localScale.y / 2;
        float maxDoorY = groundT.position.y + groundT.localScale.y / 2;
        float randomX = Random.Range(minDoorX + doorWidth / 2, maxDoorX - doorWidth / 2);
        float randomY = Random.Range(minDoorY + doorWidth / 2, maxDoorY - doorWidth / 2);

        Door.Side side;
        do
        {
            side = (Door.Side)Random.Range(0, 4);
        } while (entranceDoorScript != null && side == entranceDoorScript.side);

        targetDoorScript1.SetPosAndRotate(side, ComputeDoorPosition(side, minDoorX, maxDoorX, minDoorY, maxDoorY, randomX, randomY));
        targetDoor1.SetActive(false);

        if (roomNumber % 5 == 0)
        {
            targetDoor2 = Instantiate(doorPrefab);
            targetDoorScript2 = targetDoor2.GetComponent<Door>();
            targetDoorScript2.roomGenerator = this;
            targetDoorScript2.cameraTransform = cameraTransform;
            targetDoorScript2.SetHard();
            randomX = Random.Range(minDoorX + doorWidth / 2, maxDoorX - doorWidth / 2);
            randomY = Random.Range(minDoorY + doorWidth / 2, maxDoorY - doorWidth / 2);

            do
            {
                side = (Door.Side)Random.Range(0, 4);
            } while ((entranceDoorScript != null && side == entranceDoorScript.side) || side == targetDoorScript1.side);

            targetDoorScript2.SetPosAndRotate(side, ComputeDoorPosition(side, minDoorX, maxDoorX, minDoorY, maxDoorY, randomX, randomY));
            
            targetDoor2.SetActive(false);
        }
    }

    private static Vector3 ComputeDoorPosition(Door.Side side, float minDoorX, float maxDoorX, float minDoorY, float maxDoorY, float randomX, float randomY)
    {
        float x = side == Door.Side.left ? minDoorX :
                side == Door.Side.right ? maxDoorX : randomX;
        float y = side == Door.Side.top ? maxDoorY :
                side == Door.Side.bottom ? minDoorY : randomY;
        return new Vector3(x, y, 0f);
    }

    public void UpdateRoomNumberUI()
    {
        roomNumber++;
        Debug.Log("Room Number: " + roomNumber);
        roomNumberText.text = (roomNumber).ToString();
    }
}
