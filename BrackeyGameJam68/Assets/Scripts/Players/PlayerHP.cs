using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private int currentHealth;

    [SerializeField]
    private int maxHealth = 100;

    public HealthBarUpdate healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damageAmount)
    {
        if (damageAmount < 0) return;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            TakeDamage(20);
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " has died!");
        // Add game over logic, disable GameObject, play death animation, etc.
        Destroy(gameObject); 
    }
}
