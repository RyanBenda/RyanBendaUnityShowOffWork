using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapHandsAnimationsHandler : MonoBehaviour
{
    HandHeldTrap _HandHeldTrapScript;
    public GameObject _HandHeldTrap;
    public GameObject _TrapModel;

    private void Awake()
    {
        _HandHeldTrapScript = _HandHeldTrap.GetComponent<HandHeldTrap>();
    }

    public void PickedUpTool()
    {
        _HandHeldTrapScript._ToolAnimator.SetBool("PickingUp", false);
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
        if (_HandHeldTrapScript._ToolAnimator.GetBool("Placing") == true && _HandHeldTrapScript._ToolAnimator.GetBool("PickingUp") == false)
            _HandHeldTrap.gameObject.SetActive(false);
    }

    public void UnequipTool()
    {
        _HandHeldTrapScript._ToolAnimator.SetBool("Unequiping", false);
        ToolManager.instance._CurrentTool = null;

        _HandHeldTrap.SetActive(false);
        if (_HandHeldTrapScript._SwappingTool != null)
        {
            _HandHeldTrapScript._SwappingTool.gameObject.SetActive(true);
            _HandHeldTrapScript._SwappingTool = null;
        }

    }
}
