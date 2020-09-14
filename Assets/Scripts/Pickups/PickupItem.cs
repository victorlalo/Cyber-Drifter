using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum ItemType
{
    TOKEN,
    SHIELD,
    MUILTIPLIER,
    DIVIDER,
    NITROUS,
    TIME_SLOW,
    TIME_SLUG
}

public class PickupItem : MonoBehaviour
{
    [SerializeField] ItemType itemType;
    FloatingAnimation anim;

    public static event Action<ItemType> onItemCollision;

    void Start()
    {
        anim = GetComponent<FloatingAnimation>();
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<CarController>())
        {
            //Debug.Log("GOT ITEM: " + itemType.ToString());
            transform.parent = Camera.main.transform;
            onItemCollision?.Invoke(itemType);

            // ** TO DO: send back to pool instead of destroy
            Destroy(gameObject);
            anim.SendToHUD(itemType);
        }
            
    }
}
