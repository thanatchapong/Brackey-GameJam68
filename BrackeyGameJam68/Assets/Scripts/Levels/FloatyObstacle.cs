using UnityEngine;

public class FloatyObstacle : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Water Float Settings")]
    [SerializeField] private float driftSpeed = 0.5f;
    [SerializeField] private float noiseTimeScale = 0.4f;
    private Vector2 startPos;
    private float noiseSeed1, noiseSeed2;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        noiseSeed1 = Random.value * 1000f;
        noiseSeed2 = Random.value * 1000f;
    }

    void Start()
    {
        startPos = rb.position;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(Random.Range(-0.3f, 0.3f), 0f);
    }

    void FixedUpdate()
    {
        float t = Time.time;

        float noise1 = Mathf.PerlinNoise(noiseSeed1 + t * noiseTimeScale, noiseSeed1) - 0.5f;
        float noise2 = Mathf.PerlinNoise(noiseSeed2 + t * noiseTimeScale, noiseSeed2) - 0.5f;
        float targetVx = noise1 * driftSpeed;
        float targetVy = noise2 * driftSpeed;
        float ax = targetVx - rb.linearVelocity.x;
        float ay = targetVy - rb.linearVelocity.y;

        rb.AddForce(new Vector2(ax, ay), ForceMode2D.Force);
    }
}
