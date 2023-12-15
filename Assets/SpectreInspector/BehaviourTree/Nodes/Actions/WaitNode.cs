using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeCategory("Time")]
public class WaitNode : ActionNode
{
    public float duration = 1;
    float startTime;

    public bool _randomGeneration;
    public Vector2 _MinMax = new Vector2(0, 1);

    protected override void OnStart()
    {
       startTime = Time.time;

        if (_randomGeneration)
            duration = Random.Range(_MinMax.x, _MinMax.y);

    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (Time.time - startTime > duration)
        {
            return State.Success;
        }
        return State.Running;
    }
}
