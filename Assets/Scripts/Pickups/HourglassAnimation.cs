﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HourglassAnimation : MonoBehaviour
{
    float delay;

    void Start()
    {
        delay = Mathf.Sin(transform.position.z);
        StartAnimation();
    }

    void StartAnimation()
    {
        transform.DOMoveY(transform.position.y + 1f, .5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetDelay(delay);
        transform.DORotate(new Vector3(180, 0, 0), .85f, RotateMode.WorldAxisAdd).SetEase(Ease.InOutQuart).SetLoops(-1, LoopType.Yoyo).SetDelay(delay);
    }
}
