using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeCategory("[?] Is Close To [Blank]?")]
public class IsCloseToScript : ActionNode
{


    //public string scriptName;
    ////public string script;


    //GameObject closestObject;
    //public float radius;




    //protected override void OnStart()
    //{
    //    //This is a bad hack job but Unity has forced my hand.
    //    // TODO:
    //    // MAKE SURE THIS DOESNT RUN ON START ALL THE TIME OH GOD.

    //    Type scriptLookingFor = Type.GetType(scriptName);
    //    if (scriptLookingFor == null)
    //    {
    //        throw new Exception("NOT FOUND!");
    //    }

    //    Type[] trackableObjs;
    //    trackableObjs = FindObjectsOfType<scriptLookingFor>();

    //    List<Trackable> trackableObjs2 = new List<Trackable>();

    //    for (int i = 0; i < trackableObjs.Length; i++)
    //    {
    //        if (trackableObjs[i] == script)
    //        {
    //            trackableObjs2.Add(trackableObjs[i]);
    //        }
    //    }

    //    if (trackableObjs2.Count != 0)
    //    {
    //        float closest = Vector3.Distance(trackableObjs2[0].transform.position, attachedGameObject.transform.position);
    //        foreach (Trackable obj in trackableObjs2)
    //        {
    //            float a = Vector3.Distance(obj.transform.position, attachedGameObject.transform.position);

    //            if (a < closest)
    //            {
    //                closest = a;
    //                closestObject = obj.gameObject;
    //            }
    //        }
    //    }

    //}

    //protected override void OnStop()
    //{

    //}

    //protected override State OnUpdate()
    //{
    //    if (closestObject != null)
    //    {

    //        float distance = Vector3.Distance(closestObject.transform.position, attachedGameObject.transform.position);

    //        if (distance <= radius)
    //        {
    //            return State.Success;
    //        }

    //    }
    //    return State.Failure;

    //}
    protected override void OnStart()
    {
        throw new NotImplementedException();
    }

    protected override void OnStop()
    {
        throw new NotImplementedException();
    }

    protected override State OnUpdate()
    {
        throw new NotImplementedException();
    }
}
