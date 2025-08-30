using UnityEngine;
using System.Collections;

public class RandomParticleTrigger : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public float minDelay = 1f;
    public float maxDelay = 5f;

    void Start()
    {
        StartCoroutine(PlayParticlesRandomly());
    }

    IEnumerator PlayParticlesRandomly()
    {
        while (true)
        {
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime);

            particleSystem.Play();
        }
    }
}

