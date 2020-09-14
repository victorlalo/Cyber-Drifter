using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum AirTrick
{
    NONE,
    FRONT_FLIP,
    BACK_FLIP,
    BARREL_ROLL
}
public class CarController : MonoBehaviour
{
    [Header("Tunable Parameters")]
    // TUNABLE PARAMTERS
    [SerializeField] float speed = 10f;
    [SerializeField] float speedIncrement = 0.01f;
    [SerializeField] float normalTurnSpeed = 0.1f;
    [SerializeField] float driftTurnSpeed = 2f;
    [SerializeField] float airFlipSpeed = 50f;
    [SerializeField] float lerpSpeed = 0.5f;

    public float maxTurnAmt = 15;

    [Header("Air Tricks")]
    // AIR TRICK POINTS
    public float currentVertRotation = 0f;
    public float currentHorizRotation = 0f;
    int vertFlipCount = 0;
    int horizFlipCount = 0;
    float nextVertFrontThreshold = 360f;
    float nextVertBackThreshold = -360f;
    float nextHorizRightThreshold = -360f;
    float nextHorizLeftThreshold = 360f;
    [SerializeField] float airTrickPoints = 100f;
    [SerializeField] float wiggleRoomAmt = 10f;

    // PHYSICS STUFF
    float normalDrag, airDownDrag, driftDrag;
    float normalMass, driftMass;
    Vector3 normalGravity, heavyGravity;

    [Header("Models")]
    // 3D MODELS
    [SerializeField] GameObject carModel;
    [SerializeField] SphereCollider ballCollider;
    Rigidbody sphereRB;
    [SerializeField] Vector3 colliderOffset = new Vector3();
    GameObject[] wheels;

    [Header("Ground Detection")]
    [SerializeField] GameObject RaycastPoint;
    [SerializeField] LayerMask groundLayer;
    public float raycastDistance = 1f;
    
    Vector3 startPos;

    [Header("Item Parameters")]
    // ITEM PARAMETERS
    float nitrousAmt = 0f;
    [SerializeField] float nitrousIncrementAmt = 30f;
    float nitrousSpeedMultiplier = 1f;
    [SerializeField] ParticleSystem[] nitrousFX;
    [SerializeField] GameObject shield;
    bool shieldActive = false;

    bool timeSlow = false;
    public float slowTimeSpeed = .1f;

    [Header("States")]
    // STATES
    public bool isGrounded = true;
    public bool isDrifting = false;

    // EVENTS
    public static event Action<float> OnSpeedChange;
    public static event Action<AirTrick, float> OnTrickPerformed;
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

        wheels = GameObject.FindGameObjectsWithTag("Wheel");

        shield.SetActive(false);
        PickupItem.onItemCollision += HandleItemPickup;

