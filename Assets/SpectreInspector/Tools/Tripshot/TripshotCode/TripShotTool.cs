using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TripShotTool : PlaceableTool
{
    Vector3 _HitNormal;
    bool _RaycastHit;
    bool _SuitableLocation;

    public Vector3 _PlungerFinalNormal;
    [HideInInspector]
    public bool _PlungerFinalNormalBool;

    // Max Distance the Tripshot can initially shoot at (Currently can be pulled further)
    public float _TripShotReach;

    Color _StartColour;
    Color _EndColour;
    [HideInInspector]
    public LineRenderer _LineRenderer;

    // TripShot Children
    public Transform _Barrel;
    public Transform _BarrelLaunchPoint;
    public Transform _BarrelBase;
    public Transform _PlungerFinalPosTransform;
    public MeshRenderer _PlungerFinalPosSphere;

    // Handling Drawing of Path
    Vector3 _InitialVel;
    float _SimTime;

    float _Gravity = Physics.gravity.y;

    // Handling Aiming the 
    Vector3 _DirOfAim;
    Vector3 _FirstPointOfTraj;
    bool _TripShotRotating;

    [SerializeField] private float _RadiansSpeed;
    [SerializeField] private float _DeltaSpeed;
    [SerializeField] private float _InterpolationSpeed;

    public bool _PlungerShot;
    public GameObject _Plunger;
    public PlungerComponent _PlungerScript;
    [SerializeField] private GameObject _PlungerPrefab;
    public Vector3 _PlungerFinalPos;

    [HideInInspector]
    public Vector3 _PlungerFlightPathMiddlePoint;

    [HideInInspector]
    public HandHeldTripShot _HandHeldTripShot;

    public AudioClip[] _Shooting;
    public AudioSource _source;

    [SerializeField] private LayerMask _PlayerRaycast;
    [SerializeField] private LayerMask _LineRendererRaycast;
    [SerializeField] private LayerMask _ToolPickupLayerMask;

    public Animator _LauncherAnimator;
    public Animator _OutlineLauncherAnimator;
    public Animator _BarrelAnimator;
    public Animator _OutlineBarrelAnimator;

    [SerializeField] private List<ParticleSystem> _LaunchParticles = new List<ParticleSystem>();
    [SerializeField] private Transform _SmokeRisingParticle;

    [HideInInspector]
    public Vector3 _FinalPos;

    [HideInInspector]
    public Vector3 _FinalNorm;

    [HideInInspector]
    public Vector3 _FinalFor;

    float t = 0;

    [HideInInspector]
    public PhysicsPlayerController _PlayerControl;
    [HideInInspector]
    public PlayerInput _PlayerInput;

    float startTime;
    float journeyLength;
    [SerializeField] private float _PlacingInterpolationSpeed = 1;

    [SerializeField] private float _TripshotMaxDist = 50;

    [SerializeField] private Canvas _LogoCanvas;

    private void OnEnable()
    {
        NewInputSetup();
    }

    private void Awake()
    {
        if (_PlayerControl == null)
            _PlayerControl = FindObjectOfType<PhysicsPlayerController>();
        if (_PlayerInput == null)
            _PlayerInput = _PlayerControl._PlayerInput;
        _LineRenderer = GetComponent<LineRenderer>();
        _StartColour = new Color(_LineRenderer.startColor.r, _LineRenderer.startColor.g, _LineRenderer.startColor.b, 0);
        _EndColour = new Color(_LineRenderer.endColor.r, _LineRenderer.endColor.g, _LineRenderer.endColor.b, 0);
        _LineRenderer.startColor = _StartColour;
        _LineRenderer.endColor = _EndColour;
    }

    public override void Equip()
    {
        base.Equip();
    }

    public override void DetermineToolSwap()
    {
        base.DetermineToolSwap();
    }

    public override void ReturnTool()
    {
        NewInputUnSetup();

        ToolManager.instance.UnequipTools();

        _ToolPlaced = false;
        ToolManager.instance._CurTripShot = null;
        ToolManager.instance._CurrentTool = null;

        _HandHeldTripShot._ToolSlot.TweenDown();
        _HandHeldTripShot._ToolSlot.PopArrow();

        PlungerDestroy();

        Destroy(this.gameObject);
    }

    void UpdateToolOutline()
    {
        if (_ToolPlaced)
        {
            RaycastHit hit;

            if (Physics.SphereCast(Camera.main.transform.position - (Camera.main.transform.forward * 2), 2, Camera.main.transform.forward, out hit, 4, _ToolPickupLayerMask))
            {
                TripShotTool tripShotTool;
                if (hit.transform.TryGetComponent<TripShotTool>(out tripShotTool) == true && ToolManager.instance._HoldingTool == false)
                {
                    if (_ToolOutlines[0].activeSelf == false)
                    {
                        foreach (GameObject outline in _ToolOutlines)
                        {
                            outline.SetActive(true);
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
                }
            }
        }
    }

    public void Update()
    {
        if (_ToolPlaced && Camera.main.transform.parent.GetComponent<PhysicsPlayerController>()._PlayerState == PlayerStates.PlayState)
        {
            if (ToolManager.instance._CurrentTool == this.gameObject.GetComponent<TripShotTool>())
            {
                AimTripShot();
                Vector3 newDirection = Vector3.RotateTowards(_Barrel.forward, _DirOfAim, _RadiansSpeed, _DeltaSpeed);

                if (_TripShotRotating)
                {
                    _Barrel.rotation = Quaternion.Slerp(_Barrel.rotation, Quaternion.LookRotation(newDirection), _InterpolationSpeed);

                    if (_Barrel.rotation == Quaternion.LookRotation(newDirection))
                        _TripShotRotating = false;

                }

                if (_PlungerShot)
                {
                    _TripShotRotating = false;

                    if (Vector3.Distance(_Barrel.position, _PlungerScript.m_PlungerTail.position) > 1)
                    {
                        _Barrel.rotation = _PlungerScript.TripShotRope.transform.rotation;
                    }
                    else if (!_PlungerScript.m_NotInPlace)
                        _Barrel.rotation = _PlungerScript.TripShotRope.transform.rotation;

                    _SmokeRisingParticle.transform.forward = Vector3.up;
                }
            }

            UpdateToolOutline(); 
        }
        else if (!_ToolPlaced)
        {
            t += Time.deltaTime;

            _BarrelAnimator.SetBool("Placed", true);
            _OutlineBarrelAnimator.SetBool("Placed", true);

            this.transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(2,2,2), t);

            this.transform.up = Vector3.Lerp(this.transform.up, _FinalNorm, t);

            this.transform.position = Vector3.Lerp(this.transform.position, _FinalPos, t);

            if (Vector3.Distance(this.transform.position, _FinalPos) <= 0.25f)
            {
                _ToolPlaced = true;

                Equip();

                this.GetComponent<CapsuleCollider>().enabled = true;
            }
        }
    }

    void AimTripShot()
    {
        if (_PlayerInput.actions["SecondaryAction"].IsPressed())
        {
            RaycastHit hit;
            LayerMask tripShotLayer = ~(1 << 6);
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1000, _PlayerRaycast))
            {
                // If aim spot is within max range set aim spot there otherwise set it as far of possible in that direction
                if (Vector3.Distance(this.transform.position, hit.point) <= _TripshotMaxDist)
                {
                    _PlungerFinalPosTransform.position = hit.point;
                }
                else
                {
                    Vector3 hitPoint = hit.point;
                    hitPoint.y = this.transform.position.y;

                    Vector3 directionOfTravel = hitPoint - this.transform.position;

                    Vector3 finalDirection = directionOfTravel.normalized * _TripshotMaxDist;

                    Vector3 targetPosition = this.transform.position + finalDirection;

                    _PlungerFinalPosTransform.position = targetPosition;
                }

                _HitNormal = hit.normal;
                _RaycastHit = true;
                _SuitableLocation = true;

                _PlungerFinalNormal = hit.normal;
                _PlungerFinalNormalBool = true;

                _StartColour = new Color(_LineRenderer.startColor.r, _LineRenderer.startColor.g, _LineRenderer.startColor.b, 1);
                _EndColour = new Color(_LineRenderer.endColor.r, _LineRenderer.endColor.g, _LineRenderer.endColor.b, 1);
                _LineRenderer.startColor = _StartColour;
                _LineRenderer.endColor = _EndColour;
                _PlungerFinalPosSphere.enabled = true;
                DrawTripShotPath();
            }
            else
            {
                Vector3 newPos = Camera.main.transform.position + Camera.main.transform.forward * (1000);
                newPos.y = this.transform.position.y;

                Vector3 directionOfTravel = newPos - this.transform.position;

                Vector3 finalDirection = directionOfTravel.normalized * _TripshotMaxDist;

                Vector3 targetPosition = this.transform.position + finalDirection;

                _PlungerFinalPosTransform.position = targetPosition;

                RaycastHit hit2;

                if (Physics.Raycast(_PlungerFinalPosTransform.position, -Vector3.up, out hit2, 1000, _PlayerRaycast))
                {
                    RaycastHit hit3;

                    Vector3 posDir = hit2.point - this.transform.position;
                    if (Physics.Raycast(this.transform.position, posDir.normalized, out hit3, 1000, _PlayerRaycast))
                    {
                        _HitNormal = hit3.normal;
                        _RaycastHit = true;
                        _SuitableLocation = true;

                        _PlungerFinalNormal = hit3.normal;
                        _PlungerFinalNormalBool = true;

                        _StartColour = new Color(_LineRenderer.startColor.r, _LineRenderer.startColor.g, _LineRenderer.startColor.b, 1);
                        _EndColour = new Color(_LineRenderer.endColor.r, _LineRenderer.endColor.g, _LineRenderer.endColor.b, 1);
                        _LineRenderer.startColor = _StartColour;
                        _LineRenderer.endColor = _EndColour;
                        _PlungerFinalPosSphere.enabled = true;
                        DrawTripShotPath();
                    }
                }
            }
        }
        else
        {
            _StartColour = new Color(_LineRenderer.startColor.r, _LineRenderer.startColor.g, _LineRenderer.startColor.b, 0);
            _EndColour = new Color(_LineRenderer.endColor.r, _LineRenderer.endColor.g, _LineRenderer.endColor.b, 0);
            _LineRenderer.startColor = _StartColour;
            _LineRenderer.endColor = _EndColour;
            _PlungerFinalPosSphere.enabled = false;
        }
    }

    public void DrawTripShotPath()
    {
        if (_SuitableLocation)
        {
            Vector3 launchpos = _BarrelLaunchPoint.position;

            Vector3 previousDrawPoint = launchpos;

            int resolution = 50 + (int)(Vector3.Distance(this.transform.position, _PlungerFinalPosTransform.position) * 8f);
            _LineRenderer.positionCount = resolution;

            for (int i = 0; i < resolution - 1; i++)
            {
                float simulationTime = i / (float)resolution;

                float yValue = (launchpos.y + Vector3.Distance(launchpos, _PlungerFinalPosTransform.position) / 2) - 1;

                if (yValue < launchpos.y + ((_PlungerFinalPosTransform.position.y - launchpos.y) / 2))
                    yValue = launchpos.y + ((_PlungerFinalPosTransform.position.y - launchpos.y) / 2);

                Vector3 middlePos = new Vector3(launchpos.x + ((_PlungerFinalPosTransform.position.x - launchpos.x) / 2), yValue, launchpos.z + ((_PlungerFinalPosTransform.position.z - launchpos.z) / 2));

                _PlungerFlightPathMiddlePoint = middlePos;

                Vector3 drawPoint = QuadraticLerp(launchpos, middlePos, _PlungerFinalPosTransform.position, simulationTime);

                _LineRenderer.SetPosition(i, previousDrawPoint);
                _LineRenderer.SetPosition(i + 1, drawPoint);

                if (i == 0)
                {
                    Vector3 newPos = QuadraticLerp(launchpos, _PlungerFlightPathMiddlePoint, _PlungerFinalPosTransform.position, 0);
                    Vector3 newPos1 = QuadraticLerp(launchpos, _PlungerFlightPathMiddlePoint, _PlungerFinalPosTransform.position, 0 + (Time.deltaTime));

                    _DirOfAim = newPos1 - newPos;
                    _TripShotRotating = true;
                }

                // RayCast cuts the path early if it collides with another object
                RaycastHit hit;

                if (Physics.Raycast(previousDrawPoint, drawPoint - previousDrawPoint, out hit, Vector3.Distance(drawPoint, previousDrawPoint), _PlayerRaycast))
                {
                    _LineRenderer.positionCount = i;
                    //_RecordLineRenderPos = false;
                    _PlungerFinalPosTransform.position = hit.point;
                    _PlungerFinalNormal = hit.normal;
                    _PlungerFinalNormalBool = true;
                    return;
                }

                previousDrawPoint = drawPoint;
            }
        }
    }
    Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, t);
    }

    public Vector3 MovePlunger(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        return QuadraticLerp(a, b, c, t);
    }

    public void PlungerDestroy()
    {
        if (_Plunger)
            Destroy(_Plunger);
        _PlungerShot = false;

        if (_PlungerScript)
        {
            if (_PlungerScript.TripShotRope)
                Destroy(_PlungerScript.TripShotRope);
        }

        _PlungerScript = null;

        if (_LauncherAnimator)
            _LauncherAnimator.SetBool("FirePlunger", false);
        if (_OutlineLauncherAnimator)
            _OutlineLauncherAnimator.SetBool("FirePlunger", false);
    }

    void NewInputSetup()
    {
        _PlayerInput.actions["PrimaryAction"].performed += DoFireAction;
        _PlayerInput.actions["PrimaryAction"].Enable();

        _PlayerInput.actions["Interact"].performed += DoPickUpAction;
        _PlayerInput.actions["Interact"].Enable();

        _PlayerInput.actions["Unequip"].performed += DoUnequipAction;
        _PlayerInput.actions["Unequip"].Enable();
    }

    void NewInputUnSetup()
    {
        _PlayerInput.actions["PrimaryAction"].performed -= DoFireAction;
        _PlayerInput.actions["PrimaryAction"].Disable();

        //_PlayerInput.actions["Interact"].performed -= DoPickUpAction;
        //_PlayerInput.actions["Interact"].Disable();

        _PlayerInput.actions["Unequip"].performed -= DoUnequipAction;
        _PlayerInput.actions["Unequip"].Disable();
    }

    private void DoFireAction(InputAction.CallbackContext obj)
    {
        if (_ToolPlaced && Camera.main.transform.parent.GetComponent<PhysicsPlayerController>()._PlayerState == PlayerStates.PlayState && ToolManager.instance._CurrentTool == this.gameObject.GetComponent<TripShotTool>())
        {
            if (_SuitableLocation && !_PlungerShot)
            {
                _LauncherAnimator.SetBool("FirePlunger", true);
                _OutlineLauncherAnimator.SetBool("FirePlunger", true);

                _Plunger = Instantiate(_PlungerPrefab, _BarrelLaunchPoint);
                _Plunger.transform.parent = null;

                _PlungerScript = _Plunger.GetComponent<PlungerComponent>();

                _PlungerShot = true;
                Physics.IgnoreCollision(_Plunger.GetComponentInChildren<BoxCollider>(), GetComponent<CapsuleCollider>());
                _PlungerFinalPos = _PlungerFinalPosTransform.position;

                AudioClip audioClip = _Shooting[Random.Range(0, _Shooting.Length - 1)];
                _source.clip = audioClip;
                _source.pitch = Random.Range(1, 1.25f);
                _source.Play();

                _SmokeRisingParticle.transform.forward = Vector3.up;

                for (int i = 0; i < _LaunchParticles.Count; i++)
                {
                    _LaunchParticles[i].Play();
                }
            }
        }
    }

    private void DoPickUpAction(InputAction.CallbackContext obj)
    {
        if (_ToolPlaced)
        {
            RaycastHit hit;

            if (Physics.SphereCast(Camera.main.transform.position - (Camera.main.transform.forward * 2), 2, Camera.main.transform.forward, out hit, 4, _ToolPickupLayerMask, QueryTriggerInteraction.Ignore))
            {
                TripShotTool tripShotTool;

                if (hit.transform.TryGetComponent<TripShotTool>(out tripShotTool) == true && ToolManager.instance._HoldingTool == false)
                {
                    NewInputUnSetup();

                    if (_PlungerShot)
                    {
                        Destroy(_Plunger.GetComponent<PlungerComponent>().TripShotRope);
                        _PlungerShot = false;
                        Destroy(_Plunger);
                    }

                    ToolManager.instance.UnequipTools();

                    _ToolPlaced = false;
                    ToolManager.instance._CurTripShot = null;
                    ToolManager.instance._CurrentTool = null;

                    if (_HandHeldTripShot.gameObject.activeSelf == true)
                    {
                        _HandHeldTripShot.CallEnable();
                    }
                    else
                    {
                        _HandHeldTripShot.gameObject.SetActive(true);
                    }

                    _HandHeldTripShot._ToolAnimator.SetBool("Placing", false);
                    _HandHeldTripShot._ToolAnimator.SetBool("PickingUp", true);
                    _HandHeldTripShot._ToolSelected = true;

                    Destroy(this.gameObject); 
                }
            }
        }
    }

    private void DoUnequipAction(InputAction.CallbackContext obj)
    {
        if (ToolManager.instance._CurrentTool == this)
            ReturnTool();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PhysicsPlayerController>())
        {
            _LogoCanvas.gameObject.SetActive(false);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PhysicsPlayerController>())
        {
            _LogoCanvas.gameObject.SetActive(true);
        }
    }
}
