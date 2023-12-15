using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasActiveClones : ActionNode
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
        if (_Creature._HasActiveClones)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
