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

        NewInputSetup();


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


                    //Debug.Log("Awoken");
                    ToolManager.instance._CurrentTool = this;
                    ToolManager.instance._HoldingTool = true;
                    _ToolSelected = true;

                    //this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x + _HandHeldRot.x, this.transform.localEulerAngles.y + _HandHeldRot.y, this.transform.localEulerAngles.z + _HandHeldRot.z);
                    //this.transform.localPosition = new Vector3(this.transform.localPosition.x + _HandHeldDistFromCam.x, this.transform.localPosition.y + _HandHeldDistFromCam.y, this.transform.localPosition.z + _HandHeldDistFromCam.z);

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
                    //curTool.GetComponent<TripShotTool>()._TripShotTool = playerControl._ToolManager._CurTripShot.gameObject;
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

    /*public override void Equip()
    {
        PlayerControl playerControl = Camera.main.transform.parent.GetComponent<PlayerControl>();

        if (playerControl._ToolManager._CurTripShot == null)
        {
            base.Equip();

            Tool currentTool = playerControl._CurrentTool;

            if (currentTool == null) // NEED TO CHANGE TO HANDLE MULTIPLE TOOLS
            {
                InstantiateTripShot(currentTool);
            }
            else if (currentTool.GetComponent<TripShotTool>() == null && currentTool.GetComponent<HandHeldTripShot>() == null)
            {
                InstantiateTripShot(currentTool);
            }
        }
        else
        {
            Tool curTool = playerControl._ToolManager._CurTripShot;
            curTool._ToolSelected = true;
            curTool.GetComponent<TripShotTool>()._TripShotTool = playerControl._ToolManager._CurTripShot.gameObject;
            playerControl._CurrentTool = curTool;
        }

    }*/

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

    

    /*void InstantiateTripShot(Tool curTool)
    {
        _HandHeldTripShot = Instantiate(_TripShotHandHeldPrefab, Camera.main.transform);
        _HandHeldTripShot.transform.localEulerAngles = new Vector3(_HandHeldTripShot.transform.localEulerAngles.x + _HandHeldRot.x, _HandHeldTripShot.transform.localEulerAngles.y + _HandHeldRot.y, _HandHeldTripShot.transform.localEulerAngles.z + _HandHeldRot.z);
        _HandHeldTripShot.transform.localPosition = new Vector3(_HandHeldTripShot.transform.localPosition.x + _HandHeldDistFromCam.x, _HandHeldTripShot.transform.localPosition.y + _HandHeldDistFromCam.y, _HandHeldTripShot.transform.localPosition.z + _HandHeldDistFromCam.z);

        // Will Break if main camera is not a direct child of the player

        curTool = _HandHeldTripShot.GetComponent<HandHeldTripShot>();
        curTool._ToolSelected = true;
        curTool.GetComponent<HandHeldTripShot>()._HandHeldTripShot = _HandHeldTripShot;
        Camera.main.transform.parent.GetComponent<PlayerControl>()._CurrentTool = curTool;
    }*/

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



            TripShotScript._FinalFor = _PlayerControl.transform.forward;
                //Camera.main.transform.root.forward;
            tripShotTool.transform.forward = _PlayerControl.transform.forward;
            //Camera.main.transform.root.forward;

            //tripShotTool.GetComponent<TripShotTool>().BeginLerp();

            /*tripShotTool.transform.position = _ToolGhost.transform.position;
            tripShotTool.transform.rotation = _ToolGhost.transform.rotation;
            tripShotTool.transform.GetChild(0).rotation = _ToolGhost.transform.GetChild(0).rotation;*/


            //tripShotTool.GetComponent<TripShotTool>()._ToolPlaced = true;
            TripShotScript._ToolGhostExists = false;
            //tripShotTool.GetComponent<TripShotTool>()._TripShotTool = tripShotTool;
            //TripShotScript._ToolManager = playerControl._ToolManager;



            ToolManager.instance._CurTripShot = TripShotScript;

            _ToolGhostExists = false;
            Destroy(_ToolGhost);

            ToolManager.instance._CurrentTool = TripShotScript;

            _Tripshot = TripShotScript;

            base.Unequip();

            //tripShotTool.GetComponent<TripShotTool>().Equip();
            TripShotScript._HandHeldTripShot = this;
            ToolManager.instance._HoldingTool = false;

            //this.gameObject.SetActive(false);
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
                    //_ToolGhost.transform.GetChild(0).transform.forward = -Camera.main.transform.right;


                    /*float xVal = _ToolGhost.transform.rotation.x;
                    
                    _ToolGhost.transform.rotation = new Quaternion(xVal, _ToolGhost.transform.rotation.y, _ToolGhost.transform.rotation.z, _ToolGhost.transform.rotation.w);
                    //_ToolGhost.transform.eulerAngles = Vector3.zero;*/
                }
                else if (_ToolGhost && Vector3.Distance(this.transform.position, hit.point) > 1.2f)
                {
                    _ToolGhost.transform.position = new Vector3(hit.point.x, hit.point.y + _IncreasePlaceHeight, hit.point.z);

                    _ToolGhost.transform.rotation = Quaternion.LookRotation(hit.normal);
                    _Normal = hit.normal;

                    //_ToolGhost.transform.GetChild(0).transform.forward = -Camera.main.transform.right;



                    /*float xVal = _ToolGhost.transform.rotation.x;
                    
                    _ToolGhost.transform.rotation = new Quaternion(xVal, _ToolGhost.transform.rotation.y, _ToolGhost.transform.rotation.z, _ToolGhost.transform.rotation.w);*/
                }
                _CanPlace = true;
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


        //if (Input.GetMouseButtonDown(1) && _ToolSelected)
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
