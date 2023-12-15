using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TrackerTool : HandheldTool
{
    public static TrackerTool CameraInstance;

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
    public PlayerInput playerInput;

    public List<GameObject> _OtherTools = new List<GameObject>();

    //public HotBarPos _ToolSlot;

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

    //public CameraPhotosPage _PhotoPage;

    public Plane[] planes;

    PhysicsPlayerController playerControl;

    private void Awake()
    {
        //screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        //screenCapture2 = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        for (int i = 0; i < screenCaptures.Length; i++)
        {
            screenCaptures[i] = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        }

        _CameraToolUIScript = _CameraToolUI.GetComponent<CameraToolUI>();

        if (photoFrame == null)
            photoFrame = FindObjectOfType<PhotoPopInEffect>(true);

        //if (_PhotoPage == null)
            //_PhotoPage = FindObjectOfType<CameraPhotosPage>(true);

        //if (_CompendiumButtons == null)
            //_CompendiumButtons = FindObjectsOfType<CompendiumButtonComponent>(true);

        CameraInstance = this;

        playerInput = FindObjectOfType<PlayerInput>();

        //playerInput.actions["PrimaryAction"].performed += DoSnap;
        //playerInput.actions["PrimaryAction"].Enable();

        //playerInput.actions["SecondaryAction"].performed += DoUnequip;
        //playerInput.actions["SecondaryAction"].Enable();

        this.gameObject.SetActive(false);   
    }

    void Start()
    {
        _Cam = Camera.main;
        _fov = _Cam.fieldOfView;
        //playerInput = FindObjectOfType<PlayerInput>();


        
    }

    private void OnEnable()
    {
        if (playerControl == null)
            playerControl = Camera.main.transform.parent.GetComponent<PhysicsPlayerController>();

        //playerInput = FindObjectOfType<PlayerInput>();

        bool _delayToolSwap = false;
        if (playerControl._CurrentTool != null && playerControl._CurrentTool != this)
        {
            playerControl._CurrentTool._SwappingTool = this;

            if (playerControl._CurrentTool.gameObject.GetComponent<HandHeldTrap>() || playerControl._CurrentTool.gameObject.GetComponent<GoggleTool>())
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
            playerControl._CurrentTool = this;
            playerControl._HoldingTool = true;
            _ToolSelected = true;

            base.Equip();

            for (int i = 0; i < _OtherTools.Count; i++)
            {
                _OtherTools[i].gameObject.SetActive(false);
            }

            //_ToolSlot.TweenUp();
            //_ToolSlot.MoveArrow(3);

            _ToolAnimator.SetBool("Equipping", true);



            playerInput.actions["PrimaryAction"].performed += DoSnap;
            playerInput.actions["PrimaryAction"].Enable();

            playerInput.actions["Unequip"].performed += DoUnequip;
            playerInput.actions["Unequip"].Enable();
        }
        else
        {
            _PreventSwap = false;
        }

    }

    void NewInputSetup()
    {
        playerInput.actions["PrimaryAction"].performed += DoSnap;
        playerInput.actions["PrimaryAction"].Enable();

        playerInput.actions["Unequip"].performed += DoUnequip;
        playerInput.actions["Unequip"].Enable();

    }

    private void OnDisable()
    {
        /*playerInput.actions["PrimaryAction"].performed -= DoSnap;
        playerInput.actions["PrimaryAction"].Disable();


        playerInput.actions["Unequip"].performed -= DoUnequip;
        playerInput.actions["Unequip"].Disable();*/
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
        //if (timer < _blipIntervals)
        //{
        //    timer += Time.deltaTime;

        //    if (timer >= _blipIntervals)
        //    {
        //        timer = 0;
        //        SetCreaturePos();

        //    }
        //}

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
            if (_Cam.fieldOfView - Input.mouseScrollDelta.y * 2 >= 10 && _Cam.fieldOfView - Input.mouseScrollDelta.y * 2 <= 90 && playerControl._PlayerState == PlayerStates.PlayState)
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

            /*if (Input.GetMouseButtonDown(0))
            {
                
            }*/
        }

        //if (Input.GetMouseButtonDown(1))
        //{
        //    Unequip();
        //}
        //else if (Input.GetMouseButtonDown(0))
        //{
        //    Snap();
        //}
    }

    IEnumerator CapturePhoto()
    {
        //_CameraToolUI.SetActive(false);
        //_CrosshairUI.SetActive(false);
        //_TooltipUI.SetActive(false);

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
        //if (!viewingPhoto)
        StartCoroutine(CapturePhoto());
        //else
            //RemovePhoto();
    }

    void CheckForGhost(Texture2D text)
    {
        planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        

        List<CreatureBrain> ghosts = new List<CreatureBrain>();
        
        foreach (CreatureBrain creatureBrain in _Creatures)
        {
            ghosts.Add(creatureBrain);
        }

        for (int i = 0; i < ghosts.Count; i++)
        {
            if (!ghosts[i].IsInFrustumPlanes())
            {
                ghosts.RemoveAt(i);
                i--;
            }
        }

        bool _ContainsGhosts = false;

        for (int i = 0; i < ghosts.Count; i++)
        {
            Debug.Log("ANGLE CHECK: " + Vector3.Angle(Camera.main.transform.forward, ghosts[i].transform.position - Camera.main.transform.position) + " FOV: " + Camera.main.fieldOfView);

            if (Vector3.Angle(Camera.main.transform.forward, ghosts[i].transform.position - Camera.main.transform.position) <= Camera.main.fieldOfView * 0.65f)
            {
                Debug.Log("ANGLE Passed");
                if (PhotoRaycasts(ghosts[i].transform.position - Camera.main.transform.position))
                {
                    _ContainsGhosts = true;
                    Debug.Log("ANGLE GOTPHOTO");

                    CreatureBrain creatureBrain = ghosts[i];

                    //creatureBrain.CheckPhotoQuest();

                    /*for (int j = 0; j < _CompendiumButtons.Length; j++)
                    {
                        if (_CompendiumButtons[j]._Creature == ghosts[i]._CreatureIdentity)
                        {
                            Texture2D copyTexture = new Texture2D(text.width, text.height);
                            copyTexture.SetPixels(text.GetPixels());
                            copyTexture.Apply();

                            Sprite photoSprite = Sprite.Create(copyTexture, new Rect(0.0f, 0.0f, copyTexture.width, copyTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

                            _CompendiumButtons[j]._LastPhotoOfGhost = copyTexture;
                            _CompendiumButtons[j]._LastPhotoOfGhostSprite = photoSprite;
                            //_CompendiumButtons[j]._lastPic.sprite = photoSprite;
                        }
                    }*/
                }
                else
                {
                    ghosts.RemoveAt(i);
                    i--;
                }
            }
            else
            {
                ghosts.RemoveAt(i);
                i--;
            }
            /*RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, ghosts[i].transform.position - Camera.main.transform.position, out hit, 100))
            {
                if (hit.collider.gameObject.GetComponent<CreatureBrain>())
                {
                    CreatureBrain creatureBrain = hit.collider.gameObject.GetComponent<CreatureBrain>();

                    creatureBrain.CheckPhotoQuest();

                    for (int j = 0; j < _CompendiumButtons.Length; j++)
                    {
                        if (_CompendiumButtons[j]._Creature == ghosts[i]._CreatureIdentity)
                        {
                            Texture2D copyTexture = new Texture2D(text.width, text.height);
                            copyTexture.SetPixels(text.GetPixels());
                            copyTexture.Apply();

                            Sprite photoSprite = Sprite.Create(copyTexture, new Rect(0.0f, 0.0f, copyTexture.width, copyTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

                            _CompendiumButtons[j]._LastPhotoOfGhost = copyTexture;
                            _CompendiumButtons[j]._LastPhotoOfGhostSprite = photoSprite;
                        }
                    }

                }
                else
                {
                    ghosts.RemoveAt(i);
                    i--;
                }

            }
            else
            {
                ghosts.RemoveAt(i);
                i--;
            } */
        }


        


    }

    public bool PhotoRaycasts(Vector3 dir)
    {
        if (GhostTarget(Vector3.zero, dir))
        {
            return true;
        }
        else
        {

            for (int i = 1; i < 5; i++)
            {
                if (GhostTarget(new Vector3(i / 5, 0, 0), dir)) { return true; }
                if (GhostTarget(new Vector3(i / 5 * -1, 0, 0), dir)) { return true; }
                if (GhostTarget(new Vector3(0, i / 5, 0), dir)) { return true; }
                if (GhostTarget(new Vector3(0, i / 5 * -1, 0), dir)) { return true; }

                // Inbetweens!
                if (GhostTarget(new Vector3(i / 5, i / 5, 0), dir)) { return true; }
                if (GhostTarget(new Vector3(i / 5 * -1, i / 5 * -1, 0), dir)) { return true; }
                if (GhostTarget(new Vector3(i / 5, i / 5 * -1, 0), dir)) { return true; }
                if (GhostTarget(new Vector3(i / 5, i / 5 * -1, 0), dir)) { return true; }
            }

            return false;
        }
    }

    public bool GhostTarget(Vector3 pos, Vector3 dir)
    {
        RaycastHit hit;
        _RaycastShoot.transform.localPosition = pos;
        _RaycastShoot.transform.rotation = Camera.main.transform.rotation;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(_RaycastShoot.transform.position, dir, out hit, Mathf.Infinity, _GhostCheckLayermask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawRay(_RaycastShoot.transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if (hit.collider.gameObject.GetComponent<CreatureBrain>())
            {
                //hit.collider.gameObject.GetComponent<CreatureBrain>().CameraStun();


                return true;
            }
            

        }


        return false;

    }

    void ShowPhoto()
    {
        AudioClip audioClip = _OpeningClips[Random.Range(0, _OpeningClips.Length - 1)];
        _source.clip = audioClip;
        _source.pitch = Random.Range(1, 1.25f);
        _source.Play();

        _CameraToolUI.GetComponent<CameraToolUI>().ActivateShutter();
        ShootRaycasts();

        Sprite photoSprite = Sprite.Create(screenCaptures[screenCapturesIndex], new Rect(0.0f, 0.0f, screenCaptures[screenCapturesIndex].width, screenCaptures[screenCapturesIndex].height), new Vector2(0.5f, 0.5f), 100.0f);
        CheckForGhost(screenCaptures[screenCapturesIndex]);

        screenCapturesIndex++;

        if (screenCapturesIndex == 16)
            screenCapturesIndex = 0;



        //PhotoDisplayArea.sprite = photoSprite;

        /*if (_PhotoPage._Photos.Count == 16)
            _PhotoPage._Photos.RemoveAt(0);
        _PhotoPage._Photos.Add(photoSprite);*/

        photoFrame.gameObject.SetActive(true);

        if (!photoFrame._ActivePhoto)
            photoFrame.MoveToOnScreen();
        else
            photoFrame.MoveOffScreen(true);
        
        //_TooltipUI.SetActive(true);
    }

    void RemovePhoto()
    {
        viewingPhoto = false;
        photoFrame.gameObject.SetActive(false);
        
    }

    public override void Equip()
    {
        Tool currentTool = playerControl._CurrentTool;

        if (currentTool == null) // NEED TO CHANGE TO HANDLE MULTIPLE TOOLS
        {

        }
        else if (currentTool.GetComponent<TrackerTool>() == null)
        {

        }
    }

    void DoSnap(InputAction.CallbackContext obj)
    {
        if (playerControl._PlayerState == PlayerStates.PlayState)
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

        if (playerControl._CurrentTool)
            playerControl._CurrentTool._ToolSelected = false;
        playerControl._CurrentTool = null;
        playerControl._HoldingTool = false;
        if (_Cam)
            _Cam.fieldOfView = _fov;

        playerInput.actions["PrimaryAction"].performed -= DoSnap;
        playerInput.actions["PrimaryAction"].Disable();


        playerInput.actions["Unequip"].performed -= DoUnequip;
        playerInput.actions["Unequip"].Disable();

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
        //Destroy(_CameraToolUI);
        this.gameObject.SetActive(false);
    }

    public override void DetermineToolSwap()
    {
        base.DetermineToolSwap();
    }


    public void ShootRaycasts()
    {
        if (HitTarget(Vector3.zero))
        {
            
        }
        else
        {

            for (int i = 1; i < 20; i++)
            {
                if(HitTarget(new Vector3(i / 5, 0, 0))) { break; }
                if (HitTarget(new Vector3(i / 5 * -1, 0, 0))) { break; }
                if (HitTarget(new Vector3(0, i / 5, 0))) { break; }
                if (HitTarget(new Vector3(0, i / 5 * -1, 0))) { break; }

                // Inbetweens!
                if (HitTarget(new Vector3(i / 5, i / 5, 0 ))) { break; }
                if (HitTarget(new Vector3(i / 5 * -1, i / 5 * -1, 0))) { break; }
                if (HitTarget(new Vector3(i / 5, i / 5 * -1, 0))) { break; }
                if (HitTarget(new Vector3(i / 5, i / 5 * -1, 0))) { break; }
            }
        }
    }

    public bool HitTarget(Vector3 pos)
    {
        RaycastHit hit;
        _RaycastShoot.transform.localPosition = pos;
        _RaycastShoot.transform.rotation = Camera.main.transform.rotation;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(_RaycastShoot.transform.position, Camera.main.gameObject.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, _HitTargetLayermask))
        {
            Debug.DrawRay(_RaycastShoot.transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if (hit.collider.gameObject.GetComponent<CreatureBrain>())
            {
                hit.collider.gameObject.GetComponent<CreatureBrain>().CameraStun();
                return true;
            }
            

        }


        return false;

    }






}
