using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HotBarPos : MonoBehaviour
{
    public RectTransform _BasePos;
    public RectTransform _UpPos;
    public HotBarArrow _Arrow;

    public RectTransform _selectedBox;
    public float _increaseTo;
    public float _decreaseTo;

    public Transform _ArrowTransform;

    private void Start()
    {
        _decreaseTo = _selectedBox.transform.localScale.y;
        _ArrowTransform = _Arrow.transform;
    }
    public void TweenUp()
    {
        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(this.GetComponent<RectTransform>().DOAnchorPos(_UpPos.anchoredPosition, 0.25f));
        mySequence.Append(this.GetComponent<RectTransform>().DOShakeScale(0.25f,0.5f));

        mySequence.Play();

        _selectedBox.DOScale(_increaseTo, 0.55f);

    }

    public void TweenDown()
    {
        this.GetComponent<RectTransform>().DOAnchorPos(_BasePos.anchoredPosition, 0.5f);

        _selectedBox.DOScale(_decreaseTo, 0.55f);
    }

    public void MoveArrow(int value)
    {
        _Arrow.gameObject.SetActive(true);


        _Arrow.transform.DOScale(1, 0.25f);


        _Arrow.TweenTo(value);
    }


    public void PopArrow()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(_Arrow.transform.DOScale(0, 0.25f)).OnComplete(() => _Arrow.gameObject.SetActive(false));
        mySequence.Play();
    }

}
