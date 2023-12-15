using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IsAgentAtDest : ActionNode
{

    NavMeshAgent agent;


    protected override void OnStart()
    {
        if (agent == null)
        {
            agent = attachedGameObject.GetComponent<NavMeshAgent>();
        }
    }

    protected override void OnStop()
    {
        //throw new System.NotImplementedException();
    }

    protected override State OnUpdate()
    {
        if (!agent.pathPending)
        {
            if (agent.enabled == true && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return State.Success;
                }
            }
        }

        return State.Failure;
    }
}
