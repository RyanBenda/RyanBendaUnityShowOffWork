using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeCategory("[~] Snax-Like")]
public class CountTillFail : ActionNode
{
    public int CountToPoint = 3;
    int Count = 0;

    protected override void OnStart()
    {
        Count++;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if(Count >= CountToPoint)
        {
            Count = 0;
            return State.Failure;
        }
        else
        {
            return State.Success;
        }
    }


}
