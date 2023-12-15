using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    private int HealthState = 3;
    private int MaxHealth = 3;

    private int DrinkMeter = 0;
    private int MaxDrinkMeter = 7;

    [SerializeField] private PlayerUI playerUI;

    public static PlayerStats instance;

    Vector3 _PlayerStartPos;
    public PhysicsPlayerController player;

    public Image _FadeToBlack;
    bool _FadingToBlack;
    bool _FadingToClear;
    float ToBlack;
    float ToClear;

    public float _TimeToBlack = 1;
    public float _TimeToClear = 1;
    public float _TimeIsBlack = 2;

    public AudioSource _PlayerAudioSource;
    public AudioClip _DeathSound;
    public AudioClip[] _HurtSounds;

    public AudioSource _FootstepSource;
    float footVol;

    public Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }


        if(playerUI == null)
        {
            playerUI = FindObjectOfType<PlayerUI>();
        }

        if (player == null)
        {
            player = this.GetComponent<PhysicsPlayerController>();
        }

        if (_PlayerAudioSource == null)
        {
            _PlayerAudioSource = this.GetComponent<AudioSource>();
        }

        if (rigidbody == null)
        {
            rigidbody = this.GetComponent<Rigidbody>();
        }

        _PlayerStartPos = this.transform.position;
        //_PlayerStartPos.y += 0.5f;
    }

    private void Update()
    {
        if (HealthState == 0 && _FadingToBlack == false && _FadingToClear == false)
        {
            _FadingToBlack = true;
            _FadeToBlack.gameObject.SetActive(true);
            footVol = _FootstepSource.volume;
            _FootstepSource.volume = 0;
            _PlayerAudioSource.clip = _DeathSound;
            _PlayerAudioSource.Play();
            player._PlayerState = PlayerStates.DeathState;

            ResetTools();
        }

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

                ResetPlayer();


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
                player._PlayerState = PlayerStates.PlayState;
                _FootstepSource.volume = footVol;

                

                ToBlack = 0;
                ToClear = 0;
            }
        }
    }

    void ResetPlayer()
    {
        HealthState = 3;
        rigidbody.velocity = Vector3.zero;
        this.transform.position = _PlayerStartPos;

        playerUI.UpdateHearts(MaxHealth - HealthState);
        playerUI.UpdatePlayer(MaxHealth - HealthState);
    }

    void ResetTools()
    {
        if (ToolManager.instance._CurTrapTool != null)
        {
            ToolManager.instance._CurTrapTool.RetrieveTrap();
        }



        if (ToolManager.instance._CurrentTool != null)
        {
            if (ToolManager.instance._CurrentTool.GetComponent<CameraTool>())
            {
                CameraTool cam = ToolManager.instance._CurrentTool.GetComponent<CameraTool>();

                cam._CameraToolUI.SetActive(false);
                cam._StandardUI.SetActive(true);
                cam.UnequipCamera();
            }

            if (ToolManager.instance._CurrentTool.GetComponent<GoggleTool>())
            {
                GoggleTool goggle = ToolManager.instance._CurrentTool.GetComponent<GoggleTool>();

                goggle._GoggleScan.SetActive(false);

                for (int i = 0; i < goggle.pooledPaths.Count; i++)
                {
                    goggle.pooledPaths[i].gameObject.SetActive(false);//////////////////////////////////////////////////////////////////////////////////////
                }

                goggle.UnequipGoggles();
            }

           

            if (ToolManager.instance._CurrentTool != null)
            {
                ToolManager.instance._CurrentTool.Unequip();
                ToolManager.instance._CurrentTool.gameObject.SetActive(false);
                ToolManager.instance._CurrentTool = null;
            }
        }
    }

    public void ReduceHealth()
    {
        if (HealthState != 0)
        {
            HealthState--;
            playerUI.UpdateHearts(MaxHealth - HealthState);
            if (HealthState != 0)
                playerUI.UpdatePlayer(MaxHealth - HealthState);

            _PlayerAudioSource.clip = _HurtSounds[Random.Range(0, _HurtSounds.Length)];
            _PlayerAudioSource.pitch = Random.Range(0.95f, 1.0f);
            _PlayerAudioSource.Play();

            
        }
    }

    public void IncreaseDrink()
    {
        if(DrinkMeter < MaxDrinkMeter)
        DrinkMeter++;
        playerUI.UpdateDrink(DrinkMeter);
    }

    public void ResetDrink()
    {
        DrinkMeter = 0;
    }
    public void ResetHealth()
    {
        HealthState = 3;
    }
}
