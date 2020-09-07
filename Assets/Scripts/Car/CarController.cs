using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float speedIncrement = 0.01f;
    [SerializeField] float normalTurnSpeed = 0.1f;
    [SerializeField] float driftTurnSpeed = 2f;
    [SerializeField] float airFlipSpeed = 50f;
    [SerializeField] float lerpSpeed = 0.5f;

    public float maxTurnAmt = 15;

    [SerializeField] GameObject carModel;
    [SerializeField] SphereCollider collider;
    [SerializeField] GameObject RaycastPoint;
    public float raycastDistance = 1f;
    [SerializeField] LayerMask groundLayer;

    [SerializeField] EndlessSpawner endlessRoad;

    [SerializeField] Vector3 colliderOffset = new Vector3();

    [SerializeField] Text speedText;

    float normalDrag, airDownDrag, driftDrag;
    float normalMass, driftMass;
    Vector3 normalGravity, heavyGravity;

    Vector3 startPos;

    Rigidbody sphereRB;
    public bool isGrounded = true;
    public bool isDrifting = false;

    void Start()
    {
        sphereRB = collider.GetComponent<Rigidbody>();
        normalDrag = sphereRB.drag;
        airDownDrag = normalDrag / 20f;
        driftDrag = normalDrag * 1.5f;

        normalGravity = Physics.gravity;
        heavyGravity = normalGravity * 3f;

        normalMass = sphereRB.mass;
        driftMass = normalMass * 2f;

        startPos = transform.position;
        //collider = GetComponent<SphereCollider>();
    }

    public Quaternion GetCarRotation()
    {
        return carModel.transform.rotation;
    }

    void Update() {

        if (transform.position.y < -10 || Input.GetKeyDown(KeyCode.R))
        {
            transform.rotation = Quaternion.identity;
            sphereRB.velocity = Vector3.zero;
            sphereRB.drag = normalDrag;

            sphereRB.gameObject.transform.position = startPos;
            transform.position = startPos;

            endlessRoad.ResetRamps();

            Debug.Log("GAME OVER!");
        }

        if (isGrounded && Time.timeScale == 1)
        {
            speed += speedIncrement;
            speedText.text = speed.ToString("F2") + " km/hr";
        }

        isDrifting = Input.GetButton("Jump");
    }

    private void FixedUpdate()
    {
        float inputDir = Input.GetAxis("Horizontal");
        

        RaycastHit hit;
        isGrounded = Physics.Raycast(RaycastPoint.transform.position, -transform.up, out hit, raycastDistance, groundLayer);

        // if on ground and no drifting, can turn
        if (isGrounded)
        {
            sphereRB.drag = normalDrag;
            Physics.gravity = normalGravity;

            sphereRB.AddForce(transform.forward * speed * 100 * Time.deltaTime, ForceMode.Acceleration);
            
            //sphereRB.MovePosition(new Vector3(sphereRB.position.x + inputDir * normalTurnSpeed * Time.deltaTime, sphereRB.position.y, sphereRB.position.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0), 0.15f);

            if (isDrifting)
            {
                //sphereRB.mass = driftMass;
                sphereRB.drag = driftDrag;
                transform.Rotate(0, inputDir * driftTurnSpeed * Time.deltaTime, 0);
                //print(transform.rotation.eulerAngles.y);
                //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Mathf.Clamp(transform.rotation.eulerAngles.y, -maxTurnAmt, maxTurnAmt), transform.rotation.eulerAngles.z);
            }
            else
            {
                //sphereRB.mass = normalMass;
                sphereRB.drag = normalDrag;
                transform.Rotate(0, inputDir * normalTurnSpeed * Time.deltaTime, 0);
                Vector3 rot = transform.rotation.eulerAngles;
                if (rot.y > 180f)
                    rot.y = Mathf.Clamp(rot.y, 360 - maxTurnAmt, 360);
                else
                    rot.y = Mathf.Clamp(rot.y, 0, maxTurnAmt);

                transform.rotation = Quaternion.Euler(rot);
            }

        }

        // otherwise, do air spin tricks
        else if (!isGrounded)
        {
            float xRotation = Input.GetAxis("Vertical");
            sphereRB.AddForce(Vector3.forward * speed * 80 * Time.deltaTime, ForceMode.Acceleration);

            if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, 0.05f);
            else
                transform.Rotate(xRotation * airFlipSpeed * Time.deltaTime, 0, -inputDir * airFlipSpeed * Time.deltaTime);
            

            if (sphereRB.velocity.y < 0)
            {
                sphereRB.drag = airDownDrag;
                Physics.gravity = heavyGravity;
            }
        }


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
