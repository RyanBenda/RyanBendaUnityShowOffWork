using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[NodeCategory("[~] Holding Objects")]
public class IsHolding : ActionNode
{
    public Trackable tracker;

    int childrenCount;
    protected override void OnStart()
    {
        childrenCount = attachedGameObject.transform.childCount;

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (childrenCount != 0)
        {
            for (int i = 0; i < childrenCount; i++)
            {
                if(attachedGameObject.transform.GetChild(i).GetComponent<TrackerHolder>())
                {
                    TrackerHolder TH = attachedGameObject.transform.GetChild(i).GetComponent<TrackerHolder>();
                    foreach (Trackable a in TH.trackers)
                    {
                        if (a == tracker)
                        {
                            return State.Success;
                        }
                    }
                }

                

            }
        }
        return State.Failure;
    }


}
