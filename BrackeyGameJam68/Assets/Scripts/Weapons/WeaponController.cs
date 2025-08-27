using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] WeaponsObject currentWeapon;
    float cd;

    private Animator anim;

    void Start()
    {
        GameObject weaponInstance = Instantiate(currentWeapon.weaponPrefab, transform);

        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }
    }

    void Update()
    {
        cd += Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse0) && cd >= currentWeapon.fireRate)
        {
            cd = 0;
            Shoot();
        }
    }

    void BulletStat(Bullet bullet)
    {
        bullet.dmg = currentWeapon.baseDamage;
        bullet.bounce = currentWeapon.bounce;

        bullet.critChance = currentWeapon.criticalChance;
        bullet.critMult = currentWeapon.criticalMultiplier;

        bullet.knockbackForce = currentWeapon.knockbackForce;
        bullet.pierce = currentWeapon.pierce;
    }

    void Shoot()
    {
        if (currentWeapon.weaponType == WeaponsObject.WeaponType.Pistol)
        {
            Rigidbody2D bullet = Instantiate(currentWeapon.ammoPrefab, transform.position, transform.rotation).GetComponent<Rigidbody2D>();
            BulletStat(bullet.GetComponent<Bullet>());

            bullet.AddForce(transform.forward * currentWeapon.bulletSpeed, ForceMode2D.Impulse);

            anim.SetTrigger("Shoot");

            GetComponent<PrositionalAudio>().Play();
        }
    }
}
