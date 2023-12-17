using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoActionTillTime : DecoratorNode
{
    float timer = 0;
    public float runChildTillTime = 10;
    public bool timerActive = true;
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (timer < runChildTillTime)
        {
            timer += Time.deltaTime;
            if (timer >= runChildTillTime)
            {
                timer = 0;
                return State.Failure;
            }
        }
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
}

