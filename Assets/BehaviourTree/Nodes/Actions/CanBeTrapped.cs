using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBeTrapped : ActionNode
{
    CreatureBrain brain;
    public bool _Result;
    protected override void OnStart()
    {
        if (brain == null)
        {
            brain = attachedGameObject.GetComponent<CreatureBrain>();
        }

        brain._CanBeTrappedCurrently = _Result;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {


        return State.Success;


    }
}
