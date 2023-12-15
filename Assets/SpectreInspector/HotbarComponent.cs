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

    [SerializeField] private HandHeldTripShot _HandTripshot;
    [SerializeField] private HandHeldTrap _HandTrap;
    [SerializeField] private GoggleTool _HandGoggle;
    [SerializeField] private TrackerTool _HandCamera;


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
            _ElecTripSlot.gameObject.SetActive(true);
        }
    }
    private void OnToolSwitchTrap(InputAction.CallbackContext obj)
    {
        if (_PC._PlayerState != PlayerStates.DeathState)
        {
            _HandTrap.gameObject.SetActive(true);
        }
    }
    private void OnToolGoggle(InputAction.CallbackContext obj)
    {
        if (_PC._PlayerState != PlayerStates.DeathState)
        {
            _HandGoggle.gameObject.SetActive(true); // needs to be changed...
        }
    }
    private void OnToolCamera(InputAction.CallbackContext obj)
    {
        if (_PC._PlayerState != PlayerStates.DeathState)
        {
            _HandCamera.gameObject.SetActive(true); // needs to be changed...
        }
    }
}
