using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] CarController carPlayer;
    [SerializeField] float slerpTime = 0.01f;
    Camera cam;
    Vector3 originalCamPos;

    [SerializeField] float shakeAmt = 0.075f;
    [SerializeField] float shakeDuration = 0.5f;
    float shakeTimer = 0;
    bool shakeCam = false;

    //[SerializeField] Vector3 screenShakeAmt = new Vector3(.1f, .1f);
    //[SerializeField] Vector3 camOffset = new Vector3(0, 10, -50);

    void Awake()
    {
        CarController.OnCollideWithFloor += FloorCollisionScreenShake;
        cam = Camera.main;
        originalCamPos = cam.transform.localPosition;
        //transform.rotation = Quaternion.Euler(0, -15, 0);
        //carPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = carPlayer.transform.position;
        
        if (carPlayer.GetComponent<CarController>().isGrounded)
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, carPlayer.GetCarRotation().eulerAngles.y, 0), slerpTime);

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
