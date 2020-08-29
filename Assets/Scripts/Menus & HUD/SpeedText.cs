using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SpeedText : MonoBehaviour
{
    Text speedText;

    private void Start()
    {
        CarController.OnSpeedChange += UpdateSpeed;
        speedText = GetComponent<Text>();
    }

    private void UpdateSpeed(float speed)
    {
        speedText.text = speed.ToString("F2") + " m";
    }
}
