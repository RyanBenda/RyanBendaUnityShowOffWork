using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class ElecTripAnimationsHandler : MonoBehaviour
{
    HandHeldTripShot _HandHeldTripShotScript;
    public GameObject _HandHeldTripShot;
    public GameObject _TripshotModel;

    private void Awake()
    {
        _HandHeldTripShotScript = _HandHeldTripShot.GetComponent<HandHeldTripShot>();
    }

    public void SetInactive()
    {

        if (_HandHeldTripShotScript._ToolAnimator.GetBool("Placing") == true)
        {
            _HandHeldTripShotScript._ToolAnimator.SetBool("Placing", false);

            if (_HandHeldTripShotScript._Tripshot)
            {
                _HandHeldTripShotScript._Tripshot.gameObject.SetActive(true);
                _HandHeldTripShotScript._Tripshot.Equip();
                _HandHeldTripShotScript._Tripshot._HandHeldTripShot = _HandHeldTripShotScript;
                ToolManager.instance._HoldingTool = false;
            }

            _HandHeldTripShot.SetActive(false);
        }

    }

    public void PickedUpTool()
    {

        _HandHeldTripShotScript._ToolAnimator.SetBool("PickingUp", false);
    }

    public void HideTripshot()
    {
        _TripshotModel.SetActive(false);
    }

    public void ShowTripshot()
    {
        _TripshotModel.SetActive(true);
    }

    public void UnequipTool()
    {
        _HandHeldTripShotScript._ToolAnimator.SetBool("Unequiping", false);
        ToolManager.instance._CurrentTool = null;

       

        _HandHeldTripShot.SetActive(false);
        if (_HandHeldTripShotScript._SwappingTool != null)
        {
            _HandHeldTripShotScript._SwappingTool.gameObject.SetActive(true);
            _HandHeldTripShotScript._SwappingTool = null;
        }

    }

    public void DisableHands()
    {
        if (_HandHeldTripShotScript._ToolAnimator.GetBool("Placing") == true && _HandHeldTripShotScript._ToolAnimator.GetBool("PickingUp") == false)
            _HandHeldTripShot.gameObject.SetActive(false);
    }
}
