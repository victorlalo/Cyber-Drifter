using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower_2 : MonoBehaviour
{
    GameObject car;
    [SerializeField] Camera cam;

    [SerializeField] float slerpTime = 0.01f;
    
    Vector3 originalCamPos;

    [SerializeField] float shakeAmt = 0.075f;
    [SerializeField] float shakeDuration = 0.5f;
    float shakeTimer = 0;
    bool shakeCam = false;

    //[SerializeField] Vector3 screenShakeAmt = new Vector3(.1f, .1f);

    void Awake()
    {
        //CarController.OnCollideWithFloor += FloorCollisionScreenShake;

        car = GameObject.FindGameObjectWithTag("Player");
        originalCamPos = cam.transform.localPosition;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = car.transform.position;

        if (car.GetComponent<CarController_2>().isGrounded)

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, car.transform.eulerAngles.y, 0), slerpTime);

        if (shakeCam)
        {
            if (shakeTimer < shakeDuration)
            {
                cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, originalCamPos + Random.insideUnitSphere * shakeAmt, 0.35f);
                shakeTimer += Time.deltaTime;
            }
            else
            {
                shakeTimer = 0f;
                shakeCam = false;
                cam.transform.localPosition = originalCamPos;
            }
        }


    }

    void FloorCollisionScreenShake()
    {
        shakeCam = true;
    }

    IEnumerator ScreenShake()
    {

        yield return new WaitForSeconds(0.1f);

    }
}
