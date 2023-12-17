using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RyanTestNode : ActionNode
{
    PlayerRadius _TrackingObject;

    protected override void OnStart()
    {
        //throw new System.NotImplementedException();

        if (_TrackingObject == null)
            _TrackingObject = FindObjectOfType<PlayerRadius>();

        if (_TrackingObject != null)
        {
            _TrackingObject.ChangePos();
        }
    }

    protected override void OnStop()
    {
        //throw new System.NotImplementedException();
    }

    protected override State OnUpdate()
    {
        //throw new System.NotImplementedException();

        return State.Success;
    }

  
}
