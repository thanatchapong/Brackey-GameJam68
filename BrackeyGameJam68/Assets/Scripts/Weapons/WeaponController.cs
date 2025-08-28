using SmoothShakeFree;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class WeaponController : MonoBehaviour
{
    [SerializeField] WeaponsObject currentWeapon;
    [SerializeField] UpgradeSystem upgSystem;
    [SerializeField] SmoothShake camShake;
    [SerializeField] Transform hands;
    [SerializeField] PlayableDirector shotAnim;
    [SerializeField] Slider reloadBar;
    float spread;
    int magazine;
    int ammo;
    float reloadTime;

    float cd;
    float reloadCount;
    bool reloading;
    private Animator anim;
    private AudioSource audio;

    void Start()
    {
        GameObject weaponInstance = Instantiate(currentWeapon.weaponPrefab, hands);

        if (anim == null)
        {
            anim = hands.GetChild(0).GetComponentInChildren<Animator>();
        }

        spread = currentWeapon.accuracy;
        magazine = currentWeapon.magazineSize;
        ammo = currentWeapon.magazineSize;
        reloadTime = currentWeapon.reloadTime;

        reloadBar.maxValue = reloadTime;
        
        audio = GetComponent<AudioSource>();
        audio.clip = currentWeapon.fireSound;
    }

    public void getUpgraded()
    {
        Debug.Log("Upgraded");
        spread = currentWeapon.accuracy;
        magazine = currentWeapon.magazineSize;
        reloadTime = currentWeapon.reloadTime;

        //Upgrade
        if (upgSystem.upgInUse.Count > 0)
        {
            foreach (UpgradeObject upg in upgSystem.upgInUse)
            {
                spread += upg.accuracy;
                magazine += upg.magazineSize;
                reloadTime += upg.reloadTime;
            }
        }
    }

    void Update()
    {
        cd += Time.deltaTime;

        if (reloading)
        {
            reloadCount += Time.deltaTime;

            reloadBar.value = reloadCount;

            if (reloadCount >= reloadTime)
            {
                reloadCount = 0;
                reloading = false;
                
                reloadBar.value = 0;
                ammo = magazine;
            }
        }

        if (Input.GetKey(KeyCode.Mouse0) && cd >= 1 / currentWeapon.fireRate && ammo > 0 && reloading == false)
            {
                shotAnim.Play();
                cd = 0;
                Shoot();
                ammo -= 1;

                camShake.StartShake();
            }
            else if ((Input.GetKey(KeyCode.Mouse0) && cd >= 1 / currentWeapon.fireRate && ammo <= 0) || Input.GetKey(KeyCode.R) && reloading == false)
            {
                reloading = true;
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

        float sizeMult = currentWeapon.ammoSizeMult;
        float bulletSpeed = currentWeapon.bulletSpeed;
        //Upgrade
        if (upgSystem.upgInUse.Count > 0)
        {
            foreach (UpgradeObject upg in upgSystem.upgInUse)
            {
                bullet.dmg += upg.damage;
                bullet.bounce += upg.bounce;

                bullet.critChance += upg.criticalChance;
                bullet.critMult += upg.criticalMultiplier;

                bullet.knockbackForce += upg.knockbackForce;
                bullet.pierce += upg.pierce;

                bulletSpeed += upg.bulletSpeed;
                sizeMult += upg.ammoSizeMult;
            }
        }
        
        bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.forward * bulletSpeed, ForceMode2D.Impulse);
        bullet.transform.localScale = bullet.transform.localScale * sizeMult;
    }

    void SpawnBullet()
    {
        float randomAngle = Random.Range(-(1 - spread) * 100, (1 - spread) * 100);
        Quaternion spreadRotation = transform.rotation * Quaternion.Euler(randomAngle, 0, 0);

        BulletStat(Instantiate(currentWeapon.ammoPrefab, transform.position, spreadRotation).GetComponent<Bullet>());

        anim.SetTrigger("Shoot");
    }

    void Shoot()
    {
        if (currentWeapon.weaponType == WeaponsObject.WeaponType.Pistol)
        {
            SpawnBullet();

            GetComponent<PrositionalAudio>().Play();
        }

        if (currentWeapon.weaponType == WeaponsObject.WeaponType.Shotgun)
        {
            SpawnBullet();
            SpawnBullet();
            SpawnBullet();
            SpawnBullet();

            GetComponent<PrositionalAudio>().Play();
        }

        if (currentWeapon.weaponType == WeaponsObject.WeaponType.Rifle)
        {
            SpawnBullet();

            GetComponent<PrositionalAudio>().Play();
        }
    }
}
