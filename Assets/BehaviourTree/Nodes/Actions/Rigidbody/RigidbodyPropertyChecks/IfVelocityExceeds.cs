using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[NodeCategory("[~] Rigidbody/[?]Check Properties")]
public class IfVelocityExceeds : ActionNode
{
    public float maxVelocity = 5f;
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
        if (body.velocity.magnitude > maxVelocity)
        {
            return State.Success;
        }
        else { return State.Failure; }
    }

}
