using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[NodeCategory("Teleport Position")]
public class TeleportBehindPlayer : ActionNode
{
    GameObject player;
    public float distance = 2f;

    protected override void OnStart()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {


        attachedGameObject.transform.position = player.transform.position + -player.transform.forward * distance;

        return State.Success;
    }
}
