using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FlipText : MonoBehaviour
{
    Image flipBackground;
    Color initBGColor;
    Color fadeBGColor;

    [SerializeField] Text flipTextBox;
    Color initTextColor;
    Color fadeTextColor;

    // Start is called before the first frame update
    void Start()
    {
        flipBackground = GetComponent<Image>();
        initBGColor = flipBackground.color;
        fadeBGColor = initBGColor;
        fadeBGColor.a = 0;

        flipBackground.color = fadeBGColor;
        //flipTextBox = GetComponent<Text>();

        initTextColor = flipTextBox.color;
        fadeTextColor = initTextColor;
        fadeTextColor.a = 0;

        flipTextBox.color = fadeTextColor;

        CarController.OnTrickPerformed += ShowMessage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMessage(AirTrick trick, float amt)
    {
        flipBackground.color = initBGColor;
        flipTextBox.color = initTextColor;
        flipTextBox.text =  UtilityFunctions.trickDict[trick] + "! \n+" + amt;
        flipBackground.DOColor(fadeBGColor, 1f).SetEase(Ease.InOutQuad);
        flipTextBox.DOColor(fadeTextColor, 1f).SetEase(Ease.InOutQuad);
    }
}
