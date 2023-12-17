using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeCategory("[~] Rigidbody/[!]Edit Properties")]
public class EditGravity : ActionNode
{
    Rigidbody body;

    public bool useGravity = true;

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
        body.useGravity = useGravity;

        return State.Success;
    }
}
