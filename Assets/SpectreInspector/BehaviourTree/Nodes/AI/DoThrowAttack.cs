using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoThrowAttack : ActionNode
{
    ThrowObjectComponent throwObjectComponent;
    protected override void OnStart()
    {
        if (throwObjectComponent == null)
            throwObjectComponent = attachedGameObject.GetComponent<ThrowObjectComponent>();

        throwObjectComponent._AttackPlayer = true;
    }


    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (!throwObjectComponent._AttackPlayer)
            return State.Failure;

        return State.Running;
    }

   
}
