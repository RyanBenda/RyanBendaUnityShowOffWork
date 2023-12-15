using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[NodeCategory("[!] Pathfinding Agent")]

public class FollowPathNode : ActionNode
{
    public Vector3[] patrolPoints = new Vector3[4];
    int currentPatrolPoint = 0;
    NavMeshAgent agent;
    public float howCloseTo = 1.5f;

    protected override void OnStart()
    {
        agent = attachedGameObject.GetComponent<NavMeshAgent>();
        agent.destination = patrolPoints[0];

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        //if (attachedGameObject.transform.position.x == patrolPoints[currentPatrolPoint].x && attachedGameObject.transform.position.z == patrolPoints[currentPatrolPoint].z)
        //{
        //    return State.Success;
        //}
        agent.destination = patrolPoints[currentPatrolPoint];

        float dist = Vector3.Distance(attachedGameObject.transform.position, patrolPoints[currentPatrolPoint]);
        if (dist <= howCloseTo && currentPatrolPoint < patrolPoints.Length && currentPatrolPoint + 1 != patrolPoints.Length)
        {

            currentPatrolPoint++;
            agent.destination = patrolPoints[currentPatrolPoint];
        }
        else if (dist <= howCloseTo && currentPatrolPoint == patrolPoints.Length - 1)
        {
            currentPatrolPoint = 0;
            return State.Success;
        }

        return State.Success;

    }
}
