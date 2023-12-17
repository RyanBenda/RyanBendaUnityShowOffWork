using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[NodeCategory("[!] Pathfinding Agent")]
public class StopPathfinding : ActionNode
{
    NavMeshAgent agent;

    protected override void OnStart()
    {
        agent = attachedGameObject.GetComponent<NavMeshAgent>();

        agent.destination = attachedGameObject.transform.position;

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        return State.Success;
    }

}
