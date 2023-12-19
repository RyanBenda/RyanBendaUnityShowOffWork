using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PhotoPopInEffect : MonoBehaviour
{
    [SerializeField] private Transform _OffScreenPos;
    [SerializeField] private Transform _OnScreenPos;

    [SerializeField] private Image PhotoDisplayArea;

    public bool _ActivePhoto;
    float t;

    public void MoveToOnScreen()
    {
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
            }
        }
    }

}
