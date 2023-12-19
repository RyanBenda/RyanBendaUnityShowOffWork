using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoggleAnimationHandler : MonoBehaviour
{
    public GoggleTool _Goggles;
    public GameObject _GoggleModel;
    public GameObject _HandsModel;

    public void BeginFade()
    {
        _Goggles.BeginFadeToBlack();
    }

    public void HideGoggle()
    {
        _GoggleModel.SetActive(false);
    }

    public void ShowGoggle()
    {
        _GoggleModel.SetActive(true);
        _HandsModel.SetActive(true);
    }
}
