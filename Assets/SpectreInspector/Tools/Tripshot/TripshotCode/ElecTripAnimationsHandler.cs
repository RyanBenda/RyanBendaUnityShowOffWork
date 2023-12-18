using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class ElecTripAnimationsHandler : MonoBehaviour
{

    public GameObject _HandHeldTripShot;
    public GameObject _TripshotModel;

    public void SetInactive()
    {

        if (_HandHeldTripShot.GetComponent<HandHeldTripShot>()._ToolAnimator.GetBool("Placing") == true)
        {
            _HandHeldTripShot.GetComponent<HandHeldTripShot>()._ToolAnimator.SetBool("Placing", false);

            if (_HandHeldTripShot.GetComponent<HandHeldTripShot>()._Tripshot)
            {
                _HandHeldTripShot.GetComponent<HandHeldTripShot>()._Tripshot.gameObject.SetActive(true);
                _HandHeldTripShot.GetComponent<HandHeldTripShot>()._Tripshot.Equip();
                _HandHeldTripShot.GetComponent<HandHeldTripShot>()._Tripshot._HandHeldTripShot = _HandHeldTripShot.GetComponent<HandHeldTripShot>();
                ToolManager.instance._HoldingTool = false;
            }

            _HandHeldTripShot.SetActive(false);
        }

    }

    public void PickedUpTool()
    {

        _HandHeldTripShot.GetComponent<HandHeldTripShot>()._ToolAnimator.SetBool("PickingUp", false);
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
        _HandHeldTripShot.GetComponent<HandHeldTripShot>()._ToolAnimator.SetBool("Unequiping", false);
        ToolManager.instance._CurrentTool = null;

       

        _HandHeldTripShot.SetActive(false);
        if (_HandHeldTripShot.GetComponent<HandHeldTripShot>()._SwappingTool != null)
        {
            _HandHeldTripShot.GetComponent<HandHeldTripShot>()._SwappingTool.gameObject.SetActive(true);
            _HandHeldTripShot.GetComponent<HandHeldTripShot>()._SwappingTool = null;
        }

    }

    public void DisableHands()
    {
        if (_HandHeldTripShot.GetComponent<HandHeldTripShot>()._ToolAnimator.GetBool("Placing") == true && _HandHeldTripShot.GetComponent<HandHeldTripShot>()._ToolAnimator.GetBool("PickingUp") == false)
            _HandHeldTripShot.gameObject.SetActive(false);
    }
}
