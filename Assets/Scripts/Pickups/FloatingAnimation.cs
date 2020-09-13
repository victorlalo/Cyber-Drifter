using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloatingAnimation : MonoBehaviour
{
    float delay;

    void Start()
    {
        delay = Mathf.Sin(transform.position.z);
        StartAnimation();
    }

    void StartAnimation()
    {
        transform.DOMoveY(transform.position.y + 1, .5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetDelay(delay);
        transform.DORotate(new Vector3(0, 360, 0), .85f, RotateMode.WorldAxisAdd).SetLoops(-1).SetDelay(delay);
    }

    public void SendToHUD(ItemType type)
    {
        if (type == ItemType.TOKEN)
        {
            // send to score box (bottom left)
            Bounds camBounds = UtilityFunctions.OrthographicBounds(Camera.main);
            transform.DOMove(new Vector3(transform.position.x, transform.position.y + 3, transform.position.z + 10f), .4f);
            transform.DOScale(.25f, .25f);
            transform.DOMove(new Vector3(camBounds.min.x, camBounds.min.y, transform.position.z + 25), .5f).SetEase(Ease.InOutElastic)
            .OnComplete(SelfDestruct);

        }

        else if (type == ItemType.NITROUS)
        {
            // send to speedometer (bottom right)
            Bounds camBounds = UtilityFunctions.OrthographicBounds(Camera.main);
            transform.DOMove(new Vector3(camBounds.max.x, camBounds.min.y, transform.position.z + 25), .5f).SetEase(Ease.InOutElastic).OnComplete(SelfDestruct);
        }

        else if (type == ItemType.DIVIDER || type == ItemType.MUILTIPLIER || type == ItemType.TIME_SLOW || type == ItemType.TIME_SLUG)
        {
            // send to item box
            SelfDestruct();
        }

        else
        {
            SelfDestruct();
        }
    }

    void SelfDestruct()
    {
        Destroy(gameObject);
    }


}
