using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandHeldTrap : PlaceableTool
{
    [Header("Standard:")]
    public Animator _ToolAnimator;

    [SerializeField] private Transform _TrapHandModelTransform;

    bool _PreventSwap;
    Vector3 _Normal;

    public HotBarPos _ToolSlot;

    public PhysicsPlayerController playerControl;
    private PlayerInput _PlayerInput;
    
    private void Awake()
    {
        if (playerControl == null)
        {
            playerControl = FindObjectOfType<PhysicsPlayerController>();
        }
    }

    private void OnEnable()
    {
        if (_PlayerInput == null)
        {
            _PlayerInput = playerControl._PlayerInput;
        }

        bool _delayToolSwap = false;
        if (ToolManager.instance._CurrentTool != null && ToolManager.instance._CurrentTool != this)
        {
            ToolManager.instance._CurrentTool._SwappingTool = this;

            if (ToolManager.instance._CurrentTool.GetComponent<HandHeldTripShot>() || ToolManager.instance._CurrentTool.GetComponent<GoggleTool>() || ToolManager.instance._CurrentTool.GetComponent<CameraTool>())
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
            _ToolSlot.MoveArrow(1);

            if (ToolManager.instance._CurTrapTool == null)
            {
                _ToolSlot.TweenUp();
                
                ToolManager.instance._CurrentTool = this;
                ToolManager.instance._HoldingTool = true;
                _ToolSelected = true;

                base.Equip();

                foreach (Tool tool in ToolManager.instance._HandToolsList)
                {
                    if (tool != null & tool != this)
                        tool.gameObject.SetActive(false);
                }

                NewInputSetup();
                _ToolAnimator.SetBool("PickingUp", true);
                _ToolAnimator.SetBool("Placing", false);
            }
            else
            {
                Tool curTool = ToolManager.instance._CurTrapTool;
                curTool._ToolSelected = true;
                curTool.GetComponent<TrapTool>().CallEnable();
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

        if (ToolManager.instance._CurrentTool != null)
        {
            ToolManager.instance._CurrentTool._ToolSelected = false;
        }
        ToolManager.instance._HoldingTool = false;
        _ToolAnimator.SetBool("Unequiping", true);
    }

    public override void PlaceTool()
    {
        if (!_ToolPlaced && _ToolGhost != null && playerControl._PlayerState == PlayerStates.PlayState)
        {
            NewInputUnSetup();

            GameObject trapTool = Instantiate(_ToolPrefab);
            TrapTool trapToolScript = trapTool.GetComponent<TrapTool>();

            trapTool.transform.parent = null;
            trapTool.transform.position = _TrapHandModelTransform.position;
            trapTool.transform.rotation = _TrapHandModelTransform.rotation;

            trapToolScript._FinalPos = _ToolGhost.transform.position;
            trapToolScript._FinalNorm = _Normal;

            trapToolScript._ToolGhostExists = false;
            trapToolScript.playerControl = playerControl;
            trapToolScript._PlayerInput = _PlayerInput;
            ToolManager.instance._CurTrapTool = trapToolScript;

            _ToolGhostExists = false;
            Destroy(_ToolGhost);

            ToolManager.instance._CurrentTool = trapToolScript;
            ToolManager.instance._HoldingTool = false;

            base.Unequip();

            trapToolScript._HandHeldTrap = this;

            _ToolSelected = false;
            _ToolAnimator.SetBool("PickingUp", false);
            _ToolAnimator.SetBool("Placing", true);
        }
    }

    void Update()
    {
        if (_ToolSelected)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, _ToolPlaceDist))
            {
                if (hit.normal.y >= 0.5)
                {
                    if (!_ToolGhostExists && Vector3.Distance(this.transform.position, hit.point) > 1.2f)
                    {
                        _ToolGhostExists = true;
                        _ToolGhost = Instantiate(_ToolGhostPrefab);
                        _ToolGhost.transform.position = new Vector3(hit.point.x, hit.point.y + _IncreasePlaceHeight, hit.point.z);
                        _ToolGhost.transform.eulerAngles = Vector3.zero;
                        _Normal = hit.normal;
                    }
                    else if (_ToolGhost && Vector3.Distance(this.transform.position, hit.point) > 1.2f)
                    {
                        _ToolGhost.transform.position = new Vector3(hit.point.x, hit.point.y + _IncreasePlaceHeight, hit.point.z);
                        _ToolGhost.transform.eulerAngles = Vector3.zero;
                        _Normal = hit.normal;

                        Debug.Log(hit.collider.gameObject.name);
                    }
                }
                else if (_ToolGhost != null && _ToolGhostExists)
                {
                    Destroy(_ToolGhost);
                    _ToolGhostExists = false;
                }
            }
            else if (_ToolGhost != null && _ToolGhostExists)
            {
                Destroy(_ToolGhost);
                _ToolGhostExists = false;
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
        if(_ToolGhostExists)
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
