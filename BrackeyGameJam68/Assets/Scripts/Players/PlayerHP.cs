using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private int currentHealth;

    [SerializeField]
    private int maxHealth = 100;

    float cd = 0;

    public HealthBarUpdate healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        cd += Time.deltaTime;
    }

    public void TakeDamage(int damageAmount)
    {
        if (damageAmount < 0 || cd < 1) return;

        cd = 0;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0);

        healthBar.SetHealth(currentHealth);
        Debug.Log(gameObject.name + " took " + damageAmount + " damage. Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        if (healAmount < 0) return;

        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log(gameObject.name + " healed for " + healAmount + ". Current Health: " + currentHealth);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " has died!");
        // Add game over logic, disable GameObject, play death animation, etc.
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            TakeDamage(25);
        }
        else if (col.gameObject.tag == "Heal")
        {
            currentHealth += Random.Range(15, 26);
            currentHealth = Mathf.Min(currentHealth, maxHealth);

            healthBar.SetHealth(currentHealth);
        }
    }
}
