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

    public CameraTool _Camera;
    public bool _Inuse;

    private void Awake()
    {
        if (_Camera == null)
            _Camera = FindObjectOfType<CameraTool>();
    }

    public void UpdateSlider()
    {
        _Slider.value = Camera.main.fieldOfView;
    }

    public void ActivateShutter()
    {
        _Animator.SetTrigger("Activate");
    }

    public void CantTakePhoto()
    {
        _Inuse = true;
    }

    public void CanTakePhoto()
    {
        _Inuse = false;
    }
}
