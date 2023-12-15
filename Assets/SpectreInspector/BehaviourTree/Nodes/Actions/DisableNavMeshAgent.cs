using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DisableNavMeshAgent : ActionNode
{

    NavMeshAgent agent;
    bool _HasGottenAgent = false;

    protected override void OnStart()
    {
        if (!_HasGottenAgent)
        { 
            agent = attachedGameObject.GetComponent<NavMeshAgent>();
            _HasGottenAgent = true;
        }

        agent.enabled = false;
        
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}
