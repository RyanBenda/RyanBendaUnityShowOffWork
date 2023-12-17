using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeKinematic : ActionNode
{

    public bool status = false;

    protected override void OnStart()
    {
        attachedGameObject.GetComponent<Rigidbody>().isKinematic = status;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}
