using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpTrackerObject : ActionNode
{
    public Trackable tracker;
    public Trackable avoidTracker;

    public Vector3 holdPos = new Vector3(0, 0, 1);

    public bool useCustomPos = true;

    GameObject closestObject;

    public float radius = 2f;

    protected override void OnStart()
    {
        TrackerHolder[] Objs;
        Objs = GameObject.FindObjectsOfType<TrackerHolder>();

        if (avoidTracker == null)
        {
            avoidTracker = ScriptableObject.CreateInstance<Trackable>();
            avoidTracker.name = "DoesntUseAvoid";
        }


        if (Objs.Length != 0)
        {
            float closest = Vector3.Distance(Objs[0].transform.position, attachedGameObject.transform.position);

            foreach (TrackerHolder holder in Objs)
            {
                foreach (Trackable b in holder.trackers)
                {
                    if (b != null)
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
                        else if (b.name == avoidTracker.name)
                        {
                            closestObject = null;
                            break;
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

            if (useCustomPos)
            {
                closestObject.transform.localPosition = holdPos;
            }

            if (closestObject.GetComponent<Rigidbody>())
            {
                closestObject.GetComponent<Rigidbody>().isKinematic = true;
            }

            return State.Success;


        }
        return State.Failure;

    }
}
