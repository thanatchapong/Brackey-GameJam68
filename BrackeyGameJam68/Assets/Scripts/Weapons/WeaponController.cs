using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponsObject currentWeapon;

    void Start()
    {
        // Instantiate weapon from ScriptableObject
        GameObject weaponInstance = Instantiate(currentWeapon.weaponPrefab, transform);
    }

    void Shoot()
    {
        float damage = currentWeapon.CalculateDamage();
        // Shooting logic
    }
}
