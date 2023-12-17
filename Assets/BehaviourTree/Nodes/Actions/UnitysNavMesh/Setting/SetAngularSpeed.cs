using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[NodeCategory("[~] Unity Navmesh Agents/[!]Edit Properties")]
public class SetAngularSpeed : ActionNode
{

    NavMeshAgent agent;
    public float angularSpeed;

    protected override void OnStart()
    {

        if (agent == null)
        {
            agent = attachedGameObject.GetComponent<NavMeshAgent>();
        }

    }

    protected override void OnStop()
    {

    }
    protected override State OnUpdate()
    {
        agent.angularSpeed = angularSpeed;
        return State.Success;

    }
}
