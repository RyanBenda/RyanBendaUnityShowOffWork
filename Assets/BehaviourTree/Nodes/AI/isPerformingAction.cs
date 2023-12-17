using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class isPerformingAction : ActionNode
{
    CreatureBrain _Creature;

    protected override void OnStart()
    {
        if (_Creature == null)
            _Creature = attachedGameObject.GetComponent<CreatureBrain>();
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (_Creature._PerformingAction)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
