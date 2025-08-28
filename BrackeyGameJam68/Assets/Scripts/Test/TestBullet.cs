using UnityEngine;

public class TestBullet : MonoBehaviour
{
    [Header("Stats")]
    public int dmg = 1;
    public float speed = 5f;

    [Header("Special")]
    public int bounce = 0;
    public int pierce = 0;

    public float critChance = 0.1f;
    public float critMult = 2f;

    public float knockbackForce = 2.5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.right * speed; // ให้พุ่งไปทางขวา
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            float finalDamage = dmg;
            if (Random.value < critChance)
                finalDamage *= critMult;

            EnemySim_ItemDrop enemy = col.GetComponent<EnemySim_ItemDrop>();
            if (enemy != null)
                enemy.TakeDamage(Mathf.RoundToInt(finalDamage));

            Rigidbody2D enemyRb = col.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 knockDir = (col.transform.position - transform.position).normalized;
                enemyRb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);
            }

            if (pierce > 0) { pierce--; return; }
            Destroy(gameObject);
        }
        else if (bounce > 0)
        {
            bounce--;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}