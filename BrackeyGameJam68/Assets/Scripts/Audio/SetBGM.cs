using UnityEngine;

public class SetBGM : MonoBehaviour
{
    [SerializeField] AudioClip bgm;

    void Start()
    {
        GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().StopBGM();
        GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().PlayBGM(bgm, true, 0.15f);
        // AudioManager.StopBGM();
        // AudioManager.PlaySound(bgm);
    }

    public void StopBGM()
    {
        GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().StopBGM();
    }
}
