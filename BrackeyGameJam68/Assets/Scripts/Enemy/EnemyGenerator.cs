using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class EnemyGenerator : MonoBehaviour
{

  enum EnemyType
  {
    Ranged, Fast, Tanked, Strong
  }

  private int debug = 0;

  public GameObject enemyPrefab;

  private float interval = 1f, chance = 1f;
  private float chanceForRangedType = 0.1f;
  private float chanceForFastType = 0.1f;
  private float chanceForTankedType = 0.1f;
  private float chanceForStrongType = 0.1f;
  private float totalDeltaTime = 0f;

  private static int next = 0;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

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

    if (Random.value < chanceForRangedType)
    {
      enemyTypes.Add(EnemyType.Ranged);
    }

    if (Random.value < chanceForFastType)
    {
      enemyTypes.Add(EnemyType.Fast);
    }

    if (Random.value < chanceForTankedType)
    {
      enemyTypes.Add(EnemyType.Tanked);
    }

    if (Random.value < chanceForStrongType)
    {
      enemyTypes.Add(EnemyType.Strong);
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

  GameObject CreateEnemy(Vector3 position)
  {
    GameObject enemy = CreateEnemy();
    enemy.GetComponent<Transform>().position = position;
    return enemy;
  }

  GameObject CreateEnemy(SortedSet<EnemyType> enemyTypes)
  {


    GameObject enemy = CreateEnemy();

    EnemyTypeColorizer enemyTypeColorizer = enemy.GetComponent<EnemyTypeColorizer>();

    string typeString = "";

    if (enemyTypes.Contains(EnemyType.Ranged))
    {
      typeString += "R";
      enemyTypeColorizer.colors.Add(new Color(0.5f, 0f, 1f));
      // TODO: this.
    }

    if (enemyTypes.Contains(EnemyType.Fast))
    {
      typeString += "F";
      enemyTypeColorizer.colors.Add(new Color(0.75f, 1f, 0.75f));
      enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().speed *= 2f;
    }

    if (enemyTypes.Contains(EnemyType.Tanked))
    {
      typeString += "T";
      enemyTypeColorizer.colors.Add(new Color(0f, 0.5f, 0f));
      enemy.GetComponent<EnemySim_ItemDrop>().maxHealth *= 2;
    }

    if (enemyTypes.Contains(EnemyType.Strong))
    {
      typeString += "S";
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
