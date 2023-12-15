using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeCategory("[?] Is Close To [Blank]?")]
public class IsCloseToPlayer : ActionNode
{
    GameObject player;
    public float radius;

    protected override void OnStart()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        float distance = Vector3.Distance(attachedGameObject.transform.position, player.transform.position);

        if (distance <=  radius)
        {
            Debug.Log("player is near me. " + distance);
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }

}
