using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheromones : MonoBehaviour
{
    public float timeTillNextDrop = 0.5f;
    float time = 0;

    public GameObject sphere;


    float size;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {


        time += Time.deltaTime;

        if (time > timeTillNextDrop)
        {
            GameObject gameObject = Instantiate(sphere, transform.position, transform.rotation);
            time = 0;
        }

    }
}
