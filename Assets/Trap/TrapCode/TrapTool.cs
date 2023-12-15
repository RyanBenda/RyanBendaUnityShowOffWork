using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.InputSystem;
//using UnityEngine.UIElements;
//using Image = UnityEngine.UI.Image;

public class TrapTool : PlaceableTool
{
    [HideInInspector]
    public ToolManager _ToolManager;

    [HideInInspector]
    public HandHeldTrap _HandHeldTrap;





    public List<GameObject> _VisualTrapRange = new List<GameObject>();

    bool _CreatureInRange;

    public Material _BlueMat;
    public Material _RedMat;
    public Material _GreenMat;

    

    [HideInInspector]
    public List<CreatureBrain> _CreaturesInRange = new List<CreatureBrain>();
    [HideInInspector]
    public List<CreatureBrain> _CreaturesCaught = new List<CreatureBrain>();

    bool _CreaturesCaptured;
    bool _TweenIsPlaying;
    public float _TimeTillEscape = 10;
    float _TimeTillEscapeRememberer;

    public TextMeshProUGUI _Countdown;

    public Image _CountdownSprite;
    public Sprite[] _CountdownSprites;

    public Animator _TrapAnimator;

    [HideInInspector]
    public Vector3 _FinalPos;

    [HideInInspector]
    public Vector3 _FinalNorm;

    float t = 0;

    public Collider[] _Colliders;

    //public HotBarPos _ToolSlot;

    public LayerMask _ToolPickupLayerMask;

    public TrapLineRendererManager _TrapLineRenderer;

    bool _Tweening;

    [HideInInspector]
    public PhysicsPlayerController playerControl;
    private PlayerInput _PlayerInput;

    bool _CanGetNewGhosts = true;
    public Canvas _LogoCanvas;

    bool _BlockTrap;

    bool _CanCatchGhosts;
    bool CanCatch;

    private void OnEnable()
    {

        _TimeTillEscapeRememberer = _TimeTillEscape;

        if (_PlayerInput == null)
        {
            //_PlayerInput = playerControl.playerInput;
        }

        NewInputSetup();

        /*foreach (GameObject g in _VisualTrapRange)
        {
            var mat = g.GetComponentInChildren<Renderer>();
            mat.material.shader = Shader.Find("TerrainScanner");
            mat.material.SetColor("IntersectionColour", _BlueColour);
        }*/


    }

    public void CallEnable()
    {
        OnEnable();
    }

    private void Awake()
    {
        if (playerControl == null)
            playerControl = FindObjectOfType<PhysicsPlayerController>();

        
    }


    public void RemoveTrap()
    {
        _ToolPlaced = false;
        _ToolManager._CurTrapTool = null;
        //playerControl._CurrentTool = null;

        //_HandHeldTrap._ToolSlot.TweenDown();
        //_HandHeldTrap._ToolSlot.PopArrow();

        NewInputUnSetup();

        Destroy(this.gameObject);
    }

    //public override void ReturnTool()
    //{

    //    if (Input.GetKeyDown(KeyCode.R) && playerControl._CurrentTool == this.gameObject.GetComponent<TrapTool>() && _CreaturesCaught.Count == 0 && !_TrapLineRenderer._InUse)
    //    {
    //        _ToolManager.UnequipTools();

    //        _ToolPlaced = false;
    //        _ToolManager._CurTrapTool = null;
    //        playerControl._CurrentTool = null;

    //        _HandHeldTrap._ToolSlot.TweenDown();
    //        _HandHeldTrap._ToolSlot.PopArrow();

    //        Destroy(this.gameObject);
    //    }
        
    //}

