using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReferenceScript : MonoBehaviour
{
    public MainCanvasComponent _MainCanvas;

    // Start is called before the first frame update
    void Awake()
    {
        if (_MainCanvas == null)
            _MainCanvas = FindObjectOfType<MainCanvasComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
