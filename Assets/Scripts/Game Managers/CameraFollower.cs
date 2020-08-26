using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] CarController carPlayer;
    [SerializeField] float slerpTime = 0.01f;
    //[SerializeField] Vector3 camOffset = new Vector3(0, 10, -50);

    void Awake()
    {
        //transform.rotation = Quaternion.Euler(0, -15, 0);
        //carPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = carPlayer.transform.position;
        
        if (carPlayer.GetComponent<CarController>().isGrounded)
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, carPlayer.GetCarRotation().eulerAngles.y, 0), slerpTime);

    }
}
