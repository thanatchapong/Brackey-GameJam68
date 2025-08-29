using UnityEngine;
using System.Collections;

public class UpgradeSystemAudio : MonoBehaviour
{
    [SerializeField] AudioClip PerkAudio;

    [Header("Volume")]
    [SerializeField] float HighVolume;
    [SerializeField] float LowVolume;
    float currentVolume;

    [Header("Pitch")]
    [SerializeField] float HighPitch;
    [SerializeField] float LowPitch;

    [Header("Max Exp Sound")]
    [SerializeField] AudioClip mes;
    float currentPitch;

    private Coroutine fadeCoroutine;


    //Play SFX when max exp is reached
    public void PlayMaxExpSound()
    {
        AudioManager.instance.PlaySound(mes,0.5f);
    }

    //Play ticking sound effect and adjust BGM when selecting perks
    public void PlayPerkAudio(float duration = 1f)
    {
        AudioManager.instance.PlayLoop(PerkAudio);

        currentVolume = AudioManager.instance.BGM.volume;
        currentPitch = AudioManager.instance.BGM.pitch;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(MusicFade(duration, currentVolume, LowVolume, currentPitch, LowPitch));
    }

    public void StopPerkAudio(float duration = 1f)
    {
        AudioManager.instance.StopLoop();

        currentVolume = AudioManager.instance.BGM.volume;
        currentPitch = AudioManager.instance.BGM.pitch;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(MusicFade(duration, currentVolume, HighVolume, currentPitch, HighPitch));
    }

    //Decrease Volume and Pitch of background music

    private IEnumerator MusicFade(float duration, float volstart, float voltarget, float pitchstart, float pitchtarget)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.unscaledDeltaTime;
            Debug.Log(timeElapsed);
            float newVolume = Mathf.Lerp(volstart, voltarget, timeElapsed / duration);
            float newPitch = Mathf.Lerp(pitchstart, pitchtarget, timeElapsed / duration);
            AudioManager.instance.BGM.volume = newVolume;
            AudioManager.instance.BGM.pitch = newPitch;
            yield return null;
        }

        AudioManager.instance.BGM.volume = voltarget;
        AudioManager.instance.BGM.pitch = pitchtarget;
    }

}