    public override void PickUpTool()
    {
        if (_ToolPlaced)
        {
            RaycastHit hit;

            if (Physics.SphereCast(Camera.main.transform.position - (Camera.main.transform.forward * 2), 2, Camera.main.transform.forward, out hit, 4, _ToolPickupLayerMask))
            {
                if (hit.transform.root.GetComponent<TrapTool>() != null && playerControl._HoldingTool == false && !_TrapLineRenderer._InUse)
                {
                    if (_ToolOutlines[0].activeSelf == false)
                    {
                        foreach (GameObject outline in _ToolOutlines)
                        {
                            outline.SetActive(true);
                        }

                        _LogoCanvas.gameObject.SetActive(false);
                    }

                    //if (Input.GetKeyDown(KeyCode.E))
                    //{
                    //     RetrieveTrap();
                    //}
                }
                else
                {
                    if (_ToolOutlines[0].activeSelf == true)
                    {
                        foreach (GameObject outline in _ToolOutlines)
                        {
                            outline.SetActive(false);
                        }

                        _LogoCanvas.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                if (_ToolOutlines[0].activeSelf == true)
                {
                    foreach (GameObject outline in _ToolOutlines)
                    {
                        outline.SetActive(false);
                    }
                    _LogoCanvas.gameObject.SetActive(true);
                }
            }
        }
        
    }

    public void RetrieveTrap()
    {
        for (int i = 0; i < _CreaturesCaught.Count;)
        {
            /*InventoryComponent inven = Camera.main.transform.parent.GetComponent<InventoryComponent>();

            for (int j = 0; j < inven._Inventory.Count; j++)
            {
                if (inven._Inventory[j]._Creature == null)
                {
                    InvenStruct invenSlot = inven._Inventory[j];
                    invenSlot._Creature = _CreaturesCaught[i]._CreatureIdentity;
                    //invenSlot._CreatureSprite = _CreaturesCaught[i]._CreatureIdentity._Image; ////////////////////////////////////////////

                    invenSlot._CreatureImage.sprite = invenSlot._CreatureSprite;
                    inven._Inventory[j] = invenSlot;

                    QuestManager.TriggerGhostCaptured(_CreaturesCaught[i]);

                    break;
                }

                if (j == inven._Inventory.Count - 1)
                {
                    inven.InventoryFull(_CreaturesCaught[i]._CreatureIdentity);
                }
            }

            if (_CreaturesCaught.Count == 1)
            {

                inven._CaughtCreatureBanner.GetComponent<CaughtPrefabComponent>().SetCreature(_CreaturesCaught[i]._CreatureIdentity._Sprite, _CreaturesCaught[i]._CreatureIdentity._NamePlate);

                //inven._CaughtCreatureBanner.GetComponent<CaughtPrefabComponent>()._CreatureImage.sprite = _CreaturesCaught[i]._CreatureIdentity._Sprite; /////////////////////////////////
                //inven._CaughtCreatureBanner.GetComponent<CaughtPrefabComponent>()._CreatureName.text = _CreaturesCaught[i]._CreatureIdentity._Name;
                //inven._CaughtCreatureBanner.GetComponent<CaughtPrefabComponent>()._CreatureBannerActive = true;

                inven._CaughtCreatureBanner.gameObject.SetActive(true);

            }*/

            //_CreaturesCaught[i]._CurRoom._RoomInUse = false;

            GameObject crea = _CreaturesCaught[i].gameObject;
            _CreaturesCaught.Remove(_CreaturesCaught[i]);
            Destroy(crea);

            /*if (!AudioManager.instance.DoneCaughtDialogue)
            {
                AudioManager.instance.DoneCaughtDialogue = true;
                CallArlo.instance.PlayArloLine(CaughtDialogue);
            }*/

        }



        /*if (playerControl._CurrentTool)
        {
            playerControl._CurrentTool.NewInputSetup();
        }*/

        
        if (playerControl._CurrentTool == this || playerControl._CurrentTool == null)
        {
            NewInputUnSetup();

            _ToolManager.UnequipTools();
            _ToolManager._CurTrapTool = null;

            //_ToolManager._CurTrapTool = null;
            playerControl._CurrentTool = null;



            if (_HandHeldTrap.gameObject.activeSelf == true)
            {
                _HandHeldTrap.CallEnable();
            }
            else
            {
                _HandHeldTrap.gameObject.SetActive(true);
            }

            _HandHeldTrap._ToolAnimator.SetBool("Placing", false);
            _HandHeldTrap._ToolAnimator.SetBool("PickingUp", true);
            _HandHeldTrap._ToolSelected = true;


        }
        else
        {

            Unequip();
            //_ToolManager._CurTrapTool = null;
            //_HandHeldTrap._ToolSlot.TweenDown();
        }

        _ToolManager._CurTrapTool = null;
        _ToolPlaced = false;

        Destroy(this.gameObject);
    }

    public void CatchCreatures()
    {
        _TrapAnimator.SetBool("Capturing", true);
        _TrapAnimator.SetBool("Landed", false);

        for (int i = 0; i < _CreaturesInRange.Count; i++)
        {
            _CreaturesCaught.Add(_CreaturesInRange[i]);

            _CreaturesCaptured = true;

            CreatureObjectPooling _pooledCrea = _CreaturesInRange[i].GetComponent<CreatureObjectPooling>();

            if (_pooledCrea)
            {
                for (int k = 0; k < _pooledCrea.pooledCreatures.Count; k++)
                {
                    //_pooledCrea.pooledCreatures[k].GetComponent<CloneBrain>()._HasBeenCaught = true;
                    //_pooledCrea.pooledCreatures[k].gameObject.SetActive(false);
                    _pooledCrea.pooledCreatures[k].GetComponent<NavMeshAgent>().enabled = false;
                    //_pooledCrea.pooledCreatures[k].GetComponent<BehaviourTreeRunner>().enabled = false;
                    _pooledCrea.pooledCreatures[k].GetComponent<CloneBrain>().RemoveClone();

                }
                _CreaturesInRange[i]._HasActiveClones = false;
                _CreaturesInRange[i]._RunRandomly = false;
            }

            _CreaturesInRange[i].gameObject.SetActive(false);
            _CreaturesInRange.Remove(_CreaturesInRange[i]);

            i--;

            
        }
        _CreatureInRange = false;
        

        _Countdown.transform.parent.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (_ToolPlaced && playerControl._PlayerState == PlayerStates.PlayState)
        {
            PickUpTool();
            //ReturnTool();

            CanCatch = true;
            //bool _NoClones = true;
            if (_CreaturesInRange.Count != 0)
            {
                foreach (CreatureBrain Brain in _CreaturesInRange)
                {
                    if (Brain._CanBeTrappedCurrently == false || Brain.transform.GetComponentInChildren<PhysicsPlayerController>() == true)
                    {
                        CanCatch = false;
                    }
                }

            }
            else if (!_TrapLineRenderer._VerifiedPath)
            {
                _TrapLineRenderer._InUse = false;
            }

            //if (_CreatureInRange && _CreaturesInRange.Count == 0)
            //CanCatch = false;

            if (_CreatureInRange && !_CreaturesCaptured && CanCatch)
            {
                


                foreach (GameObject g in _VisualTrapRange)
                {
                    foreach (Renderer r in g.GetComponentsInChildren<Renderer>())
                    {
                        r.material = _GreenMat;
                    }
                }

                if (playerControl._CurrentTool == this.gameObject.GetComponent<TrapTool>())
                {
                    _CanCatchGhosts = true;

                }
                else
                    _CanCatchGhosts = false;

            }
            else if (!CanCatch)
            {
                _CanCatchGhosts = false;
                foreach (GameObject g in _VisualTrapRange)
                {
                    foreach (Renderer r in g.GetComponentsInChildren<Renderer>())
                    {
                        r.material = _RedMat;
                    }
                }
            }
            else if (_CreaturesInRange.Count == 0)
            {
                _CanCatchGhosts = false;
                foreach (GameObject g in _VisualTrapRange)
                {
                    foreach (Renderer r in g.GetComponentsInChildren<Renderer>())
                    {
                        r.material = _BlueMat;
                    }
                }

            }
            else
                _CanCatchGhosts = false;


            if (_CreaturesCaptured && _TimeTillEscape > 0 && _TrapAnimator.GetBool("Caught") == true)
            {
                


                _TimeTillEscape -= Time.deltaTime;
                float timeText = Mathf.Ceil(_TimeTillEscape);

                if (timeText > 0)
                {
                    _CountdownSprite.sprite = _CountdownSprites[(int)timeText - 1];
                }
                _Countdown.text = timeText.ToString();

                /*if (!_TweenIsPlaying)
                {
                    _TweenIsPlaying = true;

                    Sequence mySequence = DOTween.Sequence();
                    mySequence.Append(transform.DOLocalJump(transform.position, 0.25f, 1, 0.75f).Join(transform.DOPunchRotation(new Vector3(24, 17, 23), 1f).SetEase(Ease.OutBounce)));

                    mySequence.SetLoops(-1);
                    mySequence.Play();

                }*/
            }
            else if (_TimeTillEscape <= 0)
            {
                _TimeTillEscape = _TimeTillEscapeRememberer;
                _CreaturesCaptured = false;

                _TrapAnimator.SetBool("Escape", true);


                for (int i = 0; i < _CreaturesCaught.Count; i++)
                {
                    //GameObject crea = Instantiate(_CreaturesCaught[i]._CreatureIdentity._CreaturePrefab, this.transform);
                    //crea.transform.parent = null;
                    _CreaturesCaught[i].transform.position = this.transform.position;
                    //_CreaturesCaught[i].transform.localScale = Vector3.one;
                    _CreaturesCaught[i].gameObject.SetActive(true);
                    if (_CreaturesCaught[i].gameObject.GetComponent<NavMeshAgent>())
                        _CreaturesCaught[i].gameObject.GetComponent<NavMeshAgent>().enabled = true;

                    CreatureObjectPooling cob = _CreaturesCaught[i].gameObject.GetComponent<CreatureObjectPooling>();
                    if (cob)
                    {
                        if (cob._CreatureTeleports)
                            cob._ChooseNewLocation = true;
                    }

                    /*SummonAtDoorHandler sdh = _CreaturesCaught[i].gameObject.GetComponent<SummonAtDoorHandler>();
                    if (sdh)
                    {
                        sdh._ChooseNewLocation = true;
                        _CreaturesCaught[i]._CanBeTrappedCurrently = false;
                        _CreaturesCaught[i].inRangeOfTrap = false;
                        _CreaturesCaught[i]._CreatureAnimator.SetBool("Escaping", true);
                    }
                    else*/
                    _CreaturesCaught[i]._AutoCapture = false;

                    /*KnightTeleport KT = _CreaturesCaught[i].gameObject.GetComponent<KnightTeleport>();
                    if (KT)
                    {
                        _CreaturesCaught[i].gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    }*/

                    _CreaturesCaught[i].gameObject.GetComponent<Collider>().enabled = true;
                    _CreaturesCaught[i].gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    //_CreaturesCaught[i].GetComponent<BehaviourTreeRunner>().enabled = true;
                    



                    _CreaturesCaught.Remove(_CreaturesCaught[i]);
                    i--;
                }


                //RetrieveTrap();
            }

            

        }
        else if (!_ToolPlaced)
        {
            _CanCatchGhosts = false;
            t += Time.deltaTime;

            //float distCovered = (Time.time - startTime) * _PlacingInterpolationSpeed;
            //float fractionOfJourney = distCovered / journeyLength;

            //_BarrelAnimator.SetBool("Placed", true);
            //_OutlineBarrelAnimator.SetBool("Placed", true);


            //this.transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(2, 2, 2), t);


            //this.transform.GetChild(0).transform.right = Vector3.Lerp(this.transform.GetChild(0).transform.right, _FinalFor, t);
            this.transform.up = Vector3.Lerp(this.transform.up, _FinalNorm, t);



            this.transform.position = Vector3.Lerp(this.transform.position, _FinalPos, t);


            if (Vector3.Distance(this.transform.position, _FinalPos) <= 0.1f)
            {
                _ToolPlaced = true;

               

                if (playerControl._CurrentTool == this)
                    Equip();


                //this.GetComponent<CapsuleCollider>().enabled = true;

                for (int i = 0; i < _Colliders.Length; i++)
                {
                    _Colliders[i].enabled = true;
                }

                //if (_VisualTrapRange.Count > 0)
                //_VisualTrapRange[0].transform.parent.gameObject.SetActive(true);

                _TrapAnimator.SetBool("Landed", true);

            }
        }
        else
            _CanCatchGhosts = false;

        if (_Countdown.transform.parent.gameObject.activeSelf == true)
        {
            _Countdown.transform.parent.LookAt(Camera.main.transform);
        }



        
    }

    void CantCatchTween()
    {
        Sequence sequence = DOTween.Sequence();
        //sequence.Append(_Creature.transform.DOPath(path, _PathTravelTime));
        sequence.Append(_TrapAnimator.gameObject.transform.DOPunchRotation(new Vector3(0, 25, 0), 0.1f));
        sequence.Append(_TrapAnimator.gameObject.transform.DOPunchRotation(new Vector3(0, -25, 0), 0.1f).OnComplete(() => _Tweening = false));


        sequence.Play();

        _CanGetNewGhosts = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CreatureBrain>() != null && _CanGetNewGhosts)
        {
            CreatureBrain crea = other.GetComponent<CreatureBrain>();

            _CreatureInRange = true;
            crea.inRangeOfTrap = true;
            _CreaturesInRange.Add(crea);
            /*if (!crea._AutoCapture)
            {
                //_CreatureInRange = true;
                _CreaturesInRange.Add(crea);
                
            }*/
            if (crea._AutoCapture && crea._CanBeTrappedCurrently && crea.transform.GetComponentInChildren<PhysicsPlayerController>() != true)
            {
                

                _CanGetNewGhosts = false;
               // _CreatureInRange = true;
                _CreaturesInRange.Clear();
                _CreaturesInRange.Add(crea);

                crea.GetComponent<Collider>().enabled = false;
                //crea.inRangeOfTrap = true;

                _TrapLineRenderer._InUse = true;
                _TrapLineRenderer._Creature = _CreaturesInRange[0];
                _TrapLineRenderer._LineRenderer.enabled = true;
                playerControl.transform.parent = null;

                foreach (GameObject g in _VisualTrapRange)
                {
                    //g.GetComponent<MeshRenderer>().material = _GreyMat;
                    //g.SetActive(false);
                    foreach (ParticleSystem r in g.GetComponentsInChildren<ParticleSystem>())
                    {
                        var main = r.main;
                        main.loop = false;
                        var emission = r.emission;
                        emission.rateOverTime = 0;
                    }

                }
            }
        }
        else if (other.GetComponent<PhysicsPlayerController>())
        {
            _LogoCanvas.gameObject.SetActive(false);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<CreatureBrain>())
        {
            
            if (_CanGetNewGhosts && !other.GetComponent<CreatureBrain>()._AutoCapture)
            {
                //_BlockTrap = false;
                _CreatureInRange = true;
            }
            else if (_CanGetNewGhosts && other.GetComponent<CreatureBrain>()._AutoCapture && !other.GetComponent<CreatureBrain>()._CanBeTrappedCurrently)
            {
                _CreatureInRange = true;
                /*foreach (GameObject g in _VisualTrapRange)
                {
                    g.GetComponentInChildren<Renderer>().material = _RedMat;
                    //_BlockTrap = true;
                }*/
            }
            //else
                //_BlockTrap = false;

        }
        else if (other.GetComponent<PhysicsPlayerController>())
        {
            //_BlockTrap = false;
            _LogoCanvas.gameObject.SetActive(false);
        }
        //else
            //_BlockTrap = false;

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CreatureBrain>() != null && _CanGetNewGhosts)
        {
            _CreatureInRange = false;
            other.GetComponent<CreatureBrain>().inRangeOfTrap = false;
            _CreaturesInRange.Remove(other.GetComponent<CreatureBrain>());
        }
        else if (other.GetComponent<PhysicsPlayerController>())
        {
            _LogoCanvas.gameObject.SetActive(true);
        }
    }

