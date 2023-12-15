using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[NodeCategory("[~] Rigidbody/[!]Edit Properties")]
public class EditKinematic : ActionNode
{
    Rigidbody body;

    public bool useKinematic = true;

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
        body.isKinematic = useKinematic;

        return State.Success;
    }
}
