using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class KeyPromptFX : MonoBehaviour
{
    [SerializeField] float textLoopSpeed = 1f;
    Text promptText;

    void Start()
    {
        promptText = GetComponent<Text>();
        promptText.DOFade(.01f, textLoopSpeed).SetLoops(-1); //, LoopType.Yoyo);
    }

    void Update()
    {
        // any key begins game, EXCEPT mouse clicks
        if (Input.anyKeyDown && !(Input.GetKeyDown(KeyCode.Mouse0)|Input.GetKeyDown(KeyCode.Mouse1)|Input.GetKeyDown(KeyCode.Mouse2)))
        {
            Debug.Log("START!");
        }
    }
}
