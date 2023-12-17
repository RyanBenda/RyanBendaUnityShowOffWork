using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckThrowDirection : ActionNode
{
    CreatureBrain _Brain;

    protected override void OnStart()
    {
        if (_Brain == null)
            _Brain = attachedGameObject.GetComponent<CreatureBrain>();
        _Brain.ThrowCheck();
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (!_Brain._InsuranceThrowCheck)
            return State.Success;

        return State.Running;
    }

    
}
