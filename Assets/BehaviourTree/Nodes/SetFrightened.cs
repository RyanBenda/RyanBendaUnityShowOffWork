using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFrightened : ActionNode
{
    CreatureBrain creatureBrain;
    public bool changeTo = false;

    protected override void OnStart()
    {
        if(creatureBrain == null)
        {
            creatureBrain = attachedGameObject.GetComponent<CreatureBrain>();
        }
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        creatureBrain._IsFrightened = changeTo;
        return State.Success;
    }

}
