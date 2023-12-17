using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RunRandomly : ActionNode
{
    
    CreatureBrain _brain;
    CloneBrain _cloneBrain;
    int cloneBrainChecks = 0;
    NavMeshAgent agent;
    ThrowObjectComponent _thrower;
    public LayerMask layerMask;
    public float moveDistance = 5;
    public float maxTimeOnPath = 3;
    float realmaxTimeOnPath;

   

    protected override void OnStart()
    {
        if (_brain == null && _brain == null)
            _brain = attachedGameObject.GetComponent<CreatureBrain>();

        if (agent == null)
            agent = attachedGameObject.GetComponent<NavMeshAgent>();

        if (cloneBrainChecks == 0 && _cloneBrain == null)
        {
            cloneBrainChecks = 1;
            _cloneBrain = attachedGameObject.GetComponent<CloneBrain>();
        }

        if (_thrower == null)
            _thrower = attachedGameObject.GetComponent<ThrowObjectComponent>();

        Debug.Log("CALLED");

        GenNewPos();

        _brain._RunRandomly = true;
        realmaxTimeOnPath = maxTimeOnPath;
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (_cloneBrain)
        {
            if (_cloneBrain._HasBeenCaught)
            {
                agent.destination = attachedGameObject.transform.position;
                return State.Success;
            }
        }


        if(_brain.isDizzy == true)
        {
            return State.Failure;
        }


        if (_brain._RunRandomly)
        {
            if (!agent.pathPending)
            {
                if (agent.enabled == true && agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        GenNewPos();
                        realmaxTimeOnPath = maxTimeOnPath;
                    }
                }
            }

           
            

            realmaxTimeOnPath -= Time.deltaTime;

            if (realmaxTimeOnPath <= 0)
            {
                realmaxTimeOnPath = maxTimeOnPath;
                GenNewPos();
            }

            return State.Running;
        }
        else
        {
            agent.destination = attachedGameObject.transform.position;
            return State.Success;
        }

    }

    void GenNewPos()
    {
        Vector3 direction = Random.insideUnitSphere;

        //direction.x = 0;
        direction.y = 0;

        RaycastHit hit;
        if (Physics.Raycast(attachedGameObject.transform.position, direction, out hit, moveDistance, layerMask))
        {
            agent.destination = hit.point;
        }
        else
            agent.destination = attachedGameObject.transform.position + direction * moveDistance;

        //Debug.DrawLine(attachedGameObject.transform.position, attachedGameObject.transform.position + direction * moveDistance, Color.red);

        

    }

}
