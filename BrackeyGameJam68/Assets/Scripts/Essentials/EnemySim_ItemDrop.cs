using System.Collections.Generic;
using UnityEngine;

public enum LootQuality//***
{
    None,   // ไม่ได้
    Normal, // ปกติ
    Good,   // ดี
    Best    // ดีสุด
}

[System.Serializable]
public class LootItem//***
{
    public GameObject itemPrefab;
    public LootQuality quality;
}

public class EnemySim_ItemDrop : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Loot Table")]///***
    public List<LootItem> lootTable = new List<LootItem>();

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    void Update()
{
    // ถ้ากด Space จะโดนโจมตี 50 damage
    if (Input.GetKeyDown(KeyCode.Space))
    {
        TakeDamage(50);
        Debug.Log("Enemy took 50 damage! Current HP: " + currentHealth);
    }
}

    void Die()//***
    {
    LootItem chosen = GetWeightedLoot(); // เลือกไอเท็ม 1 ชิ้น
    if (chosen != null && chosen.itemPrefab != null)
        {
            InstantiateLoot(chosen.itemPrefab);
        }

        Destroy(gameObject); // ลบศัตรูหลังตาย
    }

    LootItem GetWeightedLoot()///***
    {
        float roll = Random.value; // 0-1
        float cumulative = 0f;

        foreach (LootItem lootItem in lootTable)
        {
            float chance = 0f;

            switch (lootItem.quality)
            {
                case LootQuality.Best: chance = 0.05f; break;  // 5%
                case LootQuality.Good: chance = 0.20f; break;  // 20%
                case LootQuality.Normal: chance = 0.55f; break; // 55%
                case LootQuality.None: chance = 0.20f; break;  // 20% ไม่ได้
            }

            cumulative += chance;

            if (roll <= cumulative)
            {
            // ถ้าเป็น None → ไม่ drop
                if (lootItem.quality == LootQuality.None)
                return null;

                return lootItem;
            }
        }

        return null; // ไม่ drop ถ้า roll เกินทั้งหมด
    }

    void InstantiateLoot(GameObject loot)///***
    {
        if (!loot) return;

        // สุ่มตำแหน่งรอบศัตรู (วงกลมใน XY plane)
        float radius = 1f; // ระยะกระจาย loot รอบศัตรู ปรับได้
        Vector2 randomOffset = Random.insideUnitCircle * radius;
        Vector3 spawnPos = transform.position + (Vector3)randomOffset;

        GameObject droppedLoot = Instantiate(loot, spawnPos, Quaternion.identity);

        // ใช้ Rigidbody2D กระจาย loot แบบฟิสิกส์
        Rigidbody2D rb = droppedLoot.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // สุ่มแรงดีดจากตำแหน่งศัตรูออกไป
            Vector2 forceDir = randomOffset.normalized; // กระเด้งออกจากศูนย์
            float force = Random.Range(2f, 5f); // ปรับแรงได้
            rb.AddForce(forceDir * force, ForceMode2D.Impulse);

            // เพิ่มการหมุนเล็กน้อยถ้าต้องการ
            rb.AddTorque(Random.Range(-5f, 5f), ForceMode2D.Impulse);
        }
    }

}