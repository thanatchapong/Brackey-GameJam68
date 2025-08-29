using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class EnemyGenerator : MonoBehaviour
{
  GameObject normalEnemyPrefab, rangedEnemyPrefab, fastEnemyPrefab;

  enum EnemyType
  {
    Ranged, Fast, Tanked, Strong
  }

  private List<EnemyType> possibleEnemyTypes = new List<EnemyType>();

  private int debug = 0;

  public GameObject enemyPrefab;

  private float interval = 1f, chance = 1f, chanceForTypedEnemy = 0.3f;
  private float totalDeltaTime = 0f;

  private static int next = 0;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    normalEnemyPrefab = Resources.Load<GameObject>("NormalEnemy");
    rangedEnemyPrefab = Resources.Load<GameObject>("RangedEnemy");
    fastEnemyPrefab = Resources.Load<GameObject>("FastEnemy");

    possibleEnemyTypes.Add(EnemyType.Ranged);
    possibleEnemyTypes.Add(EnemyType.Fast);
    possibleEnemyTypes.Add(EnemyType.Tanked);
    possibleEnemyTypes.Add(EnemyType.Strong);
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
        CreateRandomEnemy();
      }
      totalDeltaTime -= interval;
    }
  }

  // This also adds the created enemy to the tracker. Bad practice?
  void CreateRandomEnemy()
  {

    SortedSet<EnemyType> enemyTypes = new SortedSet<EnemyType>();

    float roll = Random.value;
    if (roll < chanceForTypedEnemy)
    {
      // TODO: This is an oversimplified version of the enemy type chance table.
      int index = Random.Range(0, possibleEnemyTypes.Count);
      enemyTypes.Add(possibleEnemyTypes[index]);
    }


    GameObject enemy = CreateEnemy(enemyTypes);

    // TODO: Store as reference???
    EnemyTracker.Add(enemy);
  }

  GameObject CreateEnemy()
  {
    GameObject enemy;
    enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
    next++;
    enemy.GetComponent<EnemyAI>().id = next;
    enemy.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    enemy.GetComponent<EnemyAI>().target = player.transform;
    return enemy;
  }

  GameObject CreateEnemy(GameObject enemyPrefab)
  {
    GameObject enemy;
    enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
    next++;
    enemy.GetComponent<EnemyAI>().id = next;
    enemy.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    enemy.GetComponent<EnemyAI>().target = player.transform;
    return enemy;
  }

  GameObject CreateEnemy(Vector3 position)
  {
    GameObject enemy = CreateEnemy();
    enemy.GetComponent<Transform>().position = position;
    return enemy;
  }

  GameObject CreateEnemy(SortedSet<EnemyType> enemyTypes)
  {

    GameObject enemyPrefab = normalEnemyPrefab;

    if (enemyTypes.Contains(EnemyType.Ranged))
    {
      enemyPrefab = rangedEnemyPrefab;
    }

    if (enemyTypes.Contains(EnemyType.Fast))
    {
      enemyPrefab = fastEnemyPrefab;
    }

    GameObject enemy = CreateEnemy(enemyPrefab);

    EnemyTypeColorizer enemyTypeColorizer = enemy.GetComponent<EnemyTypeColorizer>();


    if (enemyTypes.Contains(EnemyType.Fast))
    {
      enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().speed *= 2f;
    }

    if (enemyTypes.Contains(EnemyType.Tanked))
    {
      enemyTypeColorizer.colors.Add(new Color(0f, 0.5f, 0f));
      enemy.GetComponent<EnemySim_ItemDrop>().maxHealth *= 2;
    }

    if (enemyTypes.Contains(EnemyType.Strong))
    {
      enemyTypeColorizer.colors.Add(new Color(1f, 0f, 0f));
      // TODO: Add damage tag to enemy.
    }

    // enemy.GetComponent<TMP_Text>().text = typeString;

    return enemy;
  }

  GameObject CreateEnemy(SortedSet<EnemyType> enemyTypes, Vector3 position)
  {
    GameObject enemy = CreateEnemy(enemyTypes);
    enemy.GetComponent<Transform>().position = position;
    return enemy;
  }

  public void ForceCreate(int amount)
  {
    for (int number = 0; number < amount; number++)
    {
      CreateRandomEnemy();
    }
  }
}
