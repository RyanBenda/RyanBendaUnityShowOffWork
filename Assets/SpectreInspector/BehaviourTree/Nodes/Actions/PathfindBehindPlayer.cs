using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[NodeCategory("[!] Pathfinding Agent")]
public class PathfindBehindPlayer : ActionNode
{

    NavMeshAgent agent;
    CreatureBrain brain;
    GameObject player;
    public float distance = 2f;

    protected override void OnStart()
    {
        agent = attachedGameObject.GetComponent<NavMeshAgent>();
        brain = attachedGameObject.GetComponent<CreatureBrain>();
        player = GameObject.FindGameObjectWithTag("Player");

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {

        if (!brain._PerformingAction)
            agent.destination =  player.transform.position + -player.transform.forward * distance;

        return State.Success;
    }

}
