using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Compendium : MonoBehaviour
{
    //public TextMeshProUGUI _CreatureNameText;
    public Image _CreatureNameImage;
    public RawImage _CreatureImage;
    public TextMeshProUGUI _CreatureDescriptionText;
    //public RotateCompendiumComponent _RotateCompendium;

    public CreatureScriptableObject _StartingCreature;
    public GameObject _StartingRender;
    public Transform _StartCam;

    [HideInInspector]
    public GameObject _CurRender;
    [HideInInspector]
    public Transform _CurCamera;

    public GameObject Tab;


    public GameObject Pad;
    public GameObject PadOffscreen;
    public GameObject PadOnScreen;
    public float TimeToComplete = 0.55f;

    public List<CompendiumButtonComponent> buttons = new List<CompendiumButtonComponent>();

    

    private void Awake()
    {
        SelectCreature(_StartingCreature);
        _StartingRender.SetActive(true);
        _CurRender = _StartingRender;
        //_RotateCompendium._CameraRotator = _StartCam;
        _CurCamera = _StartCam;
    }

    private void OnEnable()
    {
        Tab.transform.localScale = Vector3.zero;
        Pad.transform.position = PadOffscreen.transform.position;
        Pad.transform.DOMoveY(PadOnScreen.transform.position.y, TimeToComplete).SetEase(Ease.OutSine).SetUpdate(true);
        Tab.transform.DOScale(Vector3.one, TimeToComplete).SetEase(Ease.OutBounce).SetUpdate(true);
    }

    public void SelectCreature(CreatureScriptableObject creature)
    {
        _CreatureNameImage.sprite = creature._CompendiumPlate;
        _CreatureImage.texture = creature._Image;
        _CreatureDescriptionText.text = creature._Description;
    }
}
