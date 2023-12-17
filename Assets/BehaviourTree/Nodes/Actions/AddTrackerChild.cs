using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTrackerChild : ActionNode
{
    public Trackable tracker;

    public Vector3 holdPos = new Vector3(0, 0, 1);

    GameObject closestObject;

    public float radius = 2f;

    protected override void OnStart()
    {
        TrackerHolder[] Objs;
        Objs = GameObject.FindObjectsOfType<TrackerHolder>();

        if (Objs.Length != 0)
        {
            float closest = Vector3.Distance(Objs[0].transform.position, attachedGameObject.transform.position);

            foreach (TrackerHolder holder in Objs)
            {
                foreach (Trackable b in holder.trackers)
                {
                    if (b.name == tracker.name)
                    {
                        float a = Vector3.Distance(holder.transform.position, attachedGameObject.transform.position);

                        if (a <= closest)
                        {
                            closest = a;
                            closestObject = holder.transform.gameObject;
                        }
                    }
                }


            }
        }

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (closestObject != null)
        {
            closestObject.transform.parent = attachedGameObject.transform;
            closestObject.transform.localPosition = holdPos;


            if(closestObject.GetComponent<Rigidbody>())
            {
                closestObject.GetComponent<Rigidbody>().isKinematic = true;
            }


            return State.Success;


        }
        return State.Failure;

    }

}
