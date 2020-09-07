using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NitrousText : MonoBehaviour
{
    Text nitrousText;
    float nitrousAmt = 0f;

    void Start()
    {
        CarController.OnNitrousPickup += AddNitrousAmt;

        nitrousText = GetComponent<Text>();
    }

    void AddNitrousAmt(float amt)
    {
        nitrousAmt = Mathf.Clamp(nitrousAmt + amt, 0, 100);
        nitrousText.text = "Nitrous \n" + Mathf.RoundToInt(nitrousAmt).ToString() + "/100";
    }
}
