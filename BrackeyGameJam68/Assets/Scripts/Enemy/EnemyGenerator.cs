using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
  private int debug = 0;

  public GameObject enemyPrefab;

  private float interval, chance, chanceForRangedType, chanceForSpeedyType, totalDeltaTime = 0;

  private static int next = 0;

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
        Create();
      }
      totalDeltaTime -= interval;
    }
  }

  // This also adds the created enemy to the tracker. Bad practice?
  void Create()
  {
    float red = 0f, blue = 0f;

    GameObject enemy;
    enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
    next++;
    enemy.GetComponent<EnemyAI>().id = next;


    float enemySpeedCoefficient = 1f;

    float rangedRoll = Random.value;
    if (rangedRoll < chanceForRangedType)
    {
      // TODO: It's similar to speed type but speed type is more easier to implement...   
      // TODO: Also might need to create a new Enemy script to manage enemy behavior all together...

      // For now give it a blue color.
      blue = 1f;

    }

    float speedyRoll = Random.value;
    if (speedyRoll < chanceForSpeedyType)
    {
      enemySpeedCoefficient = 2f;
      enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().speed *= enemySpeedCoefficient;
      red = 1f;
    }

    enemy.GetComponent<SpriteRenderer>().color = new Color(red, 1f, blue);

    GameObject player = GameObject.FindGameObjectWithTag("Player");
    enemy.GetComponent<EnemyAI>().target = player.transform;

    // TODO: Store as reference???
    EnemyTracker.Add(enemy);
  }

  public void ForceCreate(int amount)
  {
    for (int number = 0; number < amount; number++)
    {
      Create();
    }
  }
}
