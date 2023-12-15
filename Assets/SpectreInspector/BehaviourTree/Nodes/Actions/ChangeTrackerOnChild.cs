using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTrackerOnChild : ActionNode
{
    public bool adding  = true;
    public Trackable trackable;

    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        int childrenCount = attachedGameObject.transform.childCount;

        if (childrenCount != 0)
        {
            for (int i = 0; i < childrenCount; i++)
            {
                if (attachedGameObject.transform.GetChild(i).GetComponent<TrackerHolder>())
                {
                    TrackerHolder TH = attachedGameObject.transform.GetChild(i).GetComponent<TrackerHolder>();

                    bool isTrackableThere = false;
                    foreach(Trackable trac in TH.trackers)
                    {
                        if (trac != null)
                        {
                            if (trac.name == trackable.name)
                            {
                                if (adding)
                                {
                                    isTrackableThere = true;
                                }
                                if (!adding)
                                {
                                    TH.trackers.Sort();
                                    TH.trackers.Remove(trac);
                                }
                            }
                        }

                    }

                    if (adding && !isTrackableThere)
                    {
                        TH.trackers.Add(trackable);
                    }

                }

            }
        }
        return State.Failure;
    }
}
