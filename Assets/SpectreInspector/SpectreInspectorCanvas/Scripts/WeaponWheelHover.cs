using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;


public class WeaponWheelHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    RectTransform rectTransform;
    Vector2 startPos;

    Vector3 _StartScale;

    public float _scaleMult = 1.15f;

    bool _IsHoveringOn = false;

    public AudioSource _SoundEmitter;
    public AudioClip[] _Sounds;

    GameObject _selector;

    public GameObject _buttonImage;
    public bool _isSpinning = false;

    public bool _IsPhotos;
    public Transform _MiddlePos;
    public Image _BigImage;
    public CameraPhotosPage _PhotosPage;
    public bool _HasImage;
    //Vector3 _Startpos;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
        _StartScale = rectTransform.localScale;
    }

    void Awake()
    {
        if (_IsPhotos)
        {
            if (_PhotosPage == null)
                _PhotosPage = FindObjectOfType<CameraPhotosPage>(true);
        }
    }

    public void ClickedPhoto()
    {
        if (_HasImage)
        {
            _BigImage.transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
            _BigImage.transform.position = this.transform.position;
            _BigImage.sprite = this.GetComponent<Image>().sprite;

            foreach (Image image in _PhotosPage._Displays)
            {
                image.transform.DOScale(0, 1).SetUpdate(true);
                image.GetComponent<WeaponWheelHover>().enabled = false;
                image.GetComponent<Button>().enabled = false;
            }
            

            _BigImage.transform.DOMove(_MiddlePos.position, 1f).SetUpdate(true).OnComplete(() => _PhotosPage._ImageSelected = true);
            _BigImage.transform.DOScale(4, 1).SetUpdate(true);
        }
    }

    private void OnDisable()
    {
        rectTransform.localScale = _StartScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_IsPhotos)
            TweenOut();
        else if (_HasImage)
            TweenOut();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_IsPhotos)
            TweenIn();
        else if (_HasImage)
            TweenIn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_IsPhotos)
            TweenOut();
        else if (_HasImage)
            TweenOut();
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_IsPhotos)
            TweenIn();
        else if (_HasImage)
            TweenIn();
    }

    public void TweenOut()
    {
        rectTransform.DOScale(_StartScale * _scaleMult, 0.25f).SetUpdate(true);

        /*AudioClip audioClip = _Sounds[Random.Range(0, _Sounds.Length - 1)];
        _SoundEmitter.clip = audioClip;
        _SoundEmitter.Play();*/

    }

    public void TweenIn()
    {
        rectTransform.DOScale(_StartScale, 0.25f).SetUpdate(true);

    }

    // EXTRA POLISH!!!
    public void ImageSpin()
    {
        if(!_isSpinning)
        {
            _isSpinning = true;
            _buttonImage.GetComponent<RectTransform>().DORotate(new Vector3(0, 720, 0), 1.5f, RotateMode.FastBeyond360).OnComplete(() => _isSpinning = false).SetUpdate(true);
        }
    }

}
