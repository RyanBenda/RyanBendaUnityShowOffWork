using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[NodeCategory("[!] Pathfinding Agent")]
public class WalkToClosestTag : ActionNode
{
    NavMeshAgent agent;

    public string Tag;
    GameObject closestObject;
    public float radius;


    protected override void OnStart()
    {
        agent = attachedGameObject.GetComponent<NavMeshAgent>();

        GameObject[] taggedObjs;
        taggedObjs = GameObject.FindGameObjectsWithTag(Tag);

        if (taggedObjs.Length != 0)
        {
            float closest = Vector3.Distance(taggedObjs[0].transform.position, attachedGameObject.transform.position);
            foreach (GameObject obj in taggedObjs)
            {
                float a = Vector3.Distance(obj.transform.position, attachedGameObject.transform.position);

                if (a <= closest)
                {
                    closest = a;
                    closestObject = obj;
                }
            }
        }

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (closestObject != null)
        {
            agent.destination = closestObject.transform.position;
            return State.Success;
        }
        else
        {
            return State.Failure;
        }

        
    }

}