        foreach(var nitro in nitrousFX)
        {
            nitro.Pause();
            nitro.gameObject.SetActive(false);
        }
    }

    public Quaternion GetCarRotation()
    {
        return carModel.transform.rotation;
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (timeSlow)
            {
                timeSlow = false;
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
            }
            else
            {
                timeSlow = true;
                Time.timeScale = slowTimeSpeed;
                Time.fixedDeltaTime = 0.02f * slowTimeSpeed;
            }
        }
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
            nitrousAmt -= .01f;
            nitrousSpeedMultiplier = 2.5f;
            OnNitrousPickup?.Invoke(-.1f);
            
            // activate exhaust flames
            foreach(var n in nitrousFX)
            {
                n.gameObject.SetActive(true);
                n.Play();
            }
        }
        else
        {
            nitrousSpeedMultiplier = 1f;

            // deactivate exhaust flames
            foreach (var n in nitrousFX)
            {
                n.Pause();
                n.gameObject.SetActive(false);
            }
            
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
            foreach(GameObject wheel in wheels)
            {
                wheel.transform.Rotate(speed * 50 * Time.deltaTime * nitrousSpeedMultiplier,0,0);
            }
            
            //sphereRB.MovePosition(new Vector3(sphereRB.position.x + inputDir * normalTurnSpeed * Time.deltaTime, sphereRB.position.y, sphereRB.position.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0), 0.15f);

            if (isDrifting)
            {
                //sphereRB.mass = driftMass;
                sphereRB.drag = driftDrag;
                transform.Rotate(0, inputDir * driftTurnSpeed * Time.deltaTime, 0);
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


            // Reset air spin counters if grounded
            currentVertRotation = 0;
            currentHorizRotation = 0;

            vertFlipCount = 0;
            horizFlipCount = 0;

            nextHorizLeftThreshold = 360;
            nextHorizRightThreshold = -360;
            nextVertBackThreshold = -360;
            nextVertFrontThreshold = 360;

        }

        // otherwise, do air spin tricks
        else if (!isGrounded)
        {
            float xRotation = Input.GetAxis("Vertical");
            sphereRB.AddForce(Vector3.forward * speed * nitrousSpeedMultiplier * 80 * Time.deltaTime, ForceMode.Acceleration);

            if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0)
            {
                if (currentVertRotation > 0)
                {
                    if (currentVertRotation > nextVertFrontThreshold / 2)
                        currentVertRotation = Mathf.Lerp(currentVertRotation, nextVertFrontThreshold, 0.05f);
                    else
                        currentVertRotation = Mathf.Lerp(currentVertRotation, nextVertFrontThreshold - 360, 0.05f);
                }
                else
                {
                    if (currentVertRotation < nextVertBackThreshold / 2)
                        currentVertRotation = Mathf.Lerp(currentVertRotation, nextVertBackThreshold, 0.05f);
                    else
                        currentVertRotation = Mathf.Lerp(currentVertRotation, nextVertBackThreshold + 360, 0.05f);
                }
                
                if (currentHorizRotation > 0)
                {
                    if (currentHorizRotation > nextHorizLeftThreshold / 2)
                        currentHorizRotation = Mathf.Lerp(currentHorizRotation, nextHorizLeftThreshold, 0.05f);
                    else
                        currentHorizRotation = Mathf.Lerp(currentHorizRotation, nextHorizLeftThreshold - 360, 0.05f);
                }
                else
                {
                    if (currentHorizRotation < nextHorizRightThreshold / 2)
                        currentHorizRotation = Mathf.Lerp(currentHorizRotation, nextHorizRightThreshold, 0.05f);
                    else
                        currentHorizRotation = Mathf.Lerp(currentHorizRotation, nextHorizRightThreshold + 360, 0.05f);
                }
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, 0.05f);
            }
              
            else
            {
                currentVertRotation += xRotation * airFlipSpeed * Time.fixedDeltaTime;
                currentHorizRotation += -inputDir * airFlipSpeed * Time.fixedDeltaTime;
            }
            transform.rotation = Quaternion.Euler(currentVertRotation, 0, currentHorizRotation);

            if (sphereRB.velocity.y < 0)
            {
                sphereRB.drag = airDownDrag;
                Physics.gravity = heavyGravity;
            }

            if (currentVertRotation > nextVertFrontThreshold - wiggleRoomAmt)
            {
                OnTrickPerformed?.Invoke(AirTrick.FRONT_FLIP, airTrickPoints);
                //vertFlipCount++;
                nextVertFrontThreshold += 360;
                nextVertBackThreshold = 360;
            }

            else if (currentVertRotation < nextVertBackThreshold + wiggleRoomAmt)
            {
                OnTrickPerformed?.Invoke(AirTrick.BACK_FLIP, airTrickPoints);
                //vertFlipCount--;
                
                nextVertBackThreshold -= 360;
                nextVertFrontThreshold = 360;
            }

            if (currentHorizRotation > nextHorizLeftThreshold - wiggleRoomAmt)
            {
                OnTrickPerformed?.Invoke(AirTrick.BARREL_ROLL, airTrickPoints);
                nextHorizLeftThreshold += 360;
                nextHorizRightThreshold = -360;
            }
            else if (currentHorizRotation < nextHorizRightThreshold + wiggleRoomAmt)
            {
                OnTrickPerformed?.Invoke(AirTrick.BARREL_ROLL, airTrickPoints);
                nextHorizRightThreshold -= 360;
                nextHorizLeftThreshold = 360;
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
            nitrousAmt = Mathf.Clamp(nitrousAmt + nitrousIncrementAmt, 0, 100);
            OnNitrousPickup?.Invoke(nitrousIncrementAmt);
        }
        else if (itemType == ItemType.SHIELD)
        {
            shieldActive = true;
            shield.SetActive(true);
        }
    }
}
