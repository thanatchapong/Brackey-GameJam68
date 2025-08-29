using UnityEngine;

public class DynamicUI : MonoBehaviour
{
    [SerializeField] private float followSpeed = 5f;   // Higher = snappier
    [SerializeField] private Vector2 offset;           // Optional offset
    [SerializeField] private float maxDistance = 100f; // Max distance from start

    private RectTransform rectTransform;
    private Vector2 startPos;
    private Vector2 targetPos;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.localPosition; // Save initial position
    }

    void Update()
    {
        // Get mouse position in screen space → convert to local pos
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            Input.mousePosition,
            null, // works for ScreenSpaceOverlay automatically
            out targetPos
        );

        // Apply offset
        targetPos += offset;

        // Clamp movement within a circle radius
        Vector2 direction = targetPos - startPos;
        if (direction.magnitude > maxDistance)
            targetPos = startPos + direction.normalized * maxDistance;

        // Smoothly move towards target
        rectTransform.localPosition = Vector2.Lerp(
            rectTransform.localPosition,
            targetPos,
            Time.deltaTime * followSpeed
        );
    }
}