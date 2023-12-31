using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleIcon : MonoBehaviour
{
    [SerializeField] private float sizeOnScreen;

    void Update()
    {
        Vector3 a = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 b = new Vector3(a.x, a.y + sizeOnScreen, a.z);

        Vector3 aa = Camera.main.ScreenToWorldPoint(a);
        Vector3 bb = Camera.main.ScreenToWorldPoint(b);

        transform.localScale = Vector3.one * (aa - bb).magnitude;
    }
}
