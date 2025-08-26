using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] WeaponsObject currentWeapon;

    void Start()
    {
        // Instantiate weapon from ScriptableObject
        GameObject weaponInstance = Instantiate(currentWeapon.weaponPrefab, transform);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (currentWeapon.weaponType == WeaponsObject.WeaponType.Pistol)
        {
            Rigidbody2D bullet = Instantiate(currentWeapon.ammoPrefab, transform.position, transform.rotation).GetComponent<Rigidbody2D>();
            bullet.AddForce(transform.forward * currentWeapon.bulletSpeed, ForceMode2D.Impulse);
        }

        float damage = currentWeapon.CalculateDamage();
    }
}
