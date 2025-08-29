using UnityEngine;

public class GradientScroller : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float scrollSpeed = 2f;
    public float interval = 2f;
    public float duration = 2f;
    public Vector2 scrollDirection = Vector2.right;
    public float fadeDuration = 1f;  

    private Vector3 startPos;
    private bool isScrolling = false;
    private float timer = 0f;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!isScrolling && timer >= interval)
        {
            isScrolling = true;
            timer = 0f;
            StartCoroutine(ScrollGradient());
        }
    }

    System.Collections.IEnumerator ScrollGradient()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Move the object
            transform.localPosition += (Vector3)(scrollDirection * scrollSpeed * Time.deltaTime);

            // Calculate fade based on progress
            float t = elapsed / duration;
            float alpha = Mathf.Sin(t * Mathf.PI); // Smooth fade in/out using sine wave

            // Apply alpha to sprite
            Color c = spriteRenderer.color;
            c.a = alpha;
            spriteRenderer.color = c;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset position and alpha
        transform.localPosition = startPos;
        Color resetColor = spriteRenderer.color;
        resetColor.a = 0f;
        spriteRenderer.color = resetColor;

        isScrolling = false;
    }
}


