using UnityEngine;

public class FadeAndDestroy : MonoBehaviour
{
    [SerializeField] float fadeDuration = 0.2f;

    private SpriteRenderer spriteRenderer;
    private float timer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = fadeDuration;
    }

    void Update()
    {
        if (spriteRenderer == null) return;

        timer -= Time.deltaTime;
        float alpha = Mathf.Clamp01(timer / fadeDuration);

        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;

        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
