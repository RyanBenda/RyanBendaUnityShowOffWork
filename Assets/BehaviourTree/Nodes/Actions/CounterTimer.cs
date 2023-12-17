using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterTimer : DecoratorNode
{
    float timer = 0;
    public float runChildAtTime = 10;
    public bool timerActive = true;
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (timerActive)
        {
            timer += Time.deltaTime;
            if (timer >= runChildAtTime)
            {
                timer = 0;
                timerActive = false;
            }
        }

        if (!timerActive)
        {
            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Failure:
                    timerActive = true;
                    return State.Failure;
                case State.Success:
                    timerActive = true;
                    return State.Success;
                default:
                    timerActive = true;
                    return State.Success;
            }
        }

        return State.Failure;

    }
}
