using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StarRotateAround : MonoBehaviour
{
    public GameObject creature;

    Vector3 localStartPos;
    // Start is called before the first frame update
    void Start()
    {
        localStartPos = transform.localPosition;

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOMoveY(0.15f, 1.5f));
        mySequence.Append(transform.DOMoveY(0.0f, 1.5f));
        mySequence.SetLoops(-1);
        mySequence.Play();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<TrailRenderer>())
                transform.GetChild(i).GetComponent<TrailRenderer>().enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, 45 * Time.deltaTime);
        transform.localPosition = localStartPos;

    }
}
