using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class UIFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] bool OnHover;
    [SerializeField] float scaleMult;
    [SerializeField] float scaleSped = 4f;
    private Vector3 scale;

    void Start()
    {
        scale = transform.localScale;
    }

    void FixedUpdate()
    {
        if(OnHover)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, scale * scaleMult, Time.deltaTime * (scaleSped * 2));
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, scale, Time.deltaTime * scaleSped);
        }
    }

    public void OnClicked()
    {
        if(OnHover)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, scale * scaleMult, Time.deltaTime * (scaleSped * 2));
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, scale, Time.deltaTime * scaleSped);
        }
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
