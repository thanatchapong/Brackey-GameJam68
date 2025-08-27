using UnityEngine;

public class TopDownController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Maximum movement speed")]
    [SerializeField] float moveSpeed = 5f;

    [Tooltip("Acceleration for smooth movement")]
    [SerializeField] float acceleration = 10f;

    [Tooltip("Deceleration when no input is given")]
    [SerializeField] float deceleration = 10f;

    [SerializeField] Animator anim;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 currentVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        if (DialogueUI.instance && DialogueUI.instance.isActive)
        {
            movement = Vector2.zero;
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
            currentVelocity = Vector2.Lerp(currentVelocity, movement * moveSpeed, acceleration * Time.fixedDeltaTime);
            rb.linearVelocity = currentVelocity;
        }
        else
        {
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            rb.linearVelocity = currentVelocity;
        }
    }
}
