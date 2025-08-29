using UnityEngine;
using System.Collections;

public class ForceToward : MonoBehaviour
{
    [SerializeField] float forceStrength = 5f; // How strong the force is

    Rigidbody2D rb;
    Transform player;
    bool startFollow;

    IEnumerator Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.transform;

        yield return new WaitForSeconds(0.3f);

        startFollow = true;
    }

    void Update()
    {
        if (startFollow)
        {
            if (player != null && Vector3.Distance(player.position, transform.position) <= 3)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                rb.linearVelocity = direction * forceStrength;
            }
        }
    }
}