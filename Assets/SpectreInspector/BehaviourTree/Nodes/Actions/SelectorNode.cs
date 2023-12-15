using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectorNode : CompositeNode
{
    int current;


    protected override void OnStart()
    {
        current = 0;

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        var child = children[0];

        switch (child.Update())
        {
            case State.Running:
                return State.Running;
            case State.Failure:
                current++;
                break;
            case State.Success:
                return State.Failure;
        }

        return current == children.Count ? State.Success : State.Running;
    }
}
