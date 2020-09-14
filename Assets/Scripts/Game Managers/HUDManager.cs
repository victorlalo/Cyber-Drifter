using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    float distance = 0;
    [SerializeField] float amtPerUpdate = 0.0025f;
    [SerializeField] Text distanceText;

    float score = 0f;
    [SerializeField] Text scoreText;
    [SerializeField] Text tokenText;
    int tokens = 0;

    float timer = 0f;
    [SerializeField] float timePerUpdate = .1f;

    public RectTransform bottomRight;
    public RectTransform bottomLeft;
    public RectTransform topRight;
    public RectTransform topLeft;

    void Start()
    {
        PickupItem.onItemCollision += HandleTokenPickup;
        CarController.OnTrickPerformed += HandleScoreChange;
    }

    void Update()
    {
        if (timer > timePerUpdate)
        {
            distance += amtPerUpdate;
            distanceText.text = distance.ToString("F2");
            timer = 0f;
        }

        else {
            timer += Time.deltaTime;
        }
    }

    void HandleTokenPickup(ItemType item)
    {
        if (item == ItemType.TOKEN)
        {
            tokens++;
            tokenText.text = "x" + tokens.ToString();
        }
    }

    void HandleScoreChange(AirTrick trick, float amt)
    {
        score += amt;
        scoreText.text = "Score: " + score.ToString("F0");

        Debug.Log("LANDED A " + trick.ToString());
    }
}
