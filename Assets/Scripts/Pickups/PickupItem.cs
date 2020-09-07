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
    NITROUS
}

public class PickupItem : MonoBehaviour
{
    [SerializeField] ItemType itemType;

    public event Action<ItemType> onItemCollision;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("GOT ITEM: " + itemType.ToString());
            onItemCollision?.Invoke(itemType);
            Destroy(gameObject);
        }
            
    }
}
