using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonFX : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 initScale;

    void Start()
    {
        initScale = transform.localScale;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(initScale * 1.1f, 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(initScale, 0.2f);
    }
}
