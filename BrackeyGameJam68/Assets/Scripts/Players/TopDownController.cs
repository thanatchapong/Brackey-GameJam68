using UnityEngine;
using System.Linq;

public class TopDownController : MonoBehaviour
{
    [SerializeField] UpgradeSystem upgSystem;

    [Header("Movement Settings")]
    [Tooltip("Maximum movement speed")]
    [SerializeField] float moveSpeed = 5f;

    [Tooltip("Acceleration for smooth movement")]
    [SerializeField] float acceleration = 10f;

    [Tooltip("Deceleration when no input is given")]
    [SerializeField] float deceleration = 10f;

    [SerializeField] Animator anim;
    [SerializeField] UpgradeObject SNIPER;

    float currentSpeed;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Quaternion rotation;
    private Vector2 currentVelocity;
    bool holdingSpace;
    bool sniperStunt = false;
    float sniperDuration;
    float sniperTimer = 0;
    float tempSpeed;

    public void onDestroyObstacle() {
        int sniperCount = upgSystem.upgInUse.Count(u => u == SNIPER);
        if(sniperCount > 0) {
            Debug.Log("Sniper Stunt Triggered");
            if(!sniperStunt) tempSpeed = currentSpeed;
            sniperStunt = true;
            sniperTimer = 0;
            sniperDuration = sniperCount * 2;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        currentSpeed = moveSpeed;
    }

    void Update()
    {
        if(sniperStunt) {
            sniperTimer += Time.deltaTime;
            currentSpeed = 0;
            if(sniperTimer >= sniperDuration) {
                Debug.Log("Sniper Stunt Ended");
                sniperStunt = false;
                currentSpeed = tempSpeed;
                sniperTimer = 0;
            }
        }

        if (DialogueUI.instance && DialogueUI.instance.isActive)
        {
            movement = Vector2.zero;
            rotation = Quaternion.Euler(0, 0, 0);
            anim.SetBool("walking", false);
            return;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (anim && horizontalInput == 0 && verticalInput == 0)
        {
            anim.SetBool("walking", false);
        }
        else
        {
            anim.SetBool("walking", true);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            holdingSpace = true;
        }
        else
        {
            holdingSpace = false;
        }

        movement = new Vector2(horizontalInput, verticalInput).normalized;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void FixedUpdate()
    {
        if (DialogueUI.instance && DialogueUI.instance.isActive)
        {
            currentVelocity = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            return;
        }

        if (movement != Vector2.zero)
        {
            if (holdingSpace) currentVelocity = Vector2.Lerp(currentVelocity, movement * (currentSpeed * 0.25f), acceleration * Time.fixedDeltaTime);
            else currentVelocity = Vector2.Lerp(currentVelocity, movement * currentSpeed, acceleration * Time.fixedDeltaTime);
            rb.linearVelocity = currentVelocity;
        }
        else
        {
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            rb.linearVelocity = currentVelocity;
        }
    }

    public void GetUpgrade()
    {
        currentSpeed = moveSpeed;
        //Upgrade
        if (upgSystem.upgInUse.Count > 0)
        {
            foreach (UpgradeObject upg in upgSystem.upgInUse)
            {
                currentSpeed += upg.walkSpeed;
            }
        }
    }
}
