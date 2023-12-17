using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLookAtPlayer : MonoBehaviour
{
    GameObject _MainCamera;
    // Start is called before the first frame update
    void Start()
    {
        _MainCamera = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 point = new Vector3 (0, _MainCamera.transform.position.y, 0);
        transform.LookAt(Camera.main.transform);
    }
}
