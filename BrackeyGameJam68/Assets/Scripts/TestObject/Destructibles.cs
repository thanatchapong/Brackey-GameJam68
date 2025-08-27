using UnityEngine;

public class Destructibles : MonoBehaviour
{
    [SerializeField]
    int health = 1;

    [SerializeField]
    float shakeAmount = 0.1f;


    [SerializeField]
    ParticleSystem particleExplosionRef;

    public GameObject targetObject;

    bool isShaking = false;
    Vector2 startPos;
    
    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if(isShaking)
        {
            transform.position = startPos + UnityEngine.Random.insideUnitCircle * shakeAmount;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            health--;

            if (health <= 0)
            {
                Disintegrate();
            }
            else 
            {
                isShaking = true;
                Invoke("ResetShake", 0.2f);
            }
        }
        
    }

    private void ResetShake() 
    {
        isShaking = false;
        transform.position = startPos;
    }

    private void Disintegrate()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        BoxCollider2D bc2 = GetComponent<BoxCollider2D>();
        sr.enabled = false;
        bc2.enabled = false;
        
        ParticleSystem ps = Instantiate(particleExplosionRef, transform.position, Quaternion.identity);
        ps.Play();

        float totalDuration = ps.main.duration + ps.main.startLifetime.constantMax;
        Destroy(ps.gameObject, totalDuration);

        DisintegrateObject disintegrator = targetObject.GetComponent<DisintegrateObject>();
        if (disintegrator != null)
        {
            StartCoroutine(disintegrator.DisintegrateSelf());
        }

        Destroy(gameObject);
    }


}

