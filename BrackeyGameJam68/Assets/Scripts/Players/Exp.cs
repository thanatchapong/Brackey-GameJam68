using System.Collections.Generic;
using UnityEngine;

public class Exp : MonoBehaviour
{
    [SerializeField] int amount = 5;

    [SerializeField] List<AudioClip> expAudio = new List<AudioClip>();

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            AudioManager.instance.PlaySound(expAudio[Random.Range(0, expAudio.Count)],0.1f);
            GameObject.FindGameObjectWithTag("UpgradeManager").GetComponent<UpgradeSystem>().GetExp(amount);
            Destroy(gameObject);
        }       
    }
}
