using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[NodeCategory("[!] Pathfinding Agent")]
public class NavmeshWalkNode : ActionNode
{

    public Vector3[] patrolPoints = new Vector3[4];
    int currentPatrolPoint;
    public float minDistanceToPoint = 1.5f;
    NavMeshAgent agent;


    public float radius;


    GameObject player;

    protected override void OnStart()
    {
        agent = attachedGameObject.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");


        currentPatrolPoint = Random.Range(0, patrolPoints.Length);
        agent.destination = patrolPoints[currentPatrolPoint];
 


    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {

         float dist = Vector3.Distance(attachedGameObject.transform.position, patrolPoints[currentPatrolPoint]);
         if (dist < minDistanceToPoint)
         {
            return State.Success;
         }


        return State.Success;

    }

}
