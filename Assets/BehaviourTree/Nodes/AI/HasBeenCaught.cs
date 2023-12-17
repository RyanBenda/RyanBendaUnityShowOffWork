using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasBeenCaught : ActionNode
{
    CloneBrain _Cbrain;
    protected override void OnStart()
    {
        if (_Cbrain == null)
        {
            _Cbrain = attachedGameObject.GetComponent<CloneBrain>();
        }
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (_Cbrain._HasBeenCaught)
        {
            _Cbrain._HasBeenCaught = false;
            return State.Failure;
        }


        return State.Success;
    }
}
