using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[NodeCategory("[~] Rigidbody/[?]Check Properties")]
public class IsKinematic : ActionNode
{
    Rigidbody body;

    protected override void OnStart()
    {
        if (body == null)
        {
            body = attachedGameObject.GetComponent<Rigidbody>();
        }
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (body.isKinematic == true)
        {
            return State.Success;
        }
        else { return State.Failure; }
    }

}
