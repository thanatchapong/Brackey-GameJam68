using UnityEngine;
using System.Collections.Generic;

public class EnemySoundScript : MonoBehaviour
{
    [SerializeField] List<AudioClip> DeathAudio = new List<AudioClip>();

    AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayDeathSound()
    {
        AudioSource.PlayClipAtPoint(DeathAudio[Random.Range(0, DeathAudio.Count)],transform.position);
    }
}
