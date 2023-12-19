using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArmsAnimationHandler : MonoBehaviour
{
    [SerializeField] private CameraTool _Camera;
    [SerializeField] private GameObject _CameraModel;
    [SerializeField] private GameObject _HandsModel;

    public void BeginFade()
    {
        _Camera.BeginFadeToBlack();
    }

    public void HideCamera()
    {
        _CameraModel.SetActive(false);
    }

    public void ShowCamera()
    {
        _CameraModel.SetActive(true);
        _HandsModel.SetActive(true);
    }
}
