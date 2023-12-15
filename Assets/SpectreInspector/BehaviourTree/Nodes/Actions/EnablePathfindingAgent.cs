using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnablePathfindingAgent : ActionNode
{
    public bool isEnabled = true;

    NavMeshAgent navMeshAgent;
    protected override void OnStart()
    {
        if (navMeshAgent == null)
        {
            navMeshAgent = attachedGameObject.GetComponent<NavMeshAgent>();
        }
    }

    protected override void OnStop()
    {
   
    }

    protected override State OnUpdate()
    {

        navMeshAgent.enabled = isEnabled;
        return State.Success;
    }


}
