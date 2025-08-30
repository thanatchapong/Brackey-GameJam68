using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Bullet : MonoBehaviour
{
    public int dmg = 1;
    public int bounce = 0;

    public float critChance = 0.1f;
    public float critMult = 2;

    public float knockbackForce = 2.5f;
    public int pierce = 0;
    [SerializeField] ParticleSystem hitEff;

    [SerializeField] List<AudioClip> enemyHitAudio = new List<AudioClip>();
    [SerializeField] AudioClip wallHitAudio;
    [SerializeField] bool pauseImpact;
    [SerializeField] GameObject dmgIndicator;
    [SerializeField] Color critColor;

    bool crit;
    GameObject lastHitEnemy;

    public void OnTriggerEnter2D(Collider2D col)
    {
        Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
        if (rb) rb.AddForce(transform.forward * knockbackForce, ForceMode2D.Impulse);

        if (col.gameObject.tag == "Enemy")
        {
            AudioSource.PlayClipAtPoint(enemyHitAudio[Random.Range(0, enemyHitAudio.Count)], transform.position);
            pierce -= 1;

            if (hitEff) Instantiate(hitEff, transform.position, transform.rotation);

            if (pauseImpact)
            {
                if (lastHitEnemy != col.gameObject)
                {
                    StartCoroutine(PauseImpact(col.gameObject.GetComponent<EnemySim_ItemDrop>()));
                }
            }
            else
            {
                if (lastHitEnemy != col.gameObject)
                {
                    //Do Dmg
                    if ((critChance * 100) >= Random.Range(0, 101))
                    {
                        crit = true;
                        dmg = (int)Mathf.Round((float)dmg * critMult);
                    }
                    col.gameObject.GetComponent<EnemySim_ItemDrop>().TakeDamage(dmg);

                    TMP_Text dmgText = Instantiate(dmgIndicator, transform.position, Quaternion.identity).transform.GetChild(0).GetComponent<TMP_Text>();
                    dmgText.text = "-" + dmg.ToString();
                    if(crit) dmgText.color = critColor;
                }
            }

            if (pierce < 0)
            {
                Destroy(gameObject);
            }
            
            lastHitEnemy = col.gameObject;
        }
        else if (bounce <= 0)
        {
            if (col.gameObject.tag == "Metallic") AudioSource.PlayClipAtPoint(enemyHitAudio[Random.Range(0, enemyHitAudio.Count)], transform.position);
            else AudioSource.PlayClipAtPoint(wallHitAudio, transform.position);

            if (hitEff) Instantiate(hitEff, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            bounce -= 1;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        lastHitEnemy = null;
    }

    IEnumerator PauseImpact(EnemySim_ItemDrop enemyHp)
    {
        Time.timeScale = 0.5f;

        yield return new WaitForSecondsRealtime(0.1f);

        Time.timeScale = 1;
        
        //Do Dmg
        if ((critChance * 100) >= Random.Range(0, 101))
        {
            crit = true;
            dmg = (int)Mathf.Round((float)dmg * critMult);
        }
        enemyHp.TakeDamage(dmg);
        TMP_Text dmgText = Instantiate(dmgIndicator, transform.position, Quaternion.identity).transform.GetChild(0).GetComponent<TMP_Text>();
        dmgText.text = "-" + dmg.ToString();
        if(crit) dmgText.color = critColor;
    }
}
