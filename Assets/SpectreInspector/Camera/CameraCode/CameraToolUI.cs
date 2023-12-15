using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;

public class CameraToolUI : MonoBehaviour
{
    public Slider _Slider;

    public Animator _Animator;

    public GameObject _TopShutter;
    public GameObject _BottomShutter;

    public TrackerTool _Camera;
    public bool _Inuse;

    private void Awake()
    {
        if (_Camera == null)
            _Camera = FindObjectOfType<TrackerTool>();
    }

    // Start is called before the first frame update

    private void OnEnable()
    {

        if (_TopShutter == null && _BottomShutter == null)
        {
            Debug.Log("NO SHUTTER");
        }
        else
        {
            Debug.Log("SHUTTER FOUND");
        }



    }

    private void OnDisable()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.M))
        //{

        //    if (_TopShutter == null && _BottomShutter == null)
        //    {
        //        Debug.Log("NO SHUTTER");
        //    }
        //    else
        //    {
        //        Debug.Log("SHUTTER FOUND");
        //        ActivateShutter();
        //    }
        //}
    }
    public void UpdateSlider()
    {
        _Slider.value = Camera.main.fieldOfView;


    }

    public void ActivateShutter()
    {
        _Animator.SetTrigger("Activate");
        //_Animator.ResetTrigger("Activate");

        //Debug.Log("BEING CALLED!");
        //Sequence sequenceTop = DOTween.Sequence();
        //sequenceTop.Append(_TopShutter.GetComponent<RectTransform>().DOAnchorPosY(10f, 1).SetUpdate(true));
        //sequenceTop.Append(_TopShutter.GetComponent<RectTransform>().DOAnchorPosY(-10f, 1).SetUpdate(true));
        ////sequenceTop.Append(_TopShutter.GetComponent<RectTransform>().DOAnchorPosY(+10, 1));
        //sequenceTop.Play();

        //Sequence sequenceBottom = DOTween.Sequence();
        //sequenceBottom.Append(_BottomShutter.GetComponent<RectTransform>().DOAnchorPosY(-10F, 1).SetUpdate(true));
        //sequenceBottom.Append(_BottomShutter.GetComponent<RectTransform>().DOAnchorPosY(10f, 1).SetUpdate(true));
        //sequenceBottom.Play();
    }

    public void CantTakePhoto()
    {
        //_Camera.TakePhoto();
        _Inuse = true;
    }

    public void CanTakePhoto()
    {
        _Inuse = false;

    }
}
