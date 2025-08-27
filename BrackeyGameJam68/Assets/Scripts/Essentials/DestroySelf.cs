using System.Collections;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] float timeToDes = 5f;

    void Start()
    {
        StartCoroutine(StartDestroy());
    }

    IEnumerator StartDestroy()
    {
        yield return new WaitForSeconds(timeToDes);

        Destroy(gameObject);
    }
}