    void NewInputSetup()
    {
        _PlayerInput.actions["PrimaryAction"].performed += DoPrimaryAction;
        _PlayerInput.actions["PrimaryAction"].Enable();

        _PlayerInput.actions["Interact"].performed += DoInteractAction;
        _PlayerInput.actions["Interact"].Enable();

        _PlayerInput.actions["Unequip"].performed += DoUnequipAction;
        _PlayerInput.actions["Unequip"].Enable();

    }

    void NewInputUnSetup()
    {
        _PlayerInput.actions["PrimaryAction"].performed -= DoPrimaryAction;
        _PlayerInput.actions["PrimaryAction"].Disable();

        //_PlayerInput.actions["Interact"].performed -= DoInteractAction;
        //_PlayerInput.actions["Interact"].Disable();

        _PlayerInput.actions["Unequip"].performed -= DoUnequipAction;
        _PlayerInput.actions["Unequip"].Disable();
    }


    private void DoInteractAction(InputAction.CallbackContext obj)
    {
        if (_ToolPlaced)
        {
            RaycastHit hit;
            if (Physics.SphereCast(Camera.main.transform.position - (Camera.main.transform.forward * 2), 2, Camera.main.transform.forward, out hit, 4, _ToolPickupLayerMask))
            {
                if (hit.transform.root.GetComponent<TrapTool>() != null && playerControl._HoldingTool == false && !_TrapLineRenderer._InUse)
                {
                    RetrieveTrap();
                }
                
            }
        }
    }
    private void DoPrimaryAction(InputAction.CallbackContext obj)
    {
        bool _NoClones = true;
        if (_CanCatchGhosts)
        {
            

            _CanGetNewGhosts = false;
            for (int i = 0; i < _CreaturesInRange.Count; i++)
            {
                CloneBrain clone = _CreaturesInRange[i].GetComponent<CloneBrain>();
                if (clone)
                {
                    _NoClones = false;

                    int inactiveclones = 0;

                    for (int j = 0; j < clone._Pooler.pooledCreatures.Count; j++)
                    {
                        clone._Pooler.pooledCreatures[j].GetComponent<CreatureBrain>()._RunRandomly = false;

                        if (clone._Pooler.pooledCreatures[j].gameObject.activeSelf == false || clone._Pooler.pooledCreatures[j].GetComponent<CloneBrain>()._BeingSetInactive)
                            inactiveclones++;
                    }

                    if (inactiveclones >= 2)
                    {
                        clone._Pooler.gameObject.GetComponent<CreatureBrain>()._HasActiveClones = false;
                        clone._Pooler.gameObject.GetComponent<CreatureBrain>()._RunRandomly = false;

                        if (clone._Pooler._CreatureTeleports)
                            clone._Pooler._ChooseNewLocation = true;
                    }


                    //_CreaturesInRange[i].GetComponent<ThrowObjectComponent>()._IgnoreAttack = true;

                    //_CreaturesInRange[i].gameObject.SetActive(false);
                    clone.GetComponent<NavMeshAgent>().enabled = false;
                    //clone.GetComponent<BehaviourTreeRunner>().enabled = false;
                    clone.RemoveClone();
                    _CreaturesInRange.RemoveAt(i);
                    i--;
                }
            }
            if (_NoClones)
            {

                _CreaturesInRange[0].GetComponent<Collider>().enabled = false;
                _TrapLineRenderer._InUse = true;
                _TrapLineRenderer._Creature = _CreaturesInRange[0];
                _TrapLineRenderer._LineRenderer.enabled = true;
                playerControl.transform.parent = null;
                //CatchCreatures(); ///////////////////////////////////
            }
            else if (!_Tweening)
            {


                _Tweening = true;

                CantCatchTween();
            }
        }

        if (this != null)
        {
            if (CanCatch && playerControl._CurrentTool == this.gameObject.GetComponent<TrapTool>())
            {
                if (_CreaturesInRange.Count == 0 && _NoClones)
                    _TrapAnimator.SetBool("Capturing", true);

                foreach (GameObject g in _VisualTrapRange)
                {
                    //g.GetComponent<MeshRenderer>().material = _GreyMat;
                    //g.SetActive(false);
                    foreach (ParticleSystem r in g.GetComponentsInChildren<ParticleSystem>())
                    {
                        var main = r.main;
                        main.loop = false;
                        var emission = r.emission;
                        emission.rateOverTime = 0;
                    }
                }
            }
            else if (!CanCatch && !_Tweening)
            {
                _Tweening = true;

                CantCatchTween();
            }
        }
    }

    private void DoUnequipAction(InputAction.CallbackContext obj)
    {
        if (_ToolPlaced && playerControl._PlayerState == PlayerStates.PlayState)
        {
            if (playerControl._CurrentTool == this.gameObject.GetComponent<TrapTool>() && _CreaturesCaught.Count == 0 && !_TrapLineRenderer._InUse)
            {
                NewInputUnSetup();

                _ToolManager.UnequipTools();

                _ToolPlaced = false;
                _ToolManager._CurTrapTool = null;
                playerControl._CurrentTool = null;

                //_HandHeldTrap._ToolSlot.TweenDown();
                //_HandHeldTrap._ToolSlot.PopArrow();
               

                Destroy(this.gameObject);
            }
        }
    }
}
