using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEmotionCanvas : MonoBehaviour
{
    // Start is called before the first frame update

    [HideInInspector]
    public float _Timer = 0;
    public float _TimeTillInactive;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _Timer += Time.deltaTime;

        if (_Timer >= _TimeTillInactive)
            this.gameObject.SetActive(false);
    }
}
