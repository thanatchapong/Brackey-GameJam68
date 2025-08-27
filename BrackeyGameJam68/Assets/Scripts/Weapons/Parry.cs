using UnityEngine;

public class Parry : MonoBehaviour
{
    [SerializeField] GameObject parryHitbox;
    float cd;

    void Update()
    {
        cd += Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse1) && cd >= 1)
        {
            Rigidbody2D rb = Instantiate(parryHitbox, transform.position, transform.rotation).GetComponent<Rigidbody2D>();
            rb.AddForce(transform.forward * 25f, ForceMode2D.Impulse);

            cd = 0;
        }
    }
}
