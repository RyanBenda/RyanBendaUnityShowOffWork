using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[NodeCategory("[~] Transform")]
public class IsAtPos : ActionNode
{
    public Vector3 pos;
    public float closeToPoint = 1;
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {

        float distance = Vector3.Distance(pos, attachedGameObject.transform.position);

        if (distance <= closeToPoint)
        {
            return State.Success;
        }

        return State.Failure;
    }
}
