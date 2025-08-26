using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{

  private int debug = 0;

  public GameObject enemyPrefab;

  private float interval, chance, chanceForRangedType, chanceForSpeedyType, totalDeltaTime = 0;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

    // 5% chance of an enemy spawning every 1s
    interval = 1f;
    chance = 1f;

    // 10% for it being ranged, 10% for it being speedy
    // ...therefore 1% for it being both ranged and speedy!
    chanceForRangedType = 0.1f;
    chanceForSpeedyType = 0.1f;

  }

  // Update is called once per frame
  // TODO: I also need to refactor this as well lol
  void Update()
  {

    totalDeltaTime += Time.deltaTime;
    while (totalDeltaTime > interval)
    {

      float roll = Random.value;
      if (roll < chance)
      {


        GameObject enemy;
        enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);


        float enemySpeedCoefficient = 1f;

        float rangedRoll = Random.value;
        if (rangedRoll < chanceForRangedType)
        {
          // TODO: It's similar to speed type but speed type is more easier to implement...   
          // TODO: Also might need to create a new Enemy script to manage enemy behavior all together...
        }

        float speedyRoll = Random.value;
        if (speedyRoll < chanceForSpeedyType)
        {
          enemySpeedCoefficient = 2f;
          enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().velocity *= enemySpeedCoefficient;
          enemy.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f);
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        enemy.GetComponent<EnemyAI>().target = player.transform;


      }
      totalDeltaTime -= interval;
    }
  }
}
