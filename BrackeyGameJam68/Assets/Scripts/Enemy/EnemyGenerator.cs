using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Playables;

public class EnemyGenerator : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    GameObject normalEnemyPrefab, rangedEnemyPrefab, fastEnemyPrefab;

    enum EnemyType { Ranged, Fast, Tanked, Strong }

    private List<EnemyType> possibleEnemyTypes = new List<EnemyType>();
    private static int next = 0;

    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public float chanceForTypedEnemy = 0.3f;
    public Vector2 stageAreaSize = new Vector2(20f, 20f);
    public Transform[] spawnPoints;

    [SerializeField] RoomGenerator roomGen;
    private bool waveActive = false;

    // Track active enemies
  private List<GameObject> activeEnemies = new List<GameObject>();
    private bool doorSpawnedThisWave = false; // <-- Flag
  [SerializeField] PlayableDirector levelCleared;

  [SerializeField] GameObject spawnPar;
  [SerializeField] AudioSource clearSound;

  int waveLvl = 0;

  bool isSpawning = false;

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


    void Update()
    {
        // Clean up any null references (destroyed enemies)
        if (!waveActive) return; // don't check until a wave is started

        activeEnemies.RemoveAll(e => e == null);

        if (activeEnemies.Count == 0 && !doorSpawnedThisWave && isSpawning == false)
        {
            clearSound.Play();
            levelCleared.Play();
            roomGen.SetDoorActive(true, true);
            doorSpawnedThisWave = true;
            waveActive = false; // done
        }
    }

  public void SpawnWave(int waveLevel)
  {
    isSpawning = true;
    // Reset flag for the new wave
    doorSpawnedThisWave = false;
    waveActive = true; // now wave is in progress

    waveLvl = waveLevel;

    activeEnemies.Clear();

    StartCoroutine(SpawnDelay());
    }

  IEnumerator SpawnDelay()
  {
      int enemyCount = Mathf.Clamp(Mathf.RoundToInt((waveLvl + 1) * 1.5f), 1, 100);

    for (int i = 0; i < enemyCount; i++)
    {
      Vector3 spawnPos = GetRandomSpawnPosition();
      Instantiate(spawnPar, spawnPos, Quaternion.identity);
      
      yield return new WaitForSeconds(0.4f);

      GameObject enemy = CreateRandomEnemy(spawnPos);
      if (enemy != null) activeEnemies.Add(enemy);

      yield return new WaitForSeconds(0.75f);
    }
    isSpawning = false;
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-stageAreaSize.x, stageAreaSize.x),
            Random.Range(-stageAreaSize.y, stageAreaSize.y),
            0
        );
        return transform.position + randomOffset;
    }

    GameObject CreateRandomEnemy(Vector3 position)
    {
        SortedSet<EnemyType> enemyTypes = new SortedSet<EnemyType>();

        float roll = Random.value;
        if (roll < chanceForTypedEnemy)
        {
            int index = Random.Range(0, possibleEnemyTypes.Count);
            enemyTypes.Add(possibleEnemyTypes[index]);
        }

        GameObject enemy = CreateEnemy(enemyTypes, position);

        EnemyTracker.Add(enemy); // Assuming EnemyTracker is your own system
        return enemy;
    }

    GameObject CreateEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
        next++;
        enemy.GetComponent<EnemyAI>().id = next;
        enemy.GetComponent<SpriteRenderer>().color = Color.white;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        enemy.GetComponent<EnemyAI>().target = player.transform;
        enemy.GetComponent<EnemySim_ItemDrop>().SetHP(enemy.GetComponent<EnemySim_ItemDrop>().maxHealth + (int)Mathf.Round((float)waveLvl * 1.2f));
        return enemy;
    }

    GameObject CreateEnemy(GameObject prefab)
    {
        GameObject enemy = Instantiate(prefab, transform.position, transform.rotation);
        next++;
        enemy.GetComponent<EnemyAI>().id = next;
        enemy.GetComponent<SpriteRenderer>().color = Color.white;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        enemy.GetComponent<EnemyAI>().target = player.transform;
        enemy.GetComponent<EnemySim_ItemDrop>().SetHP(enemy.GetComponent<EnemySim_ItemDrop>().maxHealth + (int)Mathf.Round((float)waveLvl * 1.2f));
        return enemy;
    }

    GameObject CreateEnemy(Vector3 position)
    {
        GameObject enemy = CreateEnemy();
        enemy.transform.position = position;
        return enemy;
    }

    GameObject CreateEnemy(SortedSet<EnemyType> enemyTypes)
    {
        GameObject enemyPrefabToUse = normalEnemyPrefab;

        if (enemyTypes.Contains(EnemyType.Ranged))
            enemyPrefabToUse = rangedEnemyPrefab;
        if (enemyTypes.Contains(EnemyType.Fast))
            enemyPrefabToUse = fastEnemyPrefab;

        GameObject enemy = CreateEnemy(enemyPrefabToUse);

        EnemyTypeColorizer enemyTypeColorizer = enemy.GetComponent<EnemyTypeColorizer>();

        if (enemyTypes.Contains(EnemyType.Fast))
            enemy.GetComponent<UnityEngine.AI.NavMeshAgent>().speed *= 2f;

        if (enemyTypes.Contains(EnemyType.Tanked))
        {
            enemyTypeColorizer.colors.Add(new Color(0f, 0.5f, 0f));
            enemy.GetComponent<EnemySim_ItemDrop>().maxHealth *= 2;
        }

        if (enemyTypes.Contains(EnemyType.Strong))
            enemyTypeColorizer.colors.Add(new Color(1f, 0f, 0f));

        return enemy;
    }

    GameObject CreateEnemy(SortedSet<EnemyType> enemyTypes, Vector3 position)
    {
        GameObject enemy = CreateEnemy(enemyTypes);
        enemy.transform.position = position;
        return enemy;
    }
}