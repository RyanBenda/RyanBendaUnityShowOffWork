using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ControlUITween : MonoBehaviour
{
    Vector2 _StartPos;
    RectTransform _RectTransform; 

    // Start is called before the first frame update
    void Start()
    {




        _RectTransform = GetComponent<RectTransform>();
        _StartPos = _RectTransform.localPosition;

        //this.GetComponent<RectTransform>().localScale = Vector3.zero;

        _RectTransform.localPosition = new Vector2(0, _StartPos.y - 15);

        Sequence sequence = DOTween.Sequence();


        sequence.Append(_RectTransform.DOAnchorPos(_StartPos, 0.5f));
        sequence.Join(_RectTransform.DOScale(_RectTransform.localScale.x, 0.5f));


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ShakeIt()
    {
        _RectTransform.DOShakePosition(0.25f);
    }
    void ColorPulse()
    {
        
    }
}
