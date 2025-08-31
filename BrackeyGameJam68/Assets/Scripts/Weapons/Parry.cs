using UnityEngine;

public class Parry : MonoBehaviour
{
    [SerializeField] UpgradeSystem upgSystem;
    [SerializeField] GameObject parryHitbox;
    float cd;
    float parryCD = 1;
    

    void Update()
    {
        cd += Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse1) && cd >= Mathf.Max(parryCD, 0.33f))
        {
            DoParry();
            Rigidbody2D rb = Instantiate(parryHitbox, transform.position, transform.rotation).GetComponent<Rigidbody2D>();
            rb.AddForce(transform.forward * 25f, ForceMode2D.Impulse);

            cd = 0;
        }
    }

    void DoParry()
    {
        parryCD = 1;
        //Upgrade
        if (upgSystem.upgInUse.Count > 0)
        {
            foreach (UpgradeObject upg in upgSystem.upgInUse)
            {
                parryCD += upg.blockCooldown;
            }
        }
    }
}
