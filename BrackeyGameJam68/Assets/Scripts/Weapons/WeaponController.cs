using SmoothShakeFree;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    [SerializeField] WeaponsObject currentWeapon;
    [SerializeField] UpgradeSystem upgSystem;
    [SerializeField] SmoothShake camShake;
    [SerializeField] SmoothShake highShake;
    [SerializeField] Transform hands;
    [SerializeField] PlayableDirector shotAnim;
    [SerializeField] Slider reloadBar;
    [SerializeField] GameObject ultimate;
    [SerializeField] Transform ammoHolder;
    List<GameObject> ammoIndicator = new List<GameObject>();
    [SerializeField] GameObject ammoArt;
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

        InitAmmoUI();
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
            hands.GetComponent<PlayableDirector>().Play();
            reloadCount += Time.deltaTime;

            reloadBar.value = reloadCount;

            if (reloadCount >= reloadTime)
            {
                hands.GetComponent<PlayableDirector>().time = 0;
                hands.GetComponent<PlayableDirector>().Stop();
                hands.GetComponent<PlayableDirector>().Evaluate();;
                reloadCount = 0;
                reloading = false;

                reloadBar.value = 0;
                ammo = magazine;
                UpdateAmmoUI();
            }
        }

        if (Input.GetKey(KeyCode.Mouse0) && cd >= 1 / currentWeapon.fireRate && ammo > 0 && reloading == false)
        {
            cd = 0;
            Shoot();
            ammo -= 1;
            UpdateAmmoUI();

            camShake.StartShake();
        }
        else if ((Input.GetKey(KeyCode.Mouse0) && cd >= 1 / currentWeapon.fireRate && ammo <= 0) || Input.GetKey(KeyCode.R) && reloading == false)
        {
            reloading = true;
        }
    }

    void InitAmmoUI()
    {
        foreach (var obj in ammoIndicator)
            Destroy(obj);
        ammoIndicator.Clear();

        for (int i = 0; i < magazine; i++)
        {
            GameObject indicator = Instantiate(ammoArt, ammoHolder);
            ammoIndicator.Add(indicator);
        }
    }

    void UpdateAmmoUI()
    {
        for (int i = 0; i < ammoIndicator.Count; i++)
        {
            ammoIndicator[i].SetActive(i < ammo);
        }
    }

    public void ShootUltimate()
    {
        highShake.StartShake();

        shotAnim.Play();

        BulletStat(Instantiate(ultimate, transform.position, transform.rotation).GetComponent<Bullet>(), true);

        anim.SetTrigger("Shoot");

        GetComponent<PrositionalAudio>().Play();
    }

    void BulletStat(Bullet bullet, bool isUlt)
    {
        float sizeMult = currentWeapon.ammoSizeMult;
        float bulletSpeed = currentWeapon.bulletSpeed;

        if (isUlt == false)
        {
            bullet.dmg = currentWeapon.baseDamage;
            bullet.bounce = currentWeapon.bounce;

            bullet.critChance = currentWeapon.criticalChance;
            bullet.critMult = currentWeapon.criticalMultiplier;

            bullet.knockbackForce = currentWeapon.knockbackForce;
            bullet.pierce = currentWeapon.pierce;

            sizeMult = currentWeapon.ammoSizeMult;
            bulletSpeed = currentWeapon.bulletSpeed;
        }

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

        BulletStat(Instantiate(currentWeapon.ammoPrefab, transform.position, spreadRotation).GetComponent<Bullet>(), false);

        anim.SetTrigger("Shoot");
    }

    void Shoot()
    {
        if (currentWeapon.weaponType == WeaponsObject.WeaponType.Pistol)
        {
            shotAnim.Play();
            SpawnBullet();

            GetComponent<PrositionalAudio>().Play();
        }

        if (currentWeapon.weaponType == WeaponsObject.WeaponType.Shotgun)
        {
            shotAnim.Play();
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
