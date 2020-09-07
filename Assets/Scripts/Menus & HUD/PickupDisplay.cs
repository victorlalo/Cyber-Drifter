using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupDisplay : MonoBehaviour
{
    [SerializeField] Sprite precisionTimeIcon;
    [SerializeField] Sprite sluggishTimeIcon;
    [SerializeField] Sprite pointMultiplierIcon;
    [SerializeField] Sprite pointDividerIcon;

    [SerializeField] Image iconBox;

    void Start()
    {
        PickupItem.onItemCollision += HandleItemPickup;
    }

    void Update()
    {
        
    }

    void HandleItemPickup(ItemType it)
    {
        if (it == ItemType.TIME_SLOW)
        {
            iconBox.sprite = precisionTimeIcon;
            iconBox.color = new Color(1, 1, 1, 1);
        }
    }
}
