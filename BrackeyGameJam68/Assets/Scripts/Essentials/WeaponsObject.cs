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
    public int baseDamage = 10;
    public DamageType damageType = DamageType.Physical;
    public float criticalChance = 0.1f;
    public float criticalMultiplier = 2f;

    // Range & Accuracy
    [Header("Weapon Mechanics")]
    public float accuracy = 0.9f;
    public float fireRate = 1f;

    // Ammo & Reload (for guns)
    [Header("Ammunition")]
    public int magazineSize = 5;
    public int currentAmmo;
    public float reloadTime = 2f;
    public AmmoType ammoType;
    public int bounce = 0;
    public int pierce = 0;
    public float bulletSpeed = 20f;
    public float knockbackForce = 5f;

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
}