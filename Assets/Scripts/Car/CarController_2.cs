using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WheelPair
{
    public GameObject rightMesh;
    public GameObject leftMesh;
    public WheelCollider rightCollider;
    public WheelCollider leftCollider;

    [SerializeField] bool canAccelerate;
    [SerializeField] bool canSteer;

    public void ApplyAcceleration(float motorForce)
    {
        if (canAccelerate)
        {
            ClearBreaks();
            rightCollider.motorTorque = motorForce;
            leftCollider.motorTorque = motorForce;
        }
    }

    public void ApplyBreaks(float breakForce)
    {
        rightCollider.brakeTorque = breakForce;
        leftCollider.brakeTorque = breakForce;
    }

    public void ApplySteering(float steerAmt)
    {
        if (canSteer)
        {
            rightCollider.steerAngle = steerAmt;
            leftCollider.steerAngle = steerAmt;
        }
    }

    public void ClearBreaks()
    {
        rightCollider.brakeTorque = 0;
        leftCollider.brakeTorque = 0;
    }
}


public class CarController_2 : MonoBehaviour
{
    [Header("Wheel Objects")]
    // Wheel pairs let us pick between 2-Wheel Drive or All Wheel Drive
    [SerializeField] WheelPair frontWheels;
    [SerializeField] WheelPair backWheels;


    [Header("Car Body Objects")]
    // Car Model Object
    [SerializeField] GameObject carModel;

    // Body Collision Detection
    Collider carBodyCollider;
    //[SerializeField] Transform[] floorNormalRays;


    [Header("Movement Parameters")]
    [SerializeField] float motorForce;
    [SerializeField] float breakForce;
    [SerializeField] float turnAmount;
    [SerializeField] float airFlipSpeed;
    [SerializeField] float fallMultiplier;

    // State Checks
    public bool isGrounded = true;



    void Start()
    {
        carBodyCollider = carModel.GetComponent<Collider>();

        frontWheels.ClearBreaks();
        backWheels.ClearBreaks();

        //motorForce *= 10000000f;
    }

    void FixedUpdate()
    {
        frontWheels.ApplyAcceleration(motorForce);
        backWheels.ApplyAcceleration(motorForce);

        float steerVal = Input.GetAxis("Horizontal") * turnAmount;
        frontWheels.ApplySteering(steerVal);
    }
}
