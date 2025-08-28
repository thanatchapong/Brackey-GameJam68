using UnityEngine;
using TMPro;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] UpgradeSystem upgSystem;

    [SerializeField]
    private int currentHealth;

    [SerializeField]
    private int maxHealth = 100;

    float cd = 0;

    public HealthBarUpdate healthBar;

    [Header("Audio")]

    [SerializeField]
    private AudioClip hurtaudio;

    [SerializeField] TMP_Text hpText;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        
        hpText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    void Update()
    {
        cd += Time.deltaTime;

        currentHealth = Mathf.Min(currentHealth, maxHealth);
        hpText.text = currentHealth.ToString() + "/" + maxHealth.ToString();    
    }

    public void GetUpgrade()
    {
        maxHealth = 100;
        //Upgrade
        if (upgSystem.upgInUse.Count > 0)
        {
            foreach (UpgradeObject upg in upgSystem.upgInUse)
            {
                maxHealth += upg.hp;
            }
        }
        
        hpText.text = currentHealth.ToString() + "/" + maxHealth.ToString();

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    public void TakeDamage(int damageAmount)
    {
        if (damageAmount < 0 || cd < 1) return;

        AudioManager.instance.PlaySound(hurtaudio);

        cd = 0;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0);
        
        hpText.text = currentHealth.ToString() + "/" + maxHealth.ToString();

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

        hpText.text = currentHealth.ToString() + "/" + maxHealth.ToString();    

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
