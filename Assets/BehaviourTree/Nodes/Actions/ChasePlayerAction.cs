using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[NodeCategory("[!] Pathfinding Agent")]
public class ChasePlayerAction : ActionNode
{

    NavMeshAgent agent;
    GameObject player;

    protected override void OnStart()
    {
        agent = attachedGameObject.GetComponent<NavMeshAgent>();
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }


    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (agent.enabled)
            agent.destination = player.transform.position;

        return State.Success;
    }
}
