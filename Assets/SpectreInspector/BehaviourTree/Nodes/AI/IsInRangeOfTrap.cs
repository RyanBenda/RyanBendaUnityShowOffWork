using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsInRangeOfTrap : ActionNode
{
    CreatureBrain brain;
    protected override void OnStart()
    {
        if (brain == null)
            brain = attachedGameObject.GetComponent<CreatureBrain>();
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (brain.inRangeOfTrap)
            return State.Success;

        return State.Failure;

    }
}
