using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[NodeCategory("Teleport Position")]
public class TeleportToLocation : ActionNode
{
    public Vector3 tpLocation;
    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        attachedGameObject.transform.position = tpLocation;
        return State.Success;
    }

}
