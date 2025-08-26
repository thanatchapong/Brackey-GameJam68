using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon System/Weapon")]
public class WeaponsObject : ScriptableObject
{
        // Core Weapon Identity
    public string weaponName;
    public WeaponType weaponType;
    public Sprite weaponIcon;
    public GameObject weaponPrefab;
    public GameObject ammoPrefab;

    // Combat Stats
    [Header("Damage")]
    public float baseDamage = 10f;
    public DamageType damageType = DamageType.Physical;
    public float criticalChance = 0.1f;
    public float criticalMultiplier = 2f;

    // Range & Accuracy
    [Header("Weapon Mechanics")]
    public float range = 10f;
    public float accuracy = 0.9f;
    public float fireRate = 1f;

    // Ammo & Reload (for guns)
    [Header("Ammunition")]
    public int magazineSize = 30;
    public int currentAmmo;
    public float reloadTime = 2f;
    public AmmoType ammoType;
    public float bulletSpeed = 20f;
    public float knockbackForce = 5f;

    // Weapon Progression
    [Header("Upgrades")]
    public int weaponLevel = 1;
    public float upgradeDamageFactor = 1.1f;

    // Sound & Effects
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    // Enums for Type Categorization
    public enum WeaponType
    {
        Pistol,
        Shotgun,
        Rifle,
        Sniper
    }

    public enum DamageType
    {
        Physical,
        Fire,
        Ice,
        Electric,
        Poison
    }

    public enum AmmoType
    {
        Pistol,
        Shotgun,
        Rifle,
        None
    }

    // Method to calculate final damage
    public float CalculateDamage()
    {
        float finalDamage = baseDamage;
        
        // Critical hit calculation
        if (Random.value < criticalChance)
        {
            finalDamage *= criticalMultiplier;
        }

        return finalDamage;
    }
}