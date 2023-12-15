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
                _HandHeldTripShot.GetComponent<HandHeldTripShot>().playerControl._HoldingTool = false;
            }

            _HandHeldTripShot.SetActive(false);
        }

        /*if (_HandHeldTripShot.GetComponent<HandHeldTripShot>()._ToolAnimator.GetBool("Unequiping") == true)
        {
            _HandHeldTripShot.GetComponent<HandHeldTripShot>()._ToolAnimator.SetBool("Unequiping", false);
            _HandHeldTripShot.SetActive(false);
        }*/

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
        _HandHeldTripShot.GetComponent<HandHeldTripShot>().playerControl._CurrentTool = null;

        //_HandHeldTripShot.GetComponent<HandHeldTripShot>()._ToolSlot.TweenDown();

        _HandHeldTripShot.SetActive(false);
        if (_HandHeldTripShot.GetComponent<HandHeldTripShot>()._SwappingTool != null)
        {
            _HandHeldTripShot.GetComponent<HandHeldTripShot>()._SwappingTool.gameObject.SetActive(true);
            _HandHeldTripShot.GetComponent<HandHeldTripShot>()._SwappingTool = null;
        }
        else
        {

            //_HandHeldTripShot.GetComponent<HandHeldTripShot>()._ToolSlot.PopArrow();



            //_HandHeldTripShot.GetComponent<HandHeldTripShot>()._ToolSlot._Arrow.gameObject.SetActive(false);///////////////////////////////
        }
    }

    public void DisableHands()
    {
        if (_HandHeldTripShot.GetComponent<HandHeldTripShot>()._ToolAnimator.GetBool("Placing") == true && _HandHeldTripShot.GetComponent<HandHeldTripShot>()._ToolAnimator.GetBool("PickingUp") == false)
            _HandHeldTripShot.gameObject.SetActive(false);
    }
}
