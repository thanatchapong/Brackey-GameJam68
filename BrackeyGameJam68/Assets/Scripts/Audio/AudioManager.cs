using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource BGM;

    private AudioSource audioSource;

    private AudioSource audioSourceLoop;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        BGM = gameObject.GetComponent<AudioSource>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSourceLoop = gameObject.AddComponent<AudioSource>();

        audioSourceLoop.loop = true;

    }

    public void PlaySound(AudioClip clip, float vol = 1f)
    {
        if (clip != null)
        {
            audioSource.volume = vol;
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayLoop(AudioClip clip)
    {
        if (clip != null)
        {
            audioSourceLoop.clip = clip;
            audioSourceLoop.Play();
        }
    }

    public void StopLoop()
    {
        audioSourceLoop.Stop();
    }

}
