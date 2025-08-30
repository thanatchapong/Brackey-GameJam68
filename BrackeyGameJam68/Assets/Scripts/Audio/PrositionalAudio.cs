using UnityEngine;

public class PrositionalAudio : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip clip;

    [SerializeField] bool PlayOnStart = false;
    [SerializeField] bool loop = false;
    [SerializeField] float volume = 1f;

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
        audioSource.volume = volume;
        audioSource.loop = loop;

        if (PlayOnStart)
        {
            audioSource.Play();
        }
    }

    public void Play()
    {
        audioSource.Play();
    }
}
