using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArmsAnimationHandler : MonoBehaviour
{
    public CameraTool _Camera;
    public GameObject _CameraModel;
    public GameObject _HandsModel;

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
