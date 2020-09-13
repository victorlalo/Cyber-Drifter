using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NitrousMeter : MonoBehaviour
{
    float nitrousAmt = 0f;
    [SerializeField] GameObject nitrousPivot;

    void Start()
    {
        CarController.OnNitrousPickup += AddNitrousAmt;
        nitrousPivot.transform.localEulerAngles = new Vector3(0, 0, -110);
    }

    void AddNitrousAmt(float amt)
    { 
        nitrousAmt = Mathf.Clamp(nitrousAmt + amt, 0, 100);

        float arcVal = UtilityFunctions.Remap(nitrousAmt, 0, 100, -110, 0);
        Vector3 rot = nitrousPivot.transform.localEulerAngles;
        rot.z = arcVal;

        Ease easeType = Ease.OutSine;
        if (amt < 0)
            easeType = Ease.Unset;

        nitrousPivot.transform.DOLocalRotate(rot, 1.5f).SetEase(easeType);

        //nitrousText.text = "Nitrous \n" + Mathf.RoundToInt(nitrousAmt).ToString() + "/100";
    }
}
