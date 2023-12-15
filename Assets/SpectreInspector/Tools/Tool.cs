using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    //public WeaponWheelButton _weaponWheelButton;

    [HideInInspector]
    public bool _ToolSelected;
    //public Vector3 _HandHeldDistFromCam;
    //public Vector3 _HandHeldRot;

    [HideInInspector]
    public GameObject _ToolUI;
    public GameObject _ToolUIPrefab;
    MainCanvasComponent _MainCanvas;

    [HideInInspector]
    public Tool _SwappingTool;

    public AudioClip[] _EquipSounds;
    public AudioClip[] _UnequipSounds;
    public AudioSource _AudioSource;

    public virtual void Unequip() 
    { 
        Destroy(_ToolUI);
    }
    public virtual void Equip() 
    {
        _MainCanvas = ToolManager.instance._MainCanvas;

        _ToolUI = Instantiate(_ToolUIPrefab, _MainCanvas._ToolUILocations.transform);   
    }
    public virtual void Switch() 
    {
        ToolManager.instance._CurrentTool.Unequip();

        Equip();
    }
    public virtual void DetermineToolSwap() 
    {

        if (ToolManager.instance._CurrentTool == null)
            Equip();
        else if (ToolManager.instance._CurrentTool.name != this.gameObject.name)
        {
            Switch();
        }
    }

    public virtual void playEquipSound()
    {
        if (_AudioSource != null && _EquipSounds.Length > 0)
        {
            _AudioSource.clip = _EquipSounds[Random.Range(0, _EquipSounds.Length)];
            _AudioSource.Play();
        }
    }

    public virtual void playUnequipSound()
    {
        if (_AudioSource != null && _UnequipSounds.Length > 0)
        {
            _AudioSource.clip = _UnequipSounds[Random.Range(0, _UnequipSounds.Length)];
            _AudioSource.Play();
        }
    }
}
