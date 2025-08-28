using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Exp : MonoBehaviour
{
    [SerializeField] int amount = 5;

    [SerializeField] List<AudioClip> expaudio = new List<AudioClip>();

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            AudioManager.instance.PlaySound(expaudio[Random.Range(0, expaudio.Count)],0.1f);
            GameObject.FindGameObjectWithTag("UpgradeManager").GetComponent<UpgradeSystem>().GetExp(amount);
            Destroy(gameObject);
        }       
    }
}
