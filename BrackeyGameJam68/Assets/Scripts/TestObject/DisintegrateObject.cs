using UnityEngine;
using System.Collections;

public class DisintegrateObject : MonoBehaviour
{
    public int fragmentsPerAxis = 4;       // Grid resolution
    public float scatterForce = 50f;      // Force applied to fragments
    public float fadeDuration = 0.1f;        // Fade time
    public Color fragmentColor = Color.white; // Default fragment color

    void Start()
    {
        //StartCoroutine(DisintegrateSelf());
    }

    public IEnumerator DisintegrateSelf()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("Object must have a SpriteRenderer.");
            yield break;
        }

        Vector2 size = sr.bounds.size;
        Vector2 origin = (Vector2)transform.position - size / 2;

        // Hide original object
        sr.enabled = false;

        for (int x = 0; x < fragmentsPerAxis; x++)
        {
            for (int y = 0; y < fragmentsPerAxis; y++)
            {
                Vector2 pos = origin + new Vector2(
                    size.x * x / fragmentsPerAxis,
                    size.y * y / fragmentsPerAxis
                );

                GameObject frag = new GameObject("Fragment");
                frag.transform.position = pos;
                frag.transform.localScale = size / fragmentsPerAxis;

                // Add SpriteRenderer
                SpriteRenderer fragRenderer = frag.AddComponent<SpriteRenderer>();
                fragRenderer.sprite = sr.sprite;
                fragRenderer.color = fragmentColor;
                fragRenderer.sortingOrder = sr.sortingOrder;

                // Add Collider and Rigidbody2D
                frag.AddComponent<BoxCollider2D>();
                Rigidbody2D rb = frag.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0;
                Vector2 randomDir = Random.insideUnitCircle.normalized;
                rb.AddForce(randomDir * scatterForce);

                if (frag.GetComponent<FadeAndDestroy>() == null)
                {
                    frag.AddComponent<FadeAndDestroy>();
                }

            }
        }

        //Debug.Log("Destroy!");
        Destroy(gameObject);
        yield return null;
    }

    /*public IEnumerator FadeAndDestroy(GameObject frag, SpriteRenderer rend)
    {
        Color originalColor = rend.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            rend.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(frag);
    }*/
}

