using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsDizzy : ActionNode
{
    CreatureBrain brain;
    bool hasGottenBrain = false;

    protected override void OnStart()
    {
        if (!hasGottenBrain)
        {
            brain = attachedGameObject.GetComponent<CreatureBrain>();
            hasGottenBrain = true;
        }
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (brain.isDizzy)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }

    }
}
