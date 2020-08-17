using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed = 10f;
    public float turnSpeed = 2f;
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

        // if on ground, can turn
        if (isGrounded)
        {
            float inputDir = Input.GetAxis("Horizontal");
            transform.Rotate(0, inputDir * turnSpeed * Time.deltaTime, 0);
        }

        // otherwise, do air spin tricks
        
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

    //private void OnCollisionEnter(Collision collision)
    //{
    //    print(collision.gameObject.tag);
    //    if (collision.gameObject.CompareTag("DeathZone"))
    //    {
    //        // Fell off. Go to Game Over menu and start again.
    //        transform.position = startPos;
    //        Debug.Log("GAME OVER!");
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("DeathZone"))
        {
            transform.rotation = Quaternion.identity;
            sphereRB.velocity = Vector3.zero;
            sphereRB.drag = floatyDrag;

            sphereRB.gameObject.transform.position = startPos;
            transform.position = startPos;

            

            Debug.Log("GAME OVER!");
        }
    }
}
