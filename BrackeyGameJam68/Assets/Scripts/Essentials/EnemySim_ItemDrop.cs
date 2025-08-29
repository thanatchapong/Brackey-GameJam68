using System.Collections.Generic;
using UnityEngine;

public enum LootQuality { None, Normal, Good, Best }

[System.Serializable]
public class LootItem
{
    public GameObject itemPrefab;
    public LootQuality quality;
}

public class EnemySim_ItemDrop : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 50;
    private int currentHealth;

    [Header("Loot Table")]
    public List<LootItem> lootTable = new List<LootItem>();

    private EnemySoundScript soundScript;

    [SerializeField] GameObject dedPar;

    void Start()
    {
        currentHealth = maxHealth;

        soundScript = gameObject.GetComponent<EnemySoundScript>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (damageAmount <= 0) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"{gameObject.name} took {damageAmount} damage. Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died!");

        soundScript.PlayDeathSound();

        if (dedPar) Instantiate(dedPar, transform.position, Quaternion.identity);

        LootItem chosen = GetWeightedLoot();
        if (chosen != null && chosen.itemPrefab != null)
        {
            if (chosen.quality == LootQuality.Normal)
            {
                for (int i = 0; i < Random.Range(1, 6); i++)
                {
                    InstantiateLoot(chosen.itemPrefab);
                }
            }
            else if (chosen.quality == LootQuality.Good)
            {
                for (int i = 0; i < Random.Range(1, 4); i++)
                {
                    InstantiateLoot(chosen.itemPrefab);
                }
            }
            else
            {
                InstantiateLoot(chosen.itemPrefab);
            }
        }

        Destroy(gameObject);
    }

    LootItem GetWeightedLoot()
    {
        float roll = Random.value;
        float cumulative = 0f;

        foreach (LootItem lootItem in lootTable)
        {
            float chance = lootItem.quality switch
            {
                LootQuality.Best => 0.05f,
                LootQuality.Good => 0.20f,
                LootQuality.Normal => 0.55f,
                LootQuality.None => 0.20f,
                _ => 0f
            };

            cumulative += chance;

            if (roll <= cumulative)
                return lootItem.quality == LootQuality.None ? null : lootItem;
        }

        return null;
    }

    void InstantiateLoot(GameObject loot)
    {
        if (!loot) return;

        float radius = 1f;
        Vector2 randomOffset = Random.insideUnitCircle * radius;
        Vector3 spawnPos = transform.position + (Vector3)randomOffset;

        GameObject droppedLoot = Instantiate(loot, spawnPos, Quaternion.identity);
        Rigidbody2D rb = droppedLoot.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 forceDir = randomOffset.normalized;
            float force = Random.Range(2f, 5f);
            rb.AddForce(forceDir * force, ForceMode2D.Impulse);
            rb.AddTorque(Random.Range(-5f, 5f), ForceMode2D.Impulse);
        }
    }
}