using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController_3 : MonoBehaviour
{
    [Header("Wheel Objects")]
    // Wheel pairs let us pick between 2-Wheel Drive or All Wheel Drive
    public Wheel[] wheels;
    [SerializeField] Vector3 bodyWheelOffset;

    public Vector3 wheelAvPos;


    [Header("Car Body Objects")]
    // Car Model Object
    [SerializeField] GameObject carModel;
    [SerializeField] Transform centerOfMass;
    Rigidbody rb;

    // Body Collision Detection
    Collider carBodyCollider;
    //[SerializeField] Transform[] floorNormalRays;


    [Header("Movement Parameters")]
    [SerializeField] float motorForce;
    [SerializeField] float breakForce;
    [SerializeField] float normalTurnAmount;
    [SerializeField] float driftTurnAmount;
    [SerializeField] float airFlipSpeed;
    [SerializeField] float fallMultiplier;

    // State Checks
    public bool isGrounded = true;

    [SerializeField] GameObject debugCube;

    void Start()
    {
        carBodyCollider = carModel.GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.position;

        wheels = GetComponentsInChildren<Wheel>();
    }

    // Physics Updating
    void FixedUpdate()
    {
        float steerAmount = Input.GetAxis("Horizontal") * normalTurnAmount * Time.fixedDeltaTime;
        transform.Rotate(0, steerAmount , 0);

        

        foreach (var wheel in wheels)
        {
            wheel.ApplyAcceleration(motorForce);
            //wheel.ApplyRotation(steerAmount);
        }

        transform.position = GetAverageWheelPosition();
        carModel.transform.position = transform.position;



    }

    private void Update()
    {
        
        debugCube.transform.position = GetAverageWheelPosition();
    }

    private Vector3 GetAverageWheelPosition()
    {
        Vector3 averagePos = Vector3.zero;

        foreach (var wheel in wheels)
        {
            averagePos += wheel.transform.position;
        }
        averagePos /= wheels.Length;

        averagePos += bodyWheelOffset;
        return averagePos;
    }
}
