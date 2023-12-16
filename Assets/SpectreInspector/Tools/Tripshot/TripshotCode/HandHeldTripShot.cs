using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandHeldTripShot : PlaceableTool
{
    public Animator _ToolAnimator;

    public TripShotTool _Tripshot;
    [SerializeField] private Transform _TripshotHandModelTransform;

    bool _PreventSwap;
    Vector3 _Normal;

    public HotBarPos _ToolSlot;


    [SerializeField] private PhysicsPlayerController _PlayerControl;

    [SerializeField] private LayerMask _PlaceLayerMask;

    bool _CanPlace = false;

    [SerializeField] private PlayerInput _PlayerInput;


    private void OnEnable()
    {
        if (_PlayerControl == null)
        {
            _PlayerControl = FindObjectOfType<PhysicsPlayerController>();
        }

        if (_PlayerInput == null)
        {
            _PlayerInput = _PlayerControl._PlayerInput;
        }

        if (ToolManager.instance._CurrentTool == null || ToolManager.instance._CurrentTool.GetComponent<TripShotTool>() == null)
        {

            bool _delayToolSwap = false;
            if (ToolManager.instance._CurrentTool != null && ToolManager.instance._CurrentTool != this)
            {
                ToolManager.instance._CurrentTool._SwappingTool = this;

                if (ToolManager.instance._CurrentTool.GetComponent<GoggleTool>() || ToolManager.instance._CurrentTool.GetComponent<CameraTool>())
                    _delayToolSwap = true;

                ToolManager.instance._CurrentTool.Unequip();

                if (_delayToolSwap)
                {
                    _PreventSwap = true;
                    this.gameObject.SetActive(false);
                }
            }


            if (!_PreventSwap)
            {
                _ToolSlot.TweenUp();
                _ToolSlot.MoveArrow(0);


                if (ToolManager.instance._CurTripShot == null)
                {
                    NewInputSetup();

                    ToolManager.instance._CurrentTool = this;
                    ToolManager.instance._HoldingTool = true;
                    _ToolSelected = true;

                    base.Equip();

                    playEquipSound();

                    foreach (Tool tool in ToolManager.instance._HandToolsList)
                    {
                        if (tool != null & tool != this)
                            tool.gameObject.SetActive(false);
                    }

                    _ToolAnimator.SetBool("PickingUp", true);
                }
                else
                {
                    Tool curTool = ToolManager.instance._CurTripShot;
                    curTool._ToolSelected = true;
                    ToolManager.instance._CurrentTool = curTool;
                    ToolManager.instance._CurrentTool.Equip();

                    this.gameObject.SetActive(false);
                }
            }
            else
            {
                _PreventSwap = false;
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public void CallEnable()
    {
        OnEnable();
    }

    public override void Unequip()
    {
        NewInputUnSetup();

        base.Unequip();

        _ToolSlot.TweenDown();

        if (_SwappingTool == null)
        {
            _ToolSlot.PopArrow();
        }

        if (_ToolGhost)
            Destroy(_ToolGhost);

        ToolManager.instance._CurrentTool._ToolSelected = false;
        ToolManager.instance._HoldingTool = false;

        playUnequipSound();
        _ToolAnimator.SetBool("Unequiping", true);
    }

    public override void PlaceTool()
    {
        if (!_ToolPlaced && _ToolGhost != null && _PlayerControl._PlayerState == PlayerStates.PlayState)
        {
            GameObject tripShotTool = Instantiate(_ToolPrefab);
            TripShotTool TripShotScript = tripShotTool.GetComponent<TripShotTool>();

            tripShotTool.transform.parent = null;
            tripShotTool.transform.position = _TripshotHandModelTransform.position;
            tripShotTool.transform.rotation = _TripshotHandModelTransform.rotation;
            tripShotTool.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            TripShotScript._FinalPos = _ToolGhost.transform.position;
            TripShotScript._FinalNorm = _Normal;
            TripShotScript._PlayerControl = _PlayerControl;
            TripShotScript._PlayerInput = _PlayerInput;

            TripShotScript._FinalFor = _PlayerControl.transform.forward;
            tripShotTool.transform.forward = _PlayerControl.transform.forward;

            TripShotScript._ToolGhostExists = false;

            ToolManager.instance._CurTripShot = TripShotScript;

            _ToolGhostExists = false;
            Destroy(_ToolGhost);

            ToolManager.instance._CurrentTool = TripShotScript;

            _Tripshot = TripShotScript;

            base.Unequip();

            TripShotScript._HandHeldTripShot = this;
            ToolManager.instance._HoldingTool = false;

            _ToolSelected = false;

            _ToolAnimator.SetBool("Placing", true);
        }
    }

    private void Update()
    {
        if (_ToolSelected)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, _ToolPlaceDist, _PlaceLayerMask))
            {
                if (!_ToolGhostExists && Vector3.Distance(this.transform.position, hit.point) > 1.2f)
                {
                    _ToolGhostExists = true;
                    _ToolGhost = Instantiate(_ToolGhostPrefab);
                    _ToolGhost.transform.position = new Vector3 (hit.point.x, hit.point.y + _IncreasePlaceHeight, hit.point.z);
                    _ToolGhost.transform.rotation = Quaternion.LookRotation(hit.normal);
                    _Normal = hit.normal;
                }
                else if (_ToolGhost && Vector3.Distance(this.transform.position, hit.point) > 1.2f)
                {
                    _ToolGhost.transform.position = new Vector3(hit.point.x, hit.point.y + _IncreasePlaceHeight, hit.point.z);

                    _ToolGhost.transform.rotation = Quaternion.LookRotation(hit.normal);
                    _Normal = hit.normal;
                }
                _CanPlace = true;
            }
            else if (_ToolGhost != null && _ToolGhostExists)
            {
                Destroy(_ToolGhost);
                _ToolGhostExists = false;
            }
            if (_ToolGhost == null && !_ToolGhostExists)
            {
                _CanPlace = false;
            }
        }
        else if (_ToolGhost != null && _ToolGhostExists)
        {
            Destroy(_ToolGhost);
            _ToolGhostExists = false;
        }

        if (_ToolGhost == null)
        {
            _ToolGhostExists = false;
        }
    }

    private void DoPrimaryAction(InputAction.CallbackContext obj)
    {
        if(_CanPlace)
        {
            PlaceTool();
        }
    }

    private void DoUnequipAction(InputAction.CallbackContext obj)
    {
        Unequip();
    }

    void NewInputSetup()
    {
        _PlayerInput.actions["PrimaryAction"].performed += DoPrimaryAction;
        _PlayerInput.actions["PrimaryAction"].Enable();

        _PlayerInput.actions["Unequip"].performed += DoUnequipAction;
        _PlayerInput.actions["Unequip"].Enable();
    }

    void NewInputUnSetup()
    {
        _PlayerInput.actions["PrimaryAction"].performed -= DoPrimaryAction;
        _PlayerInput.actions["PrimaryAction"].Disable();

        _PlayerInput.actions["Unequip"].performed -= DoUnequipAction;
        _PlayerInput.actions["Unequip"].Disable();
    }
}
