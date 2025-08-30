using UnityEngine;

public class Door : MonoBehaviour
{
    public RoomGenerator roomGenerator;
    private Transform playerTransform;
    public Transform cameraTransform;
    private BoxCollider2D boxCollider2D;
    private Vector3 playerSpawnpoint = Vector3.zero;

    [Header("Door Settings")]
    public bool isActive = true;
    public bool isHard = false;
    public enum Side : int { top = 0, left = 1, bottom = 2, right = 3 };
    public Side side;
    [SerializeField] Color inactiveColor = Color.gray;
    [SerializeField] Color activeColor = new Color(145, 255, 118);
    [SerializeField] Color hardColor = Color.red;

    DoorAudio doorAudio;
    
    public void SetInactive()
    {
        isActive = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        GetComponent<SpriteRenderer>().color = inactiveColor;
    }

    public void SetActive()
    {
        isActive = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().color = activeColor;
        
        transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = activeColor;
        transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = activeColor;
        transform.GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().color = activeColor;
    }

    public void SetHard() {
        Debug.Log("SetHard Door");
        isActive = true;
        isHard = true;

        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().color = hardColor;
        
        transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = hardColor;
        transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = hardColor;
        transform.GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().color = hardColor;
    }

    public void SetOpposite() {
        SetPosAndRotate(GetOppositeSide(), GetOppositePosition());
    }

    private Side GetOppositeSide()
    {
        return (Side)(((int)side + 2) % 4);
    }

    private Vector3 GetOppositePosition()
    {
        Transform groundT = roomGenerator.level.transform.Find("Ground");
        if (groundT == null)
        {
            Debug.LogError("No 'Ground' child found in level GameObject!");
            return Vector3.zero;
        }

        float minX = groundT.position.x - groundT.localScale.x / 2;
        float maxX = groundT.position.x + groundT.localScale.x / 2;
        float minY = groundT.position.y - groundT.localScale.y / 2;
        float maxY = groundT.position.y + groundT.localScale.y / 2;

        Vector3 currentPos = transform.position;

        switch (side)
        {
            case Side.top: return new Vector3(currentPos.x, minY, currentPos.z);
            case Side.bottom: return new Vector3(currentPos.x, maxY, currentPos.z);
            case Side.left: return new Vector3(maxX, currentPos.y, currentPos.z);
            case Side.right: return new Vector3(minX, currentPos.y, currentPos.z);
            default: return Vector3.zero;
        }
    }
    
    public void SetPosAndRotate(Side newSide, Vector3 position)
    {
        side = newSide;
        transform.position = position;
        switch(side) { // pos
            case Side.top:
                playerSpawnpoint = new Vector3(position.x, position.y - 0.5f, position.z);
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case Side.left:
                playerSpawnpoint = new Vector3(position.x + 0.5f, position.y, position.z);
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case Side.bottom:
                playerSpawnpoint = new Vector3(position.x, position.y + 0.5f, position.z);
                transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case Side.right:
                playerSpawnpoint = new Vector3(position.x - 0.5f, position.y, position.z);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
        }
    }

    void Start()
    {
        doorAudio = gameObject.GetComponent<DoorAudio>();
        // Initialize components
        boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D == null)
        {
            Debug.LogError("Door requires a BoxCollider2D component!");
        }
        else
        {
            boxCollider2D.isTrigger = true;
        }
        
        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        else
        {
            Debug.LogError("No GameObject with 'Player' tag found!");
        }
        
        SetPosAndRotate(side, transform.position);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"[DOOR] ENTER | isHard={isHard}");
        if(collision.CompareTag("Player") && isActive)
        {
            doorAudio.PlayDoorSound(isHard);
            SetInactive();
            SetOpposite();
            playerTransform.position = playerSpawnpoint;
            cameraTransform.position = new Vector3(playerSpawnpoint.x, playerSpawnpoint.y, cameraTransform.position.z);
            Debug.LogError("DOOR GENERATE ROOM");
            roomGenerator.GenerateRoom(isHard);
        }
    }
}
