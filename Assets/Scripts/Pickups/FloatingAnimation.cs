using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloatingAnimation : MonoBehaviour
{
    float delay;

    void Start()
    {
        delay = Mathf.Sin(transform.position.z);
        StartAnimation();
    }

    void StartAnimation()
    {
        transform.DOMoveY(transform.position.y + 1, .5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetDelay(delay);
        transform.DORotate(new Vector3(0, 360, 0), .85f, RotateMode.WorldAxisAdd).SetLoops(-1).SetDelay(delay);
    }


}
