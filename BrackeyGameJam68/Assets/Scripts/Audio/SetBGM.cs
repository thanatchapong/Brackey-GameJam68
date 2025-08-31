using UnityEngine;
using System.Collections.Generic;

public class SetBGM : MonoBehaviour
{
    [SerializeField] List<AudioClip> bgm = new List<AudioClip>();

    void Start()
    {
        GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().StopBGM();

        int randomBGM = Random.Range(0, bgm.Count);
        GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().PlayBGM(bgm[randomBGM], true, 0.15f);
        // AudioManager.StopBGM();
        // AudioManager.PlaySound(bgm);
    }

    public void StopBGM()
    {
        GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().StopBGM();
    }
}
