using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITabButtonClick : MonoBehaviour
{
    public GameObject[] OtherTabs;
    public GameObject myTab;

    public AudioSource AudioSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonClicked()
    {
        for(int i = 0; i < OtherTabs.Length; i++)
        {
            OtherTabs[i].gameObject.SetActive(false);

        }
        myTab.SetActive(true);
        AudioSound.Play();
        GetComponent<WeaponWheelHover>().ImageSpin();
    }
}
