using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[NodeCategory("[~] Transform")]
public class DeleteSelf : ActionNode
{
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        Destroy(attachedGameObject);
        return State.Success;
    }

}
