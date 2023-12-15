using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HotBarArrow : MonoBehaviour
{
    public List<RectTransform> _ArrowSpots = new List<RectTransform>();

    public void TweenTo(int value)
    {
        if (value >= 0 && value <= _ArrowSpots.Count - 1)
        {
            this.GetComponent<RectTransform>().DOAnchorPos(_ArrowSpots[value].anchoredPosition, 0.5f);
        }
    }

    
}
