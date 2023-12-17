using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarComponent : MonoBehaviour
{

    [SerializeField] private GameObject _ElecTripSlot;
    [SerializeField] private GameObject _TrapSlot;
    [SerializeField] private GameObject _GoggleSlot;
    [SerializeField] private GameObject _CameraSlot;

    [SerializeField] private PhysicsPlayerController _PC;
    private PlayerInput _PlayerInput;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        if (_PC == null)
        {
            _PC = FindObjectOfType<PhysicsPlayerController>();
        }

        _PlayerInput = _PC._PlayerInput;

        _PlayerInput.actions["EquipGravPolt"].performed += OnToolSwitchElec; 
        _PlayerInput.actions["EquipGravPolt"].Enable();

        _PlayerInput.actions["EquipTrap"].performed += OnToolSwitchTrap;
        _PlayerInput.actions["EquipTrap"].Enable();

        _PlayerInput.actions["EquipGoggles"].performed += OnToolGoggle; 
        _PlayerInput.actions["EquipGoggles"].Enable();

        _PlayerInput.actions["EquipCamera"].performed += OnToolCamera;
        _PlayerInput.actions["EquipCamera"].Enable();
    }


    private void OnDisable()
    {
        _PlayerInput.actions["EquipGravPolt"].performed -= OnToolSwitchElec; 
        _PlayerInput.actions["EquipGravPolt"].Disable();

        _PlayerInput.actions["EquipTrap"].performed -= OnToolSwitchTrap; 
        _PlayerInput.actions["EquipTrap"].Disable();

        _PlayerInput.actions["EquipGoggles"].performed -= OnToolGoggle; 
        _PlayerInput.actions["EquipGoggles"].Disable();

        _PlayerInput.actions["EquipCamera"].performed -= OnToolCamera; 
        _PlayerInput.actions["EquipCamera"].Disable();
    }

    private void OnToolSwitchElec(InputAction.CallbackContext obj)
    {
        if (_PC._PlayerState != PlayerStates.DeathState)
        {
            if (ToolManager.instance._HandHeldTripShot.gameObject.activeSelf == false)
                ToolManager.instance._HandHeldTripShot.gameObject.SetActive(true);
            else if (ToolManager.instance._CurrentTool != null && !ToolManager.instance._CurrentTool.GetComponent<HandHeldTripShot>() && ToolManager.instance._CurTripShot == null)
                ToolManager.instance._HandHeldTripShot.CallEnable();
        }
    }
    private void OnToolSwitchTrap(InputAction.CallbackContext obj)
    {
        if (_PC._PlayerState != PlayerStates.DeathState)
        {
            if (ToolManager.instance._HandHeldTrap.gameObject.activeSelf == false)
                ToolManager.instance._HandHeldTrap.gameObject.SetActive(true);
            else if (ToolManager.instance._CurrentTool != null && !ToolManager.instance._CurrentTool.GetComponent<HandHeldTrap>() && ToolManager.instance._CurTrapTool == null)
                ToolManager.instance._HandHeldTrap.CallEnable();
        }
    }
    private void OnToolGoggle(InputAction.CallbackContext obj)
    {
        if (_PC._PlayerState != PlayerStates.DeathState)
        {
            ToolManager.instance._GoggleTool.gameObject.SetActive(true);
        }
    }
    private void OnToolCamera(InputAction.CallbackContext obj)
    {
        if (_PC._PlayerState != PlayerStates.DeathState)
        {
            ToolManager.instance._CurCameraTool.gameObject.SetActive(true);
        }
    }
}
