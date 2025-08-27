using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 20f; 
    public int damage = 10; 
    public float lifetime = 3f; 
    public string playerTag = "Player";

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        {
            rb.linearVelocity = transform.up * speed;
        }

        Destroy(gameObject, lifetime);
    }

    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            PlayerHP playerHP = collision.gameObject.GetComponent<PlayerHP>();

            if (playerHP != null)
            {
                playerHP.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}