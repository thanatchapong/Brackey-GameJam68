using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int dmg = 1;
    public int bounce = 0;

    public float critChance = 0.1f;
    public float critMult = 2;

    public float knockbackForce = 2.5f;
    public int pierce = 0;

    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            // float finalDamage = baseDamage;
        
            // if (Random.value < criticalChance)
            // {
            //     finalDamage *= criticalMultiplier;
            // }

            if(pierce == 0)
            {
                Destroy(gameObject);
            }
        }
        else if(bounce <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            bounce -= 1;
        }
    }
}
