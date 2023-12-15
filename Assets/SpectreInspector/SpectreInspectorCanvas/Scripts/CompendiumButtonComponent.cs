using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompendiumButtonComponent : MonoBehaviour
{
    public Compendium _Compendium;
    public GameObject _RenderObject;
    public Transform _CameraRotator;
    public CreatureScriptableObject _Creature;
    public Image _lastPic;

    [HideInInspector]
    public Texture2D _LastPhotoOfGhost;
    public Sprite _LastPhotoOfGhostSprite;

    public Sprite _SelectedSprite;
    public Sprite _UnselectedSprite;

    private void OnEnable()
    {
        
    }

    public void ChangeCreature()
    {

        /*for (int i = 0; i < _Compendium.buttons.Count; i++)
        {
            _Compendium.buttons[i].GetComponent<Button>().image.sprite = _UnselectedSprite;
        }
        GetComponent<Button>().image.sprite = _SelectedSprite;

        if (_Compendium._CurRender && _Compendium._CurRender != _RenderObject)
        {
            _Compendium._CurRender.SetActive(false);
            _RenderObject.SetActive(true);
            _Compendium._CurRender = _RenderObject;
        }
        else if (_Compendium._CurRender == null)
        {
            _RenderObject.SetActive(true);
            _Compendium._CurRender = _RenderObject;
        }

        if (_Compendium._CurCamera && _Compendium._CurCamera != _CameraRotator)
            _Compendium._CurCamera.GetComponent<RenderCameraComponent>().ResetRot();

        _Compendium._CurCamera = _CameraRotator;
        _Compendium._RotateCompendium._CameraRotator = _CameraRotator;
        _lastPic.sprite = _LastPhotoOfGhostSprite;

        _Compendium.SelectCreature(_Creature);*/
    }
}
