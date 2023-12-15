using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapHandsAnimationsHandler : MonoBehaviour
{
    public GameObject _HandHeldTrap;
    public GameObject _TrapModel;

    public void PickedUpTool()
    {
        _HandHeldTrap.GetComponent<HandHeldTrap>()._ToolAnimator.SetBool("PickingUp", false);
    }

    public void HideTrap()
    {
        _TrapModel.SetActive(false);
    }

    public void ShowTrap()
    {
        _TrapModel.SetActive(true);
    }

    public void DisableHands()
    {
        if (_HandHeldTrap.GetComponent<HandHeldTrap>()._ToolAnimator.GetBool("Placing") == true && _HandHeldTrap.GetComponent<HandHeldTrap>()._ToolAnimator.GetBool("PickingUp") == false)
            _HandHeldTrap.gameObject.SetActive(false);
    }

    public void UnequipTool()
    {
        _HandHeldTrap.GetComponent<HandHeldTrap>()._ToolAnimator.SetBool("Unequiping", false);
        ToolManager.instance._CurrentTool = null;

        //_HandHeldTrap.GetComponent<HandHeldTrap>()._ToolSlot.TweenDown();

        _HandHeldTrap.SetActive(false);
        if (_HandHeldTrap.GetComponent<HandHeldTrap>()._SwappingTool != null)
        {
            _HandHeldTrap.GetComponent<HandHeldTrap>()._SwappingTool.gameObject.SetActive(true);
            _HandHeldTrap.GetComponent<HandHeldTrap>()._SwappingTool = null;
        }
        else
        {

            //_HandHeldTrap.GetComponent<HandHeldTrap>()._ToolSlot.PopArrow();
        }
    }
}
