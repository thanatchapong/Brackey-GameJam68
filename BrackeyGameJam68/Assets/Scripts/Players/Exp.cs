using UnityEngine;

public class Exp : MonoBehaviour
{
    [SerializeField] int amount = 5;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("UpgradeManager").GetComponent<UpgradeSystem>().GetExp(amount);
            Destroy(gameObject);
        }       
    }
}
