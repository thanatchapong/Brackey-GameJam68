using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int dmg = 1;
    public int bounce = 0;

    public float critChance = 0.1f;
    public float critMult = 2;

    public float knockbackForce = 2.5f;
    public int pierce = 0;
    [SerializeField] ParticleSystem hitEff;

    public void OnTriggerEnter2D(Collider2D col)
    {
        Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
        if (rb) rb.AddForce(transform.forward * knockbackForce, ForceMode2D.Impulse);

        if (col.gameObject.tag == "Enemy")
        {
            pierce -= 1;

            //Do Dmg
            col.gameObject.GetComponent<EnemySim_ItemDrop>().TakeDamage(dmg);

            if (pierce < 0)
            {
                if (hitEff) Instantiate(hitEff, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
        else if (bounce <= 0)
        {
            if (hitEff) Instantiate(hitEff, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            bounce -= 1;
        }
    }
}
