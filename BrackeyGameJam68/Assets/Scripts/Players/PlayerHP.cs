using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.AI; 
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
    [SerializeField] private AudioClip hurtaudio;

    [SerializeField] TMP_Text hpText;
    [SerializeField] PlayableDirector hurtAnim;
    [SerializeField] PlayableDirector gameOverAnim;

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

    public void TakeDamage(int damageAmount, GameObject attacker = null)
    {
        if (damageAmount < 0 || cd < 1) return;

        AudioManager.instance.PlaySound(hurtaudio);

        cd = 0;

        if (hurtAnim) hurtAnim.Play();

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0);

        hpText.text = currentHealth.ToString() + "/" + maxHealth.ToString();

        healthBar.SetHealth(currentHealth);
        Debug.Log(gameObject.name + " took " + damageAmount + " damage. Current Health: " + currentHealth);

        if (attacker != null) KnockbackEnemy(attacker);
        if (attacker != null && attacker.GetComponent<EnemySim_ItemDrop>().desSelf == true) Destroy(attacker);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void KnockbackEnemy(GameObject enemy)
    {
        float knockbackDistance = 15f; // ระยะผลัก

        Rigidbody2D agent = enemy.GetComponent<Rigidbody2D>();
        if (agent != null)
        {
            Vector3 knockDir = (enemy.transform.position - transform.position).normalized;
            agent.AddForce(knockDir * knockbackDistance, ForceMode2D.Impulse); 
        }
        else
        {
           
            enemy.transform.position += (enemy.transform.position - transform.position).normalized * knockbackDistance;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2f); 
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
        AudioManager.instance.MusicFade(1f, 0.03f, 0.5f);
        Debug.Log(gameObject.name + " has died!");
        gameOverAnim.Play();
        Time.timeScale = 0;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (cd >= 1.5f) 
        {
            if (col.gameObject.CompareTag("Enemy"))
            {
                TakeDamage(25, col.gameObject);
                cd = 0; 
            }
        }

        if (col.gameObject.CompareTag("Heal"))
        {
            currentHealth += Random.Range(15, 26);
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            healthBar.SetHealth(currentHealth);
        }
    }
}
