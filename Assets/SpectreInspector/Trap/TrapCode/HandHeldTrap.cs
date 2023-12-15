using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandHeldTrap : PlaceableTool
{

    


    [Header("Standard:")]
    public Animator _ToolAnimator;

    public Transform _TrapHandModelTransform;

    bool _PreventSwap;
    Vector3 _Normal;

    public List<GameObject> _OtherTools = new List<GameObject>();
    //public HotBarPos _ToolSlot;


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
            //_PlayerInput = playerControl.playerInput;
        }

        



        bool _delayToolSwap = false;
        if (playerControl._CurrentTool != null && playerControl._CurrentTool != this)
        {
            playerControl._CurrentTool._SwappingTool = this;

            if (playerControl._CurrentTool.gameObject.GetComponent<GoggleTool>() || playerControl._CurrentTool.gameObject.GetComponent<TrackerTool>())
                _delayToolSwap = true;

            playerControl._CurrentTool.Unequip();

            if (_delayToolSwap)
            {
                _PreventSwap = true;
                this.gameObject.SetActive(false);
            }
        }

        if (!_PreventSwap)
        {
            

            //_ToolSlot.TweenUp();
            //_ToolSlot.MoveArrow(1);

            if (playerControl._ToolManager._CurTrapTool == null)
            {
                

                playerControl._CurrentTool = this;
                playerControl._HoldingTool = true;
                _ToolSelected = true;

                base.Equip();

                for (int i = 0; i < _OtherTools.Count; i++)
                {
                    _OtherTools[i].gameObject.SetActive(false);
                }

                NewInputSetup();
                _ToolAnimator.SetBool("PickingUp", true);
            }
            else
            {
                Tool curTool = playerControl._ToolManager._CurTrapTool;
                curTool._ToolSelected = true;
                curTool.GetComponent<TrapTool>().CallEnable();
                playerControl._CurrentTool = curTool;
                playerControl._CurrentTool.Equip();

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

        //_ToolSlot.TweenDown();


        if (_SwappingTool == null)
        {
            //_ToolSlot.PopArrow();
        }


        if (_ToolGhost)
            Destroy(_ToolGhost);

        if (playerControl._CurrentTool != null)
        {
            playerControl._CurrentTool._ToolSelected = false;
            //transform.root.GetComponent<PlayerControl>()._CurrentTool = null;
        }
        playerControl._HoldingTool = false;

        //this.gameObject.SetActive(false);

        _ToolAnimator.SetBool("Unequiping", true);
    }

    public override void PlaceTool()
    {
        if (!_ToolPlaced && _ToolGhost != null && playerControl._PlayerState == PlayerStates.PlayState)
        {

            //RumbleManager.instance.RumblePulse(1, 1, 0.25f);

            GameObject trapTool = Instantiate(_ToolPrefab);
            TrapTool trapToolScript = trapTool.GetComponent<TrapTool>();

            trapTool.transform.parent = null;
            //trapTool.transform.position = _ToolGhost.transform.position;
            trapTool.transform.position = _TrapHandModelTransform.position;
            trapTool.transform.rotation = _TrapHandModelTransform.rotation;

            //trapToolScript._ToolPlaced = true;
            trapToolScript._FinalPos = _ToolGhost.transform.position;
            trapToolScript._FinalNorm = _Normal;

            trapToolScript._ToolGhostExists = false;
            trapToolScript._ToolManager = playerControl._ToolManager;
            trapToolScript.playerControl = playerControl;
            //trapToolScript._GPG = GravPolt_Gun;
            playerControl._ToolManager._CurTrapTool = trapToolScript;

            _ToolGhostExists = false;
            Destroy(_ToolGhost);

            playerControl._CurrentTool = trapToolScript;
            playerControl._HoldingTool = false;

            base.Unequip();

            //trapToolScript.Equip();
            trapToolScript._HandHeldTrap = this;

            //this.gameObject.SetActive(false);
            _ToolSelected = false;
            _ToolAnimator.SetBool("Placing", true);
        }
    }

    // Update is called once per frame
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

                    //if (Input.GetMouseButtonDown(0))
                    //{
                    //    PlaceTool();
                    //}
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


        //if (Input.GetMouseButtonDown(1))
        //{
        //    Unequip();
        //}

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
