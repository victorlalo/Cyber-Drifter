using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Speedometer : MonoBehaviour
{
    [SerializeField] Text speedText;
    [SerializeField] GameObject arrowPivot;

    [SerializeField] float arrowMinAngle = -135f;
    [SerializeField] float arrowMaxAngle = 135f;


    private void Start()
    {
        CarController.OnSpeedChange += UpdateSpeed;
    }

    private void UpdateSpeed(float speed)
    {
        speedText.text = speed.ToString("F2");

        float arcVal = -UtilityFunctions.Remap(speed, 60, 200, arrowMinAngle, arrowMaxAngle);

        Vector3 arrowRot = arrowPivot.transform.eulerAngles;
        arrowRot.z = arcVal;
        arrowPivot.transform.eulerAngles = arrowRot;

    }
}
