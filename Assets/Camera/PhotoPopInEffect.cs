using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PhotoPopInEffect : MonoBehaviour
{
    public Transform _OffScreenPos;
    public Transform _OnScreenPos;

    [SerializeField] private Image PhotoDisplayArea;
    //public CameraPhotosPage _PhotosPage;

    public bool _ActivePhoto;
    float t;

    private void Awake()
    {
        //if (_PhotosPage == null)
            //_PhotosPage = FindObjectOfType<CameraPhotosPage>(true);
    }

    public void MoveToOnScreen()
    {

        //PhotoDisplayArea.sprite = _PhotosPage._Photos[_PhotosPage._Photos.Count - 1];
        /*Sequence sequence = DOTween.Sequence();
        //sequence.Append(_Creature.transform.DOPath(path, _PathTravelTime));
        sequence.Append(this.transform.DOMove(_OnScreenPos.position, 0.5f)).SetEase(Ease.InSine);
        sequence.Append(this.transform.DOMove(_OnScreenPos.position - (_OnScreenPos.position - _OffScreenPos.position) / 2, 0.25f));
        sequence.Append(this.transform.DOMove(_OnScreenPos.position, 0.5f)).SetEase(Ease.InSine).SetEase(Ease.OutSine).OnComplete(() => this.transform.position = _OnScreenPos.position);
        sequence.Play();*/

        this.transform.DOMove(_OnScreenPos.position, 1f).OnComplete(() => this.transform.position = _OnScreenPos.position);

        _ActivePhoto = true;
    }

    public void MoveOffScreen(bool newPhoto)
    {
        t = 0;
        if (newPhoto)
            this.transform.DOMove(_OffScreenPos.position, 1f).OnComplete(() => MoveToOnScreen());
        else
            this.transform.DOMove(_OffScreenPos.position, 1f).OnComplete(() => this.transform.position = _OffScreenPos.position);
        _ActivePhoto = false;
        
    }

    public void SetOffScreen()
    {
        t = 0;
        _ActivePhoto = false;
        this.transform.position = _OffScreenPos.position;
    }

    private void Update()
    {
        if (_ActivePhoto)
        {
            t += Time.deltaTime;

            if (t >= 3)
            {
                MoveOffScreen(false);
                //t = 0;
            }

        }
    }

}
