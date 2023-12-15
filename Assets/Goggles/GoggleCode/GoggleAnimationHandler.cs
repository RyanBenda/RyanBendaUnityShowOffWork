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

    //public void SwapTool()
    //{
    //    _Goggles._ToolAnimator.SetBool("Unequiping", false);
    //    transform.root.GetComponent<PlayerControl>()._CurrentTool = null;
    //    _Goggles.gameObject.SetActive(false);
    //    if (_Goggles._SwappingTool != null)
    //    {
    //        _Goggles._SwappingTool.gameObject.SetActive(true);
    //        _Goggles._SwappingTool = null;
    //    }
    //}

}
