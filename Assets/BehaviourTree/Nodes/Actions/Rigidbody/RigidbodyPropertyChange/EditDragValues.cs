using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[NodeCategory("[~] Rigidbody/[!]Edit Properties")]
public class EditAngularDragValues : ActionNode
{
    Rigidbody body;
    public float angularDrag = 1f;


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
        body.angularDrag = angularDrag;

        return State.Success;
    }

}
