using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatForDuration : DecoratorNode
{
    public float duration = 1;
    float startTime;
    protected override void OnStart()
    {
        startTime = Time.time;
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        


        switch (child.Update())
        {
            case State.Running:
                return State.Running;
            case State.Failure:
                if (Time.time - startTime > duration)
                {
                    return State.Success;
                }
                return State.Running;
            case State.Success:
                if (Time.time - startTime > duration)
                {
                    return State.Success;
                }
                return State.Running;
            default:
                return State.Success;
        }
    }

    
}
