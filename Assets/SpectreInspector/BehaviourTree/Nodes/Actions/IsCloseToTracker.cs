using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IsCloseToTracker : ActionNode
{

    public Trackable tracker;

    GameObject closestObject;
    public float radius;

    public bool applyTagToObject;
    public Trackable applyTag;

    TrackerHolder[] Objs;


    protected override void OnStart()
    {

        if (Objs == null)
        {
            Objs = GameObject.FindObjectsOfType<TrackerHolder>();
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
                            if (closestObject == null)
                            {
                                closestObject = holder.gameObject;
                            }
                            float a = Vector3.Distance(holder.transform.position, attachedGameObject.transform.position);


                            if (holder.GetComponent<Rigidbody>())
                            {
                                if (a <= closest && holder.GetComponent<Rigidbody>().velocity.magnitude <= 1.5)
                                {
                                    closest = a;
                                    closestObject = holder.transform.gameObject;
                                }
                            }
                            else if (!holder.GetComponent<Rigidbody>())
                            {
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
        }

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (closestObject != null)
        {

            float distance = Vector3.Distance(closestObject.transform.position, attachedGameObject.transform.position);

            if (distance <= radius)
            {
                if (applyTag != null)
                {
                    bool haveapplyTag = false;
                    foreach (Trackable trac in closestObject.GetComponent<TrackerHolder>().trackers)
                    {
                        if (trac.name == applyTag.name)
                        {
                            haveapplyTag = true;
                        }
                    }
                    if (haveapplyTag)
                    {
                        closestObject.GetComponent<TrackerHolder>().trackers.Add(applyTag);
                    }



                }

                return State.Success;
            }

        }
        return State.Failure;

    }
}
