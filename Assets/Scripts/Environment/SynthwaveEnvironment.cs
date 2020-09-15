using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynthwaveEnvironment : MonoBehaviour
{
    [SerializeField] float distanceFromCam = 50f;
    Camera cam;

    Vector3 updatedPos;

    void Awake()
    {
        cam = Camera.main;

        updatedPos = transform.position;
        updatedPos.z = cam.transform.position.z + distanceFromCam;

        transform.position = updatedPos;
    }


    void Update()
    {
        updatedPos = transform.position;
        updatedPos.z = cam.transform.position.z + distanceFromCam;

        transform.position = Vector3.Lerp(transform.position, updatedPos, 0.25f);
    }
}
