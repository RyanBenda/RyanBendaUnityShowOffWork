using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Compendium : MonoBehaviour
{


    public GameObject Pad;
    public GameObject PadOffscreen;
    public GameObject PadOnScreen;
    public float TimeToComplete = 0.55f;

    public List<CompendiumButtonComponent> buttons = new List<CompendiumButtonComponent>();



    private void OnEnable()
    {

        Pad.transform.position = PadOffscreen.transform.position;
        Pad.transform.DOMoveY(PadOnScreen.transform.position.y, TimeToComplete).SetEase(Ease.OutSine).SetUpdate(true);

    }
}
