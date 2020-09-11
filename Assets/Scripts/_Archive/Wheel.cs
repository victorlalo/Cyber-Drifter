using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    Rigidbody rb;
    SphereCollider collidr;
    GameObject mesh;

    [SerializeField] bool canAccelerate;
    [SerializeField] bool canSteer;
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] float damper = .5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collidr = GetComponentInChildren<SphereCollider>();
        mesh = GetComponentInChildren<MeshRenderer>().gameObject;
    }

    public void ApplyAcceleration(float motorForce)
    {
        rb.AddForce(transform.forward * motorForce, ForceMode.Acceleration);
        //transform.position += transform.forward * motorForce;
        
    }

    public void ApplyRotation(float steerAmt)
    {
        Vector3 rotation = mesh.transform.eulerAngles;
        //rotation.x += Time.deltaTime * rotationSpeed;
        rotation.z = 0;

        if (canSteer)
        {
            rotation.y = steerAmt * damper;
        }
        else
        {
            rotation.y = 0;
        }

        mesh.transform.eulerAngles = rotation;
    }
}
