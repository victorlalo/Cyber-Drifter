using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed = 10f;
    public float normalTurnSpeed = 0.1f;
    public float driftTurnSpeed = 2f;
    public float airFlipSpeed = 50f;
    public float lerpSpeed = 0.5f;

    [SerializeField] GameObject carModel;
    [SerializeField] SphereCollider collider;
    [SerializeField] GameObject RaycastPoint;
    public float raycastDistance = 1f;
    [SerializeField] LayerMask groundLayer;

    [SerializeField] Vector3 colliderOffset = new Vector3();

    float floatyDrag, heavyDrag;

    Vector3 startPos;

    Rigidbody sphereRB;
    public bool isGrounded = true;
    public bool isDrifting = false;

    void Start()
    {
        sphereRB = collider.GetComponent<Rigidbody>();
        floatyDrag = sphereRB.drag;
        heavyDrag = floatyDrag / 10f;

        startPos = transform.position;
        //collider = GetComponent<SphereCollider>();
    }

    public Quaternion GetCarRotation()
    {
        return carModel.transform.rotation;
    }

    void Update() {

        if (transform.position.y < -20)
        {
            transform.rotation = Quaternion.identity;
            sphereRB.velocity = Vector3.zero;
            sphereRB.drag = floatyDrag;

            sphereRB.gameObject.transform.position = startPos;
            transform.position = startPos;



            Debug.Log("GAME OVER!");
        }

        float inputDir = Input.GetAxis("Horizontal");

        isDrifting = Input.GetButton("Jump");

        // if on ground and no drifting, can turn
        if (isGrounded && !isDrifting)
        {
            sphereRB.MovePosition(new Vector3(sphereRB.position.x + inputDir * normalTurnSpeed * Time.deltaTime, sphereRB.position.y, sphereRB.position.z));
        }

        else if (isGrounded && isDrifting)
        {
            transform.Rotate(0, inputDir * driftTurnSpeed * Time.deltaTime, 0);
        }

        // otherwise, do air spin tricks
        else if (!isGrounded)
        {
            float xRotation = Input.GetAxis("Vertical");
            transform.Rotate(xRotation * airFlipSpeed * Time.deltaTime, 0, -inputDir * airFlipSpeed * Time.deltaTime);
        }

    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(RaycastPoint.transform.position, -transform.up, out hit, raycastDistance, groundLayer);

        if (!isGrounded && sphereRB.velocity.y < 0)
        {
            sphereRB.drag = heavyDrag;
        }
        else
        {
            sphereRB.drag = floatyDrag;
        }

        sphereRB.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Acceleration);
        transform.position = collider.transform.position - colliderOffset;

    }

    //private void OnTriggerEnter(Collider other)
    //{

    //    if (other.gameObject.CompareTag("DeathZone"))
    //    {
    //        transform.rotation = Quaternion.identity;
    //        sphereRB.velocity = Vector3.zero;
    //        sphereRB.drag = floatyDrag;

    //        sphereRB.gameObject.transform.position = startPos;
    //        transform.position = startPos;

            

    //        Debug.Log("GAME OVER!");
    //    }
    //}
}
