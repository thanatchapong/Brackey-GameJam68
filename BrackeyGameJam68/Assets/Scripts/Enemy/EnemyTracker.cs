using UnityEngine;
using System.Collections.Generic;

public sealed class EnemyTracker : MonoBehaviour
{

  public static Dictionary<int, GameObject> enemies = new Dictionary<int, GameObject>();



  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  // TODO: Store as reference???
  public static void Add(GameObject enemy)
  {
    int key = enemy.GetComponent<EnemyAI>().id;
    enemies.Add(key, enemy);
  }

  public static void RemoveByKey(int key)
  {
    enemies.Remove(key);
  }

  public static void RemoveAll()
  {
    enemies.Clear();
  }

  public static int GetAmount()
  {
    return enemies.Count;
  }

  public static bool HasRemaining()
  {
    return GetAmount() <= 0;
  }
}
