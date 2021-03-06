﻿using UnityEngine;
using System;

public class EndGate : MonoBehaviour
{
    public static event Action OnGatePassed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CarController>())
        {
            transform.parent.GetComponent<RoadChunk>().ClearPlatform();
            OnGatePassed?.Invoke();
        }
    }
}
