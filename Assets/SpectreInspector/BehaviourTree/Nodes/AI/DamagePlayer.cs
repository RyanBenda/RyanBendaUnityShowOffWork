using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : ActionNode
{
    protected override void OnStart()
    {
        PlayerStats.instance.ReduceHealth();
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}
