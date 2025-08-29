using UnityEngine;

public class DynamicUI : MonoBehaviour
{
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private Vector2 offset;
    [SerializeField] private float maxDistance = 100f;

    private RectTransform rectTransform;
    private Vector2 startPos;
    private Vector2 targetPos;
    private Camera uiCamera;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.localPosition;

        // get camera if canvas uses one
        Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
        uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
    }

    void Update()
    {
        if (rectTransform.parent == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            Input.mousePosition,
            uiCamera,
            out targetPos
        );

        targetPos += offset;

        Vector2 direction = targetPos - startPos;
        if (direction.magnitude > maxDistance)
            targetPos = startPos + direction.normalized * maxDistance;

        rectTransform.localPosition = Vector2.Lerp(
            rectTransform.localPosition,
            targetPos,
            Time.deltaTime * followSpeed
        );
    }
}