using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class TrapTool : PlaceableTool
{
    [HideInInspector]
    public HandHeldTrap _HandHeldTrap;

    public List<GameObject> _VisualTrapRange = new List<GameObject>();

    bool _CreatureInRange;

    [SerializeField] private Material _BlueMat;
    [SerializeField] private Material _RedMat;
    [SerializeField] private Material _GreenMat;

    [HideInInspector]
    public List<CreatureBrain> _CreaturesInRange = new List<CreatureBrain>();
    [HideInInspector]
    public List<CreatureBrain> _CreaturesCaught = new List<CreatureBrain>();

    bool _CreaturesCaptured;
    bool _TweenIsPlaying;
    [SerializeField] private float _TimeTillEscape = 10;
    float _TimeTillEscapeRememberer;

    [SerializeField] private TextMeshProUGUI _Countdown;

    [SerializeField] private Image _CountdownSprite;
    [SerializeField] private Sprite[] _CountdownSprites;

    public Animator _TrapAnimator;

    [HideInInspector]
    public Vector3 _FinalPos;

    [HideInInspector]
    public Vector3 _FinalNorm;

    float t = 0;

    [SerializeField] private Collider[] _Colliders;

    [SerializeField] private LayerMask _ToolPickupLayerMask;

    public TrapLineRendererManager _TrapLineRenderer;

    bool _Tweening;

    [HideInInspector]
    public PhysicsPlayerController playerControl;
    public PlayerInput _PlayerInput;

    bool _CanGetNewGhosts = true;
    [SerializeField] private Canvas _LogoCanvas;

    bool _BlockTrap;

    bool _CanCatchGhosts;
    bool CanCatch;

    [SerializeField] private CaughtPrefabComponent CaughtCreatureBanner;

    private void OnEnable()
    {
        _TimeTillEscapeRememberer = _TimeTillEscape;

        if (_PlayerInput == null)
        {
            _PlayerInput = playerControl._PlayerInput;
        }

        NewInputSetup();
    }

    public void CallEnable()
    {
        OnEnable();
    }

    private void Awake()
    {
        if (playerControl == null)
            playerControl = FindObjectOfType<PhysicsPlayerController>();

        CaughtCreatureBanner = FindObjectOfType<CaughtPrefabComponent>(true);
    }

    public void RemoveTrap()
    {
        _ToolPlaced = false;
        ToolManager.instance._CurTrapTool = null;

        NewInputUnSetup();

        Destroy(this.gameObject);
    }

    public void OutlineHandler()
    {
        if (_ToolPlaced)
        {
            RaycastHit hit;

            if (Physics.SphereCast(Camera.main.transform.position - (Camera.main.transform.forward * 2), 2, Camera.main.transform.forward, out hit, 4, _ToolPickupLayerMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.root.GetComponent<TrapTool>() != null && ToolManager.instance._HoldingTool == false && !_TrapLineRenderer._InUse)
                {
                    if (_ToolOutlines[0].activeSelf == false)
                    {
                        foreach (GameObject outline in _ToolOutlines)
                        {
                            outline.SetActive(true);
                        }

                        _LogoCanvas.gameObject.SetActive(false);
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
        // Sets up the banner informing player they caught creature
        for (int i = 0; i < _CreaturesCaught.Count;)
        {
            CaughtCreatureBanner.SetCreature(_CreaturesCaught[i]._CreatureIdentity._Sprite, _CreaturesCaught[i]._CreatureIdentity._NamePlate);

            CaughtCreatureBanner._CreatureImage.sprite = _CreaturesCaught[i]._CreatureIdentity._Sprite;
            CaughtCreatureBanner._CreatureBannerActive = true;

            CaughtCreatureBanner.gameObject.SetActive(true);

            _CreaturesCaught[i]._CurRoom._RoomInUse = false;

            GameObject crea = _CreaturesCaught[i].gameObject;
            _CreaturesCaught.Remove(_CreaturesCaught[i]);
            Destroy(crea);
        }
        
        // If player isn't holding a tool return trap to hands, otherwise just remove the trap
        if (!ToolManager.instance._HoldingTool)
        {
            NewInputUnSetup();

            ToolManager.instance.UnequipTools();
            ToolManager.instance._CurTrapTool = null;

            ToolManager.instance._CurrentTool = null;

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
        }

        ToolManager.instance._CurTrapTool = null;
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

            // Removes Clone Creatures when you catch the real Creature
            CreatureObjectPooling _pooledCrea = _CreaturesInRange[i].GetComponent<CreatureObjectPooling>();

            if (_pooledCrea)
            {
                for (int k = 0; k < _pooledCrea.pooledCreatures.Count; k++)
                {
                    _pooledCrea.pooledCreatures[k].GetComponent<NavMeshAgent>().enabled = false;
                    _pooledCrea.pooledCreatures[k].GetComponent<BehaviourTreeRunner>().enabled = false;
                    _pooledCrea.pooledCreatures[k].GetComponent<CloneBrain>().RemoveClone();

                }
                _CreaturesInRange[i]._HasActiveClones = false;
                _CreaturesInRange[i]._RunRandomly = false;
            }

            SpawnTrackingComponent _SPC = _CreaturesInRange[i].GetComponent<SpawnTrackingComponent>();

            if (_SPC)
            {
                _SPC.RemoveAllTracks();
            }

            _CreaturesInRange[i].gameObject.SetActive(false);
            _CreaturesInRange.Remove(_CreaturesInRange[i]);

            i--;
        }
        _CreatureInRange = false;
        

        _Countdown.transform.parent.gameObject.SetActive(true);
    }

    void Update()
    {
        if (_ToolPlaced && playerControl._PlayerState == PlayerStates.PlayState)
        {
            PickUpTool();
            OutlineHandler();

            CanCatch = true;

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

            // Handle colour of radar from trap
            if (_CreatureInRange && !_CreaturesCaptured && CanCatch)
            {
                foreach (GameObject g in _VisualTrapRange)
                {
                    foreach (Renderer r in g.GetComponentsInChildren<Renderer>())
                    {
                        r.material = _GreenMat;
                    }
                }

                if (ToolManager.instance._CurrentTool == this.gameObject.GetComponent<TrapTool>())
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
            }
            else if (_TimeTillEscape <= 0)
            {
                _TimeTillEscape = _TimeTillEscapeRememberer;
                _CreaturesCaptured = false;

                _TrapAnimator.SetBool("Escape", true);


                for (int i = 0; i < _CreaturesCaught.Count; i++)
                {
                    _CreaturesCaught[i].transform.position = this.transform.position;
                    _CreaturesCaught[i].gameObject.SetActive(true);
                    if (_CreaturesCaught[i].gameObject.GetComponent<NavMeshAgent>())
                        _CreaturesCaught[i].gameObject.GetComponent<NavMeshAgent>().enabled = true;

                    // Tells Creature to teleport away after breaking out of trap
                    CreatureObjectPooling cob = _CreaturesCaught[i].gameObject.GetComponent<CreatureObjectPooling>();
                    if (cob)
                    {
                        if (cob._CreatureTeleports)
                            cob._ChooseNewLocation = true;
                    }

                    _CreaturesCaught[i]._AutoCapture = false;

                    _CreaturesCaught[i].gameObject.GetComponent<Collider>().enabled = true;
                    _CreaturesCaught[i].gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    _CreaturesCaught[i].GetComponent<BehaviourTreeRunner>().enabled = true;

                    _CreaturesCaught.Remove(_CreaturesCaught[i]);
                    i--;
                }
            }
        }
        else if (!_ToolPlaced)
        {
            _CanCatchGhosts = false;
            t += Time.deltaTime;

            this.transform.up = Vector3.Lerp(this.transform.up, _FinalNorm, t);

            this.transform.position = Vector3.Lerp(this.transform.position, _FinalPos, t);

            if (Vector3.Distance(this.transform.position, _FinalPos) <= 0.1f)
            {
                _ToolPlaced = true;

                if (ToolManager.instance._CurrentTool == this)
                    Equip();

                for (int i = 0; i < _Colliders.Length; i++)
                {
                    _Colliders[i].enabled = true;
                }

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

            if (crea._AutoCapture && crea._CanBeTrappedCurrently && crea.transform.GetComponentInChildren<PhysicsPlayerController>() != true)
            {
                _CanGetNewGhosts = false;

                _CreaturesInRange.Clear();
                _CreaturesInRange.Add(crea);

                crea.GetComponent<Collider>().enabled = false;

                // TrapLineRenderer now now tells the trap to catch after it finishes pulling in the creature
                _TrapLineRenderer._InUse = true;
                _TrapLineRenderer._Creature = _CreaturesInRange[0];
                _TrapLineRenderer._LineRenderer.enabled = true;
                playerControl.transform.parent = null;

                foreach (GameObject g in _VisualTrapRange)
                {
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
                _CreatureInRange = true;
            }
            else if (_CanGetNewGhosts && other.GetComponent<CreatureBrain>()._AutoCapture && !other.GetComponent<CreatureBrain>()._CanBeTrappedCurrently)
            {
                _CreatureInRange = true;
            }
        }
        else if (other.GetComponent<PhysicsPlayerController>())
        {
            _LogoCanvas.gameObject.SetActive(false);
        }
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
            if (Physics.SphereCast(Camera.main.transform.position - (Camera.main.transform.forward * 2), 2, Camera.main.transform.forward, out hit, 4, _ToolPickupLayerMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.root.GetComponent<TrapTool>() != null && ToolManager.instance._HoldingTool == false && !_TrapLineRenderer._InUse)
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
                // Removes clone from trap when trying to capture it
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

                    clone.GetComponent<NavMeshAgent>().enabled = false;
                    clone.GetComponent<BehaviourTreeRunner>().enabled = false;
                    clone.RemoveClone();
                    _CreaturesInRange.RemoveAt(i);
                    i--;
                }
            }
            if (_NoClones && _CreaturesInRange.Count > 0)
            {
                // TrapLineRenderer now now tells the trap to catch after it finishes pulling in the creature
                _CreaturesInRange[0].GetComponent<Collider>().enabled = false;
                _TrapLineRenderer._InUse = true;
                _TrapLineRenderer._Creature = _CreaturesInRange[0];
                _TrapLineRenderer._LineRenderer.enabled = true;
                playerControl.transform.parent = null;
            }
            else if (!_Tweening)
            {
                _Tweening = true;

                CantCatchTween();
            }
        }

        if (this != null)
        {
            if (CanCatch && ToolManager.instance._CurrentTool == this.gameObject.GetComponent<TrapTool>())
            {
                if (_CreaturesInRange.Count == 0 && _NoClones)
                    _TrapAnimator.SetBool("Capturing", true);

                foreach (GameObject g in _VisualTrapRange)
                {
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
            if (ToolManager.instance._CurrentTool == this && _CreaturesCaught.Count == 0 && !_TrapLineRenderer._InUse)
            {
                NewInputUnSetup();

                ToolManager.instance.UnequipTools();

                _ToolPlaced = false;
                ToolManager.instance._CurTrapTool = null;
                ToolManager.instance._CurrentTool = null;

                _HandHeldTrap._ToolSlot.TweenDown();
                _HandHeldTrap._ToolSlot.PopArrow();
               
                Destroy(this.gameObject);
            }
        }
    }
}
