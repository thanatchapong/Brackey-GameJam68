using UnityEngine;

public class RandomAnimationPlayer : MonoBehaviour
{
    public Animator animator;
    public string[] animationTriggers; // List of trigger names for animations
    public float minRestTime = 5f;
    public float maxRestTime = 10f;

    private bool isPlaying = false;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        StartCoroutine(PlayAnimationsWithRest());
    }

    System.Collections.IEnumerator PlayAnimationsWithRest()
    {
        while (true)
        {
            if (!isPlaying)
            {
                // Pick a random animation trigger
                string trigger = animationTriggers[Random.Range(0, animationTriggers.Length)];
                animator.SetTrigger(trigger);
                isPlaying = true;

                // Wait for animation to finish (optional: use animation length or fixed time)
                yield return new WaitForSeconds(2f); // Adjust based on animation length
                isPlaying = false;

                // Rest period
                float restTime = Random.Range(minRestTime, maxRestTime);
                yield return new WaitForSeconds(restTime);
            }
            yield return null;
        }
    }
}
