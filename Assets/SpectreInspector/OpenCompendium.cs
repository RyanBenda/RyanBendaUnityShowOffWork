using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenCompendium : MonoBehaviour
{

    PlayerInput _PlayerInput;

    [SerializeField] private GameObject _CompendiumOBJ;
    [SerializeField] private GameObject _CrossHair;
    //GameObject _Compendium;

    bool _CompendiumOpen;

    [SerializeField] private Vector3 _CompendiumSpawnPosition;

    PhysicsPlayerController _PC;


    // Start is called before the first frame update
    void Awake()
    {
        _PC = FindObjectOfType<PhysicsPlayerController>();
        _PlayerInput = _PC._PlayerInput;




    }

    private void OnEnable()
    {
        _PlayerInput.actions["OpenCompendium"].performed += DoCompendiumAction; // Binds DoJump to the action of "Jump"
        _PlayerInput.actions["OpenCompendium"].Enable();
    }

    private void OnDisable()
    {
        _PlayerInput.actions["OpenCompendium"].performed -= DoCompendiumAction; // Binds DoJump to the action of "Jump"
        _PlayerInput.actions["OpenCompendium"].Disable();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!_CompendiumOpen && Input.GetKeyDown(KeyCode.Tab))
        //{
        //    OpenCompendiumMethod();
        //}
        //else if (_CompendiumOpen && Input.GetKeyDown(KeyCode.Tab))
        //{
        //    CloseCompendium();
        //}
    }

    void DoCompendiumAction(InputAction.CallbackContext obj)
    {
        if (!_CompendiumOpen)
        {
            if (ToolManager.instance._CurrentTool == null)
                OpenCompendiumMethod();
            else if (ToolManager.instance._CurrentTool.GetComponent<CameraTool>() == null && ToolManager.instance._CurrentTool.GetComponent<GoggleTool>() == null)
                OpenCompendiumMethod();
        }
        else if (_CompendiumOpen)
        {
            CloseCompendium();
        }
    }   
    

    void OpenCompendiumMethod()
    {
        if (_PC._PlayerState == PlayerStates.PlayState)
        {
            _PC._PlayerState = PlayerStates.PauseState;

            _CompendiumOBJ.SetActive(true);
            if (_CompendiumOBJ.GetComponent<Compendium>()._CurRender)
                _CompendiumOBJ.GetComponent<Compendium>()._CurRender.SetActive(true);

            _CrossHair.SetActive(false);
            //_Compendium = Instantiate(_CompendiumPrefab, Camera.main.transform);

            //_Compendium.transform.localPosition = new Vector3(_Compendium.transform.localPosition.x + _CompendiumSpawnPosition.x, _Compendium.transform.localPosition.y + _CompendiumSpawnPosition.y, _Compendium.transform.localPosition.z + _CompendiumSpawnPosition.z);

            _CompendiumOpen = true;
            Cursor.lockState = CursorLockMode.Confined;

           
        }
    }

    void CloseCompendium()
    {
        _PC._PlayerState = PlayerStates.PlayState;
        if (_CompendiumOBJ.GetComponent<Compendium>()._CurRender)
            _CompendiumOBJ.GetComponent<Compendium>()._CurRender.SetActive(false);

        _CompendiumOBJ.SetActive(false);
        _CrossHair.SetActive(true);
        //Destroy(_Compendium);
        _CompendiumOpen = false;
        Cursor.lockState = CursorLockMode.Locked;

        
    }
}
