using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] CarController carPlayer;
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
        transform.rotation = carPlayer.GetCarRotation();
    }
}
