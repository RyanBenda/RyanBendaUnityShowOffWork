using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraTool : HandheldTool
{
    public static CameraTool CameraInstance;

    public List<CreatureBrain> _Creatures;
    //public CompendiumButtonComponent[] _CompendiumButtons;

    public float _blipIntervals;
    public Canvas _radarPanel;
    public float _maxDistance = 10f;
    float timer = 0;

    public GameObject[] _CreaturesDot;

    public GameObject _player;
    public Camera _Cam;
    float _fov;

    public GameObject _spritePrefab;

    List<GameObject> Blips = new List<GameObject>();

    GameObject _TrackerTool;

    public GameObject _CameraToolUI;
    CameraToolUI _CameraToolUIScript;
    public GameObject _StandardUI;

    public GameObject _TrackerToolPrefab;
    public GameObject _UIPrefab;

    public AudioClip[] _OpeningClips;
    public AudioClip[] _ZoomInClips;
    public AudioClip[] _ZoomOutClips;
    public AudioSource _source;


    public GameObject _RaycastShoot;
    public PlayerInput _PlayerInput;

    public HotBarPos _ToolSlot;

    public Image _FadeToBlack;
    bool _FadingToBlack;
    bool _FadingToClear;
    float ToBlack;
    float ToClear;

    public float _TimeToBlack = 1;
    public float _TimeToClear = 1;
    public float _TimeIsBlack = 2;

    bool _RemovingCamera;

    bool _GogglesOn;

    public Animator _ToolAnimator;
    public GameObject[] _CameraObjects;

    bool _PreventSwap;

    public LayerMask _HitTargetLayermask;
    public LayerMask _GhostCheckLayermask;

    [Header("Photo Taker")]
    [SerializeField] private Image PhotoDisplayArea;
    [SerializeField] private PhotoPopInEffect photoFrame;

    private Texture2D[] screenCaptures = new Texture2D[16];
    //private Texture2D screenCapture2;
    int screenCapturesIndex = 0;

    private bool viewingPhoto;

    public GameObject[] _UiToHide;

    public GameObject _TooltipUI;
    public GameObject _CrosshairUI;

    public CameraPhotosPage _PhotoPage;

    public Plane[] planes;

    PhysicsPlayerController _PlayerControl;

    private void Awake()
    {

        for (int i = 0; i < screenCaptures.Length; i++)
        {
            screenCaptures[i] = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        }

        _CameraToolUIScript = _CameraToolUI.GetComponent<CameraToolUI>();

        if (photoFrame == null)
            photoFrame = FindObjectOfType<PhotoPopInEffect>(true);

        if (_PhotoPage == null)
            _PhotoPage = FindObjectOfType<CameraPhotosPage>(true);

        CameraInstance = this;

        _PlayerInput = FindObjectOfType<PlayerInput>();


        this.gameObject.SetActive(false);   
    }

    void Start()
    {
        _Cam = Camera.main;
        _fov = _Cam.fieldOfView;
    }

    private void OnEnable()
    {
        if (_PlayerControl == null)
            _PlayerControl = Camera.main.transform.parent.GetComponent<PhysicsPlayerController>();

        bool _delayToolSwap = false;
        if (ToolManager.instance._CurrentTool != null && ToolManager.instance._CurrentTool != this)
        {
            ToolManager.instance._CurrentTool._SwappingTool = this;

            if (ToolManager.instance._CurrentTool.GetComponent<HandHeldTrap>() || ToolManager.instance._CurrentTool.GetComponent<GoggleTool>())
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
            ToolManager.instance._CurrentTool = this;
            ToolManager.instance._HoldingTool = true;
            _ToolSelected = true;

            base.Equip();

            foreach (Tool tool in ToolManager.instance._HandToolsList)
            {
                if (tool != null & tool != this)
                    tool.gameObject.SetActive(false);
            }

            _ToolSlot.TweenUp();
            _ToolSlot.MoveArrow(3);

            _ToolAnimator.SetBool("Equipping", true);

            _PlayerInput.actions["PrimaryAction"].performed += DoSnap;
            _PlayerInput.actions["PrimaryAction"].Enable();

            _PlayerInput.actions["Unequip"].performed += DoUnequip;
            _PlayerInput.actions["Unequip"].Enable();
        }
        else
        {
            _PreventSwap = false;
        }

    }

    void NewInputSetup()
    {
        _PlayerInput.actions["PrimaryAction"].performed += DoSnap;
        _PlayerInput.actions["PrimaryAction"].Enable();

        _PlayerInput.actions["Unequip"].performed += DoUnequip;
        _PlayerInput.actions["Unequip"].Enable();
    }

    public void BeginFadeToBlack()
    {
        _FadingToBlack = true;
        ToBlack = 0;
        ToClear = 0;
        _RemovingCamera = false;
        _FadeToBlack.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_GogglesOn)
        {
            _CameraToolUIScript._Inuse = true;
            if (!_RemovingCamera)
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
                        _CameraToolUI.SetActive(true);
                        _StandardUI.SetActive(false);

                        for (int i = 0; i < _CameraObjects.Length; i++)
                        {
                            _CameraObjects[i].SetActive(false);
                        }
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
                        _GogglesOn = true;
                        _CameraToolUIScript._Inuse = false;
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

                        _CameraToolUI.gameObject.SetActive(false);
                        _StandardUI.SetActive(true);

                        for (int i = 0; i < _CameraObjects.Length; i++)
                        {
                            _CameraObjects[i].SetActive(true);
                        }

                        _ToolAnimator.SetBool("Unequipping", true);

                        photoFrame.SetOffScreen();

                        if (_Cam)
                            _Cam.fieldOfView = _fov;

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
                        _RemovingCamera = false;
                        _ToolAnimator.SetBool("Unequipping", false);
                        UnequipCamera();
                    }
                }
            }
        }
        else
        {
            if (_Cam.fieldOfView - Input.mouseScrollDelta.y * 2 >= 10 && _Cam.fieldOfView - Input.mouseScrollDelta.y * 2 <= 90 && _PlayerControl._PlayerState == PlayerStates.PlayState)
            {
                _Cam.fieldOfView -= Input.mouseScrollDelta.y * 2;

                if (Input.mouseScrollDelta.y * 2 > 0)
                {
                    if (_source != null && _ZoomInClips.Length > 0)
                    {
                        _source.clip = _ZoomInClips[Random.Range(0, _ZoomInClips.Length)];

                        if (!_source.isPlaying)
                        {
                            _source.pitch = Random.Range(1, 1.25f);
                            _source.Play();
                        }
                    }
                }
                else if (Input.mouseScrollDelta.y * 2 < 0)
                {
                    if (_source != null && _ZoomOutClips.Length > 0)
                    {
                        _source.clip = _ZoomOutClips[Random.Range(0, _ZoomOutClips.Length)];
                        if (!_source.isPlaying)
                        {
                            _source.pitch = Random.Range(1, 1.25f);
                            _source.Play();
                        }
                    }
                }

                _CameraToolUI.GetComponent<CameraToolUI>().UpdateSlider();
            }
        }
    }

    IEnumerator CapturePhoto()
    {

        for (int i = 0; i < _UiToHide.Length; i++)
        {
            _UiToHide[i].SetActive(false);
        }

        viewingPhoto = true;

        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);

        screenCaptures[screenCapturesIndex].ReadPixels(regionToRead, 0, 0, false);
        screenCaptures[screenCapturesIndex].Apply();
        
        
        _CameraToolUI.SetActive(true);
        _CrosshairUI.SetActive(true);

        for (int i = 0; i < _UiToHide.Length; i++)
        {
            _UiToHide[i].SetActive(true);
        }

        ShowPhoto();
    }

    public void TakePhoto()
    {
        StartCoroutine(CapturePhoto());
    }


    void ShowPhoto()
    {
        AudioClip audioClip = _OpeningClips[Random.Range(0, _OpeningClips.Length - 1)];
        _source.clip = audioClip;
        _source.pitch = Random.Range(1, 1.25f);
        _source.Play();

        _CameraToolUI.GetComponent<CameraToolUI>().ActivateShutter();

        Sprite photoSprite = Sprite.Create(screenCaptures[screenCapturesIndex], new Rect(0.0f, 0.0f, screenCaptures[screenCapturesIndex].width, screenCaptures[screenCapturesIndex].height), new Vector2(0.5f, 0.5f), 100.0f);

        screenCapturesIndex++;

        if (screenCapturesIndex == 16)
            screenCapturesIndex = 0;



        PhotoDisplayArea.sprite = photoSprite;

        if (_PhotoPage._Photos.Count == 16)
            _PhotoPage._Photos.RemoveAt(0);
        _PhotoPage._Photos.Add(photoSprite);

        photoFrame.gameObject.SetActive(true);

        if (!photoFrame._ActivePhoto)
            photoFrame.MoveToOnScreen();
        else
            photoFrame.MoveOffScreen(true);
        
        _TooltipUI.SetActive(true);
    }

    void RemovePhoto()
    {
        viewingPhoto = false;
        photoFrame.gameObject.SetActive(false);
        
    }



    void DoSnap(InputAction.CallbackContext obj)
    {
        if (_PlayerControl._PlayerState == PlayerStates.PlayState)
        {
            if (!_CameraToolUIScript._Inuse)
            {
                _CameraToolUIScript._Inuse = true;
                TakePhoto();
            }
        }
    }

    void DoUnequip(InputAction.CallbackContext obj)
    {
        Unequip();
    }


    public void SpawnUI()
    {
        base.Equip();
    }

    

    public void RemoveCamera()
    {
        if (!_RemovingCamera)
        {
            _GogglesOn = false;
            _RemovingCamera = true;
            ToBlack = 0;
            ToClear = 0;
            _FadingToBlack = true;
            _FadeToBlack.gameObject.SetActive(true);
            _ToolAnimator.SetBool("Equipping", false);
        }
    }

    public override void Unequip()
    {
        RemoveCamera();
    }



    public void UnequipCamera()
    {
        base.Unequip();

        _GogglesOn = false;

        if (ToolManager.instance._CurrentTool)
            ToolManager.instance._CurrentTool._ToolSelected = false;
        ToolManager.instance._CurrentTool = null;
        ToolManager.instance._HoldingTool = false;
        

        _PlayerInput.actions["PrimaryAction"].performed -= DoSnap;
        _PlayerInput.actions["PrimaryAction"].Disable();


        _PlayerInput.actions["Unequip"].performed -= DoUnequip;
        _PlayerInput.actions["Unequip"].Disable();

        if (_SwappingTool != null)
        {
            _SwappingTool.gameObject.SetActive(true);
            _SwappingTool = null;
        }
        else
        {


            _ToolSlot._Arrow.gameObject.SetActive(false);
        }

        _ToolSlot.TweenDown();
 
        this.gameObject.SetActive(false);
    }

    public override void DetermineToolSwap()
    {
        base.DetermineToolSwap();
    }

}
