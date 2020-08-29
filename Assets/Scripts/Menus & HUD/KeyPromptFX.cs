using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
            SceneManager.LoadScene(1);
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
                    Application.Quit();
        #elif UNITY_WEBGL
                    Application.OpenURL("about:blank");
        #endif
    }
}
