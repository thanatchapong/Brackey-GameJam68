using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;

    public Transform spawnPoint;

    public float bulletSpeed = 20f;

    
    public void SpawnBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet Prefab is not assigned! Please assign a bullet prefab in the Inspector.");
            return; 
        }

        if (spawnPoint == null)
        {
            Debug.LogError("Spawn Point is not assigned! Please assign a spawn point (Transform) in the Inspector.");
            return; 
        }
        
        GameObject newBullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        
        Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
       
        if (rb != null)
        {
            rb.AddForce(spawnPoint.forward * bulletSpeed, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogWarning("Bullet prefab does not have a Rigidbody component. Bullet will not move unless you have other movement scripts attached.");
        }

        Destroy(newBullet, 5f);
    }
}