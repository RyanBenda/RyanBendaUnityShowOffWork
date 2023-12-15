using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class CameraPhotosPage : MonoBehaviour
{
    public List<Sprite> _Photos = new List<Sprite>();

    public List<Image> _Displays = new List<Image>();

    public bool _ImageSelected;
    public Image _BigImage;

    private void OnEnable()
    {
        int index = 0;
        for (int i = _Photos.Count - 1; i >= 0; i--)
        {
            _Displays[index].sprite = _Photos[i];
            _Displays[index].GetComponent<WeaponWheelHover>()._HasImage = true;
            index++;
            
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_ImageSelected && Input.GetMouseButtonDown(0))
        {
            _ImageSelected = false;

            foreach (Image image in _Displays)
            {
                image.transform.DOScale(1, 1).SetUpdate(true).OnComplete(() => OnTweenComplete(image));
                
            }

            _BigImage.transform.position = new Vector3(10000, 10000, 10000);

        }
    }

    void OnTweenComplete(Image image)
    {
        image.GetComponent<WeaponWheelHover>().enabled = true;
        image.GetComponent<Button>().enabled = true;
    }
}
