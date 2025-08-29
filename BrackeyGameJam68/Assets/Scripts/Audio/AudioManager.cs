using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource BGM;

    private AudioSource audioSource;

    private AudioSource audioSourceLoop;

    private Coroutine fadeCoroutine;

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


    public void MusicFade(float duration, float volstart, float voltarget, float pitchstart, float pitchtarget)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(MusicFadeCoroutine(duration, volstart, voltarget, pitchstart, pitchtarget));
    }


    //Decrease Volume and Pitch of background music

    private IEnumerator MusicFadeCoroutine(float duration, float volstart, float voltarget, float pitchstart, float pitchtarget)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.unscaledDeltaTime;
            float newVolume = Mathf.Lerp(volstart, voltarget, timeElapsed / duration);
            float newPitch = Mathf.Lerp(pitchstart, pitchtarget, timeElapsed / duration);
            BGM.volume = newVolume;
            BGM.pitch = newPitch;
            yield return null;
        }

        BGM.volume = voltarget;
        BGM.pitch = pitchtarget;
    }

}
