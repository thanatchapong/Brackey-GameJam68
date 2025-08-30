using UnityEngine;

public class RangedEnemyAttacking : MonoBehaviour
{
  private float totalTime = 0f;

  [SerializeField] public Transform target;
  private float distanceToFire = 3.75f;

  GameObject player, bulletEnemyPrefab;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    bulletEnemyPrefab = Resources.Load<GameObject>("BulletEnemy");
    player = GameObject.FindGameObjectWithTag("Player");
  }

  // Update is called once per frame
  void Update()
  {

    float squaredDistanceFromTarget = (transform.position.x - player.transform.position.x) * (transform.position.x - player.transform.position.x) + (transform.position.y - player.transform.position.y) * (transform.position.y - player.transform.position.y);
    if (squaredDistanceFromTarget < distanceToFire * distanceToFire)
    {
      // fire bullets because the enemy is close enough to the player.
      totalTime += Time.deltaTime;
      if (totalTime >= 0.25f)
      {
        GameObject bulletEnemy = Instantiate(bulletEnemyPrefab, transform.position, transform.rotation);
        bulletEnemy.GetComponent<BulletEnemyNavigation>().target = player.transform;
        totalTime -= 0.25f;
      }
    }
  }
}

