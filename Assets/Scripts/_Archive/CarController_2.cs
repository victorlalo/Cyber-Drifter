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

    public void UpdateMeshes()
    {
        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;

        rightCollider.GetWorldPose(out pos, out rot);
        rightMesh.transform.position = pos;
        rightMesh.transform.rotation = rot;

        leftCollider.GetWorldPose(out pos, out rot);
        leftMesh.transform.position = pos;
        leftMesh.transform.rotation = rot;

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
    [SerializeField] Transform centerOfMass;
    Rigidbody rb;

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
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.position;

        frontWheels.ClearBreaks();
        backWheels.ClearBreaks();

        //motorForce *= 10000000f;
    }

    void FixedUpdate()
    {
        float motorVal = Input.GetAxisRaw("Vertical") * motorForce;
        //frontWheels.ApplyAcceleration(motorVal);
        backWheels.ApplyAcceleration(motorVal);

        float steerVal = Input.GetAxisRaw("Horizontal") * turnAmount;
        frontWheels.ApplySteering(steerVal);
    }

    private void Update()
    {
        frontWheels.UpdateMeshes();
        backWheels.UpdateMeshes();
    }
}
