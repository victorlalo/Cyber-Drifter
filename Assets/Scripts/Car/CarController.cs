using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CarController : MonoBehaviour
{
    // TUNABLE PARAMTERS
    [SerializeField] float speed = 10f;
    [SerializeField] float speedIncrement = 0.01f;
    [SerializeField] float normalTurnSpeed = 0.1f;
    [SerializeField] float driftTurnSpeed = 2f;
    [SerializeField] float airFlipSpeed = 50f;
    [SerializeField] float lerpSpeed = 0.5f;

    public float maxTurnAmt = 15;

    // PHYSICS STUFF
    float normalDrag, airDownDrag, driftDrag;
    float normalMass, driftMass;
    Vector3 normalGravity, heavyGravity;


    // 3D MODELS
    [SerializeField] GameObject carModel;
    [SerializeField] SphereCollider ballCollider;
    Rigidbody sphereRB;
    [SerializeField] Vector3 colliderOffset = new Vector3();


    [SerializeField] GameObject RaycastPoint;
    public float raycastDistance = 1f;
    [SerializeField] LayerMask groundLayer; 
    

    Vector3 startPos;


    // ITEM PARAMETERS
    float nitrousAmt = 0f;
    float nitrousSpeedMultiplier = 1f;
    [SerializeField] GameObject shield;
    bool shieldActive = false;

    // STATES
    public bool isGrounded = true;
    public bool isDrifting = false;

    // EVENTS
    public static event Action<float> OnSpeedChange;
    public static event Action<float> OnTrickPerformed;
    public static event Action<float> OnAddPoints;
    public static event Action OnCollideWithFloor;
    public static event Action OnGameOver;

    public static event Action<float> OnNitrousPickup;

    void Start()
    {
        sphereRB = ballCollider.GetComponent<Rigidbody>();
        normalDrag = sphereRB.drag;
        airDownDrag = normalDrag / 20f;
        driftDrag = normalDrag * 1.5f;

        normalGravity = Physics.gravity;
        heavyGravity = normalGravity * 3f;

        normalMass = sphereRB.mass;
        driftMass = normalMass * 2f;

        startPos = transform.position;
        //collider = GetComponent<SphereCollider>();

        shield.SetActive(false);
        PickupItem.onItemCollision += HandleItemPickup;
    }

    public Quaternion GetCarRotation()
    {
        return carModel.transform.rotation;
    }

    void Update() 
    {

        if (transform.position.y < -10 || Input.GetKeyDown(KeyCode.R))
        {
            transform.rotation = Quaternion.identity;
            sphereRB.velocity = Vector3.zero;
            sphereRB.drag = normalDrag;

            sphereRB.gameObject.transform.position = startPos;
            transform.position = startPos;

            OnGameOver?.Invoke();
            Debug.Log("GAME OVER!");
        }

        if (isGrounded && Time.timeScale == 1)
        {
            speed += speedIncrement;
            OnSpeedChange?.Invoke(speed);
            //speedText.text = speed.ToString("F2") + "\nkm/hr";
        }

        isDrifting = Input.GetButton("Jump");

        if (Input.GetKey(KeyCode.X) && nitrousAmt > 0f)
        {
            nitrousAmt -= .1f;
            nitrousSpeedMultiplier = 2.5f;
            OnNitrousPickup?.Invoke(-.1f);
            
            // activate exhaust flames
        }
        else
        {
            nitrousSpeedMultiplier = 1f;

            // deactivate exhaust flames
        }
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

            sphereRB.AddForce(transform.forward * speed * 100 * nitrousSpeedMultiplier * Time.deltaTime, ForceMode.Acceleration);
            
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
            sphereRB.AddForce(Vector3.forward * speed * nitrousSpeedMultiplier * 80 * Time.deltaTime, ForceMode.Acceleration);

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


        transform.position = ballCollider.transform.position - colliderOffset;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (Time.timeSinceLevelLoad < 0.5f)
        {
            return;
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            OnCollideWithFloor?.Invoke();
        }
    }

    void HandleItemPickup(ItemType itemType)
    {
        if (itemType == ItemType.NITROUS)
        {
            nitrousAmt = Mathf.Clamp(nitrousAmt + 10f, 0, 100);
            OnNitrousPickup?.Invoke(10f);
        }
        else if (itemType == ItemType.SHIELD)
        {
            shieldActive = true;
            shield.SetActive(true);
        }
    }
}
