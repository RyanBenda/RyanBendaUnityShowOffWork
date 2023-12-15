using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[NodeCategory("[~] Rigidbody/[!]Edit Properties")]
public class EditMass : ActionNode
{
    Rigidbody body;

    public float mass = 1f;

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
        body.mass = mass;

        return State.Success;
    }

}
