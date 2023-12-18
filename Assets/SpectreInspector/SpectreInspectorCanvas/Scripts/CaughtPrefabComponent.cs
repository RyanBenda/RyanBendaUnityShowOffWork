using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using DG.Tweening;

public class CaughtPrefabComponent : MonoBehaviour
{
    public Image _CreatureImage;
    public TextMeshProUGUI _CreatureName;
    public Image _CreatureNamePlate;

    [HideInInspector]
    public bool _CreatureBannerActive;
    public float _CreatureBannerTimer;
    float _RealCreatureBannerTimer;

    float OriginalUniformScale;

    private void Awake()
    {
        OriginalUniformScale = transform.localScale.x;
        transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        _RealCreatureBannerTimer = _CreatureBannerTimer;

        transform.DOScale(OriginalUniformScale, 0.45f).SetEase(Ease.OutBounce);
        transform.DOShakeRotation(0.35f, 40);
    }

    void Update()
    {
        if (_CreatureBannerActive && _RealCreatureBannerTimer > 0)
        {
            _RealCreatureBannerTimer -= Time.deltaTime;

        }
        else if (_CreatureBannerActive)
        {
            _CreatureBannerActive = false;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(0, 0.45f)
                .SetEase(Ease.InBounce).OnComplete(() => this.gameObject.SetActive(false)));
            sequence.Play();
        }
    }

    void TurnOff()
    {
        this.gameObject.SetActive(false);
    }

    public void SetCreature(Sprite GhostIcon, Sprite NamePlate)
    {
        _CreatureImage.sprite = GhostIcon;
        _CreatureNamePlate.sprite = NamePlate;
        _CreatureBannerActive = true;
    }
}
