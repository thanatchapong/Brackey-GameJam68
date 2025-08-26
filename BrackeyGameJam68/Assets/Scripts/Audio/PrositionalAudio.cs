using UnityEngine;

public class PrositionalAudio : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip clip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = clip;
        audioSource.spatialBlend = 1.0f; // 3D sound
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 20f;
        audioSource.loop = false;
    }

    public void Play()
    {
        audioSource.Play();
    }
}
