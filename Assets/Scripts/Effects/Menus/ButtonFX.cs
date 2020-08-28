using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonFX : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 initScale;

    void Awake()
    {
        initScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = initScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(initScale * 1.1f, 0.2f).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(initScale, 0.2f).SetUpdate(true);
    }
}
