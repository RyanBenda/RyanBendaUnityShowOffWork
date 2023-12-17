using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasLineofSightToPlayer : ActionNode
{
    public LayerMask layerMask;

    PhysicsPlayerController player;

    protected override void OnStart()
    {
        if (player == null)
            player = FindObjectOfType<PhysicsPlayerController>();
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(attachedGameObject.transform.position, player.transform.position - attachedGameObject.transform.position, out hit, 100, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.tag == "Player")
            {
                return State.Success;
            }
        }

        return State.Failure;
    }
}
