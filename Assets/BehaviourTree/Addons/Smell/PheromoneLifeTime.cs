using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheromoneLifeTime : MonoBehaviour
{
    public float maxLifeTime = 5f;
    public float time = 0;
    Vector3 reducePerFrame;


    //Priority Decay
    public Trackable trackablePheromone;
    Trackable trackable;

    TrackerHolder trackerHolder;
    float reduceAmount;

    float size;
    // Start is called before the first frame update
    void Start()
    {
        trackerHolder = GetComponent<TrackerHolder>();
        size = gameObject.transform.localScale.x;
        reducePerFrame = new Vector3 (size / maxLifeTime, size / maxLifeTime, size / maxLifeTime) ;


        for(int i = 0; i < trackerHolder.trackers.Count; i++)
        {
            if(trackerHolder.trackers[i].name == trackablePheromone.name)
            {
                trackable = trackerHolder.trackers[i];
                break;
            }
        }

        reduceAmount = trackable.priority / maxLifeTime;
    }

    // Update is called once per frame
    void Update()
    {

        transform.localScale -= reducePerFrame * Time.deltaTime;
        //trackable.priority -= 0.01f * Time.deltaTime;

        time  += Time.deltaTime;

        if (time > maxLifeTime)
        {
            Destroy(gameObject);
        }

    }
}
