using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TitleFX : MonoBehaviour
{
    [SerializeField] Text titleText;
    [SerializeField] Color startColor;
    [SerializeField] Color endColor;

    [SerializeField] float transitionDuration = 1f;

    void Start()
    {
        titleText.color = startColor;
        titleText.DOColor(endColor, transitionDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }

    void Update()
    {
        
    }
}
