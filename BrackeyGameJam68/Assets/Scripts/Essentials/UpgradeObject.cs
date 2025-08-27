using UnityEngine;

[CreateAssetMenu(fileName = "UpgradePerk", menuName = "Upgrade System/UpgradePerk")]
public class UpgradeObject : ScriptableObject
{
    public string upgradeName;
    public string upgradeDetails;
    public Texture upgradeIcon;
    public bool isRisk = false;

    //------------Upgrade Stats----------------
    [Header("Players")]
    public int hp;
    public int walkSpeed;
    public int blockCooldown;

    [Header("Weapon")]
    public int damage;
    public float criticalChance;
    public float criticalMultiplier;
    public float accuracy;
    public float fireRate;

    [Header("Ammunition")]
    public int magazineSize;
    public float ammoSizeMult;
    public float reloadTime;
    public int bounce;
    public int pierce;
    public float bulletSpeed;
    public float knockbackForce;
}
