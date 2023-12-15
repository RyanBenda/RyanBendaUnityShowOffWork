using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierSoundComponent : MonoBehaviour
{
    public AudioSource _AudioSource;

    float timeTillNextZap;
    float t;

    // Start is called before the first frame update
    void Awake()
    {
        timeTillNextZap = Random.Range(0.5f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;

        if (t >= timeTillNextZap && _AudioSource != null)
        {
            _AudioSource.pitch = Random.Range(1, 1.25f);
            _AudioSource.Play();

            timeTillNextZap = Random.Range(0.5f, 3f);

            t = 0;
        }
    }
}
