using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GoggleTool : HandheldTool
{

    [Header("First Time Lines:")]
    
    private bool DoneDialogue = false;

    [Header("other:")]
    public Image _FadeToBlack;
    public GameObject _GoggleScan;
    bool _FadingToBlack;
    bool _FadingToClear;
    float ToBlack;
    float ToClear;

    public float _TimeToBlack = 1;
    public float _TimeToClear = 1;
    public float _TimeIsBlack = 2;

    bool _RemovingGoggles;

    //public FootprintObjectPools _FootPrintPoolObject;

    //public List<SpriteRenderer> _FootPrints= new List<SpriteRenderer>();

    public Animator _ToolAnimator;
    public GameObject[] _GogglesObjects;

    public List<GameObject> _OtherTools = new List<GameObject>();

    bool _PreventSwap;
    //bool _RemovingGogglesBool;
    //public HotBarPos _ToolSlot;


    public Outline[] _OutlineObjects;

    public List<GameObject> pooledPaths;
    public GameObject pathsToPool;
    public int amountToPool = 10;
    //public PhysicsPlayerController _Player;
    public LayerMask _TrailSpawnPosLayerMask;

    //public GhostRoom[] _Rooms;

    private PlayerInput _PlayerInput;

    PhysicsPlayerController playerControl;

    private void Awake()
    {
        /*pooledPaths = new List<GameObject>();/////////////////////////////////////////////////////////////////////
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(pathsToPool);
            tmp.SetActive(false);
            GoggleVisualPath path = tmp.GetComponent<GoggleVisualPath>();
            if (path)
            {
                path._GhostToGoTo = (Ghosts)i;
            }
            pooledPaths.Add(tmp);
        }*/

        if (playerControl == null)
            playerControl = FindObjectOfType<PhysicsPlayerController>();

        //_Rooms = FindObjectsOfType<GhostRoom>();

        
    }

    public GameObject GetPooledObject()//////////////////////////////////////////////////////////////////////
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledPaths[i].activeInHierarchy)
            {
                return pooledPaths[i];
            }
        }


        return null;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        if (playerControl == null)
            playerControl = Camera.main.transform.parent.GetComponent<PhysicsPlayerController>();

        

        bool _delayToolSwap = false;
        if (playerControl._CurrentTool != null && playerControl._CurrentTool != this)
        {
            playerControl._CurrentTool._SwappingTool = this;

            if (playerControl._CurrentTool.gameObject.GetComponent<HandHeldTrap>() || playerControl._CurrentTool.gameObject.GetComponent<TrackerTool>())
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
            //_ToolSlot.MoveArrow(2);

            if (playerControl._CurrentTool != null && playerControl._CurrentTool != this)
                playerControl._CurrentTool.Unequip();

            if (_PlayerInput == null)
            {
                //_PlayerInput = playerControl.playerInput; ///////////////////////////
            }

            NewInputSetup();

            playerControl._CurrentTool = this;
            playerControl._HoldingTool = true;
            _ToolSelected = true;

            base.Equip();

            for (int i = 0; i < _OtherTools.Count; i++)
            {
                _OtherTools[i].gameObject.SetActive(false);
            }

            //BeginFadeToBlack();

            _ToolAnimator.SetBool("Equipping", true);
        }
        else
        {
            _PreventSwap = false;
        }
    }

    public void BeginFadeToBlack()
    {
        _FadingToBlack = true;
        ToBlack = 0;
        ToClear = 0;
        _RemovingGoggles = false;
        _FadeToBlack.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_RemovingGoggles)
        {
            if (_FadingToBlack)
            {
                ToBlack += Time.deltaTime / _TimeToBlack;

                if (ToBlack < 1)
                {
                    _FadeToBlack.color = new Vector4(_FadeToBlack.color.r, _FadeToBlack.color.g, _FadeToBlack.color.b, Mathf.Lerp(_FadeToBlack.color.a, 1, ToBlack));
                }
                else if (ToBlack >= _TimeToBlack + _TimeIsBlack)
                {
                    _FadingToBlack = false;
                    _FadingToClear = true;
                    _GoggleScan.SetActive(true);

                    for (int i = 0; i < _OutlineObjects.Length; i++)
                    {
                        _OutlineObjects[i].enabled = true;
                    }

                    for (int i = 0; i < _GogglesObjects.Length; i++)
                    {
                        _GogglesObjects[i].SetActive(false);
                    }
                    



                    // UNDO THIS WHEN WE HAVE A CHANCE PLEASE - LEE

                    //for (int i = 0; i < _FootPrintPoolObject.pooledObjects.Count; i++)
                    //{
                    //    _FootPrintPoolObject.pooledObjects[i].GetComponentInChildren<SpriteRenderer>().enabled = true;
                    //}
                }
            }
            else if (_FadingToClear)
            {
                ToClear += Time.deltaTime / _TimeToClear;
                _FadeToBlack.color = new Vector4(_FadeToBlack.color.r, _FadeToBlack.color.g, _FadeToBlack.color.b, Mathf.Lerp(_FadeToBlack.color.a, 0, ToClear));

                if (ToClear >= 1)
                {
                    _FadingToClear = false;
                    _FadeToBlack.gameObject.SetActive(false);

                    RaycastHit hit;
                    Vector3 hitpos = playerControl.transform.position;
                    hitpos.y -= 1f;

                    /*if (Physics.Raycast(_Player.transform.position, -Vector3.up, out hit, 5, _TrailSpawnPosLayerMask))
                    {
                        hitpos = hit.point;
                        hitpos.y += 0.4f;
                    }*/



                    /*for (int i = 0; i < _Rooms.Length; i++)
                    {
                        if (_Rooms[i]._RoomInUse)
                        {
                            GameObject trail = GetPooledObject();
                            trail.transform.position = hitpos;
                            trail.SetActive(true);
                            //trail.GetComponent<GoggleVisualPath>()._Agent.destination = _Rooms[i].transform.position;////////////////////////////////////
                        }
                        
                    }*/
                }
            }
        }
        else
        {
            if (_FadingToBlack)
            {
                ToBlack += Time.deltaTime / _TimeToBlack;

                if (ToBlack < 1)
                {
                    _FadeToBlack.color = new Vector4(_FadeToBlack.color.r, _FadeToBlack.color.g, _FadeToBlack.color.b, Mathf.Lerp(_FadeToBlack.color.a, 1, ToBlack));
                }
                else if (ToBlack >= _TimeToBlack + _TimeIsBlack)
                {
                    _FadingToBlack = false;
                    _FadingToClear = true;
                    _GoggleScan.SetActive(false);
                    _ToolAnimator.SetBool("Unequipping", true);

                    for (int i = 0; i < _OutlineObjects.Length; i++)
                    {
                        _OutlineObjects[i].enabled = false;
                    }

                    for (int i = 0; i < _GogglesObjects.Length; i++)
                    {
                        _GogglesObjects[i].SetActive(true);
                    }

                    for (int i = 0; i < pooledPaths.Count; i++)
                    {
                        pooledPaths[i].gameObject.SetActive(false);//////////////////////////////////////////////////////////////////////////////////////
                    }

                    // interesting hmmmmmmmm WE SHOULD FIX THIS - Lee

                    //for (int i = 0; i < _FootPrintPoolObject.pooledObjects.Count; i++)
                    //{
                    //    _FootPrintPoolObject.pooledObjects[i].GetComponentInChildren<SpriteRenderer>().enabled = false;
                    //}
                }
            }
            else if (_FadingToClear)
            {
                ToClear += Time.deltaTime / _TimeToClear;
                _FadeToBlack.color = new Vector4(_FadeToBlack.color.r, _FadeToBlack.color.g, _FadeToBlack.color.b, Mathf.Lerp(_FadeToBlack.color.a, 0, ToClear));

                if (ToClear >= 1)
                {
                    _FadingToClear = false;
                    _FadeToBlack.gameObject.SetActive(false);
                    _RemovingGoggles = false;
                    _ToolAnimator.SetBool("Unequipping", false);
                    UnequipGoggles();
                }
            }
        }

        /*if (Input.GetMouseButtonDown(1))
        {
            RemoveGoggles();
        }*/
    }

    public void RemoveGoggles()
    {
        if (!_RemovingGoggles)
        {
            _RemovingGoggles = true;
            ToBlack = 0;
            ToClear = 0;
            _FadingToBlack = true;
            _FadeToBlack.gameObject.SetActive(true);
            _ToolAnimator.SetBool("Equipping", false);
        }
    }

    public override void Unequip()
    {
        RemoveGoggles();
    }

    public void UnequipGoggles()
    {
        base.Unequip();
        _GoggleScan.SetActive(false);

        playerControl._CurrentTool = null;
        playerControl._HoldingTool = false;



        if (_SwappingTool != null)
        {
            _SwappingTool.gameObject.SetActive(true);
            _SwappingTool = null;
        }
        else
        {


            //_ToolSlot._Arrow.gameObject.SetActive(false);//////////////////////////////
        }

        //_ToolSlot.TweenDown();

        this.gameObject.SetActive(false);
    }

    void NewInputSetup()
    {
        _PlayerInput.actions["Unequip"].performed += DoUnequipAction;
        _PlayerInput.actions["Unequip"].Enable();
    }

    void NewInputUnSetup()
    {
        _PlayerInput.actions["Unequip"].performed -= DoUnequipAction;
        _PlayerInput.actions["Unequip"].Disable();
    }

    private void DoUnequipAction(InputAction.CallbackContext obj)
    {
        RemoveGoggles();
    }

}
