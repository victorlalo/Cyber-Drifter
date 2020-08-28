﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    float score = 0;
    [SerializeField] float amtPerUpdate = 0.0025f;
    [SerializeField] Text scoreText;
    [SerializeField] Text bgScoreText;

    float timer = 0f;
    [SerializeField] float timePerUpdate = .1f;

    void Start()
    {
        
    }

    void Update()
    {
        if (timer > timePerUpdate)
        {
            score += amtPerUpdate;
            scoreText.text = score.ToString("F2") + " m";
            bgScoreText.text = scoreText.text;

            timer = 0f;
        }

        else {
            timer += Time.deltaTime;
        }
        

        
    }
}