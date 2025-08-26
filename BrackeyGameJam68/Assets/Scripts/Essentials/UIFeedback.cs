using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class UIFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] bool OnHover;
    [SerializeField] float scaleMult = 1.25f;
    [SerializeField] float scaleSped = 4f;
    [SerializeField] float rotateTilt = 3;
    [SerializeField] float rotateSped = 4f;
    private Vector3 scale;
    private Quaternion rotate;

    void Start()
    {
        scale = transform.localScale;
        rotate = transform.rotation;
    }

    void FixedUpdate()
    {
        if (OnHover)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, scale * scaleMult, Time.deltaTime * (scaleSped * 2));
            transform.rotation = Quaternion.Lerp(transform.rotation, rotate * Quaternion.Euler(0, 0, rotateTilt), Time.deltaTime * (rotateSped * 1.5f));
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, scale, Time.deltaTime * scaleSped);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotate, Time.deltaTime * rotateSped);
        }
    }

    public void OnClicked()
    {
        transform.rotation = rotate * Quaternion.Euler(0, 0, -rotateTilt);
        transform.localScale =  scale * (scaleMult/2);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHover = false;
    }
}
