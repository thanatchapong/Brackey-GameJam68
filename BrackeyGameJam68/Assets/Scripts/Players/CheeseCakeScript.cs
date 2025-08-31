    using UnityEngine;

public class CheeseCakeScript : MonoBehaviour
{
    [Header("Movement Settings")]

    [Tooltip("Maximum movement speed")]
    [SerializeField] float moveSpeed = 5f;

    [Tooltip("Acceleration for smooth movement")]
    [SerializeField] float acceleration = 10f;

    [Tooltip("Deceleration when no input is given")]
    [SerializeField] float deceleration = 10f;

    [Header("Distance")]
    [SerializeField] float movingDistance = 0.25f;
    [SerializeField] float stoppingDistance = 1f;

    Animator anim;

    Transform door;

    private Rigidbody2D rb;

    Vector2 desiredPosition;

    GameObject Door;

    bool isDoorFound = false;

    private Vector2 movement;
    private Quaternion rotation;
    private Vector2 currentVelocity;

    Transform target;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
    }

    void Update()
    {
        Door = GameObject.Find("Door(Clone)");

        if (Door != null)
        {
            if (Door.GetComponent<Door>().isActive)
                isDoorFound = true;
            else
                Door.name = "Door(Inactive)";
        }
        else
        {
            isDoorFound = false;
        }

        if (!isDoorFound)
            movement = (new Vector2(target.position.x, target.position.y) - rb.position).normalized;
        else
            movement = (new Vector2(Door.transform.position.x, Door.transform.position.y) - rb.position).normalized;



        rotation = Quaternion.Euler(0, 0, 0);

        transform.rotation = rotation;
    }

    void FixedUpdate()
    {
        if (target == null) return; // Ensure target is set

        float distanceToTarget = Vector2.Distance(rb.position, target.position);

        if (distanceToTarget > 10f)
        {
            rb.position = target.position;
        }

        if (!isDoorFound)
        {
            if (distanceToTarget > stoppingDistance)
            {
                currentVelocity = Vector2.Lerp(currentVelocity, movement * moveSpeed, acceleration * Time.fixedDeltaTime);
                rb.linearVelocity = currentVelocity;
            }
            else if (distanceToTarget < movingDistance)
            {
                currentVelocity = Vector2.Lerp(currentVelocity, -1 * movement * moveSpeed, acceleration * Time.fixedDeltaTime);
                rb.linearVelocity = currentVelocity;
            }
            else
            {
                currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
                rb.linearVelocity = currentVelocity;
            }
        }
        else
        {
            if (distanceToTarget < stoppingDistance)
            {
                currentVelocity = Vector2.Lerp(currentVelocity, movement * moveSpeed, acceleration * Time.fixedDeltaTime);
                rb.linearVelocity = currentVelocity;
            }
            else if (distanceToTarget > stoppingDistance + .125f)
            {
                movement = (new Vector2(target.position.x, target.position.y) - rb.position).normalized;
                currentVelocity = Vector2.Lerp(currentVelocity, movement * moveSpeed, acceleration * Time.fixedDeltaTime);
                rb.linearVelocity = currentVelocity;
            }
            else
            {
                currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
                rb.linearVelocity = currentVelocity;
            }
        }

        if (rb.linearVelocity.x < 0)
        {
            transform.localScale = new Vector2(-1f, 1f);
        }
        else
        {
            transform.localScale = new Vector2(1f, 1f);
        }

        if (rb.linearVelocity.magnitude > 0.1f)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
    }
}