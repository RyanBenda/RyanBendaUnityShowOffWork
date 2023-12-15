using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnableNavMeshAgent : ActionNode
{
    NavMeshAgent agent;


    protected override void OnStart()
    {
        if (agent == null)
        {
            agent = attachedGameObject.GetComponent<NavMeshAgent>();
        }

        agent.enabled = true;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}
