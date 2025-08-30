using UnityEngine;
using System.Collections;
using UnityEngine.Audio;//

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Sources")]
    public AudioSource BGM;

    private AudioSource audioSource;
    private AudioSource audioSourceLoop;
    private Coroutine fadeCoroutine;
    
    [Header("Mixer Groups (ลากมาจาก AudioMixer)")]
    public AudioMixerGroup BGMGroup;
    public AudioMixerGroup SFXGroup;

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
            return;
        }

        if (BGM == null) BGM = GetComponent<AudioSource>();
        if (BGM == null) BGM = gameObject.AddComponent<AudioSource>();
        BGM.playOnAwake = false;
        BGM.loop = true;
        if (BGMGroup != null) BGM.outputAudioMixerGroup = BGMGroup;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        if (SFXGroup != null) audioSource.outputAudioMixerGroup = SFXGroup;

        audioSourceLoop = gameObject.AddComponent<AudioSource>();
        audioSourceLoop.playOnAwake = false;
        audioSourceLoop.loop = true;
        if (SFXGroup != null) audioSourceLoop.outputAudioMixerGroup = SFXGroup;
    }

    public void PlaySound(AudioClip clip, float vol = 1f)
    {
        if (clip == null) return;
        audioSource.PlayOneShot(clip, vol);
    }

    public void PlayLoop(AudioClip clip)
    {
        if (clip == null) return;
        audioSourceLoop.clip = clip;
        audioSourceLoop.Play();
    }

    public void StopLoop()
    {
        audioSourceLoop.Stop();
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;
        BGM.loop = loop;
        BGM.clip = clip;
        BGM.Play();
    }

    public void StopBGM()
    {
        BGM.Stop();
        BGM.clip = null;
    }

    public void MusicFade(float duration, float volPercentTarget, float pitchTarget)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(MusicFadePercentCoroutine(duration, volPercentTarget, pitchTarget));
    }

    //Decrease Volume and Pitch of background music

    private IEnumerator MusicFadePercentCoroutine(float duration, float volPercentTarget, float pitchTarget)
    {
        float startPercent = 100f;
        Audio_Setting setting = Audio_Setting.Instance;

        if (setting != null && setting.TryGetVolumePercent(Audio_Setting.KEY_BGM, out float p))
            startPercent = p;

        float timeElapsed = 0f;
        float startPitch = BGM.pitch;
        volPercentTarget = Mathf.Clamp(volPercentTarget, 0f, 120f);

        while (timeElapsed < duration)
        {
            timeElapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(timeElapsed / duration);

            float newPercent = Mathf.Lerp(startPercent, volPercentTarget, t);
            float newPitch = Mathf.Lerp(startPitch, pitchTarget, t);
            if (setting != null)
                setting.SetVolumePercent(Audio_Setting.KEY_BGM, newPercent, save: false);

            BGM.pitch = newPitch;

            yield return null;
        }

        if (setting != null)
            setting.SetVolumePercent(Audio_Setting.KEY_BGM, volPercentTarget, save: false);

        BGM.pitch = pitchTarget;
        fadeCoroutine = null;
    }
}
