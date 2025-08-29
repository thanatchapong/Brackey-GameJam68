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


    //Play SFX when max exp is reached
    public void PlayMaxExpSound()
    {
        AudioManager.instance.PlaySound(mes,0.5f);
    }

    //Play ticking sound effect and adjust BGM when selecting perks
    public void PlayPerkAudio(float duration = 1f)
    {
        AudioManager.instance.PlayLoop(PerkAudio);

        AudioManager.instance.MusicFade(duration, LowVolume, LowPitch);
    }

    public void StopPerkAudio(float duration = 1f)
    {
        AudioManager.instance.StopLoop();

        AudioManager.instance.MusicFade(duration, HighVolume, HighPitch);
    }
}
