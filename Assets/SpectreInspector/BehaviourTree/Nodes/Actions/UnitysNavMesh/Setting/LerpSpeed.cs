using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LerpSpeed : ActionNode
{
    NavMeshAgent agent;
    public float speed;

    public float _LerpSpeed = 1;

    float t = 0;

    protected override void OnStart()
    {
        //throw new System.NotImplementedException();

        if (agent == null)
        {
            agent = attachedGameObject.GetComponent<NavMeshAgent>();
        }

        //t = 0;

        //Debug.Log("RestartedLerp");
    }

    protected override void OnStop()
    {
        //throw new System.NotImplementedException();
    }

    protected override State OnUpdate()
    { 
        t += Time.deltaTime / _LerpSpeed;

        agent.speed = Mathf.Lerp(agent.speed, speed, t);

        Debug.Log("Speed: "+ agent.speed);

        if (agent.speed == speed)
        {
            t = 0;

            //return State.Success;
        }
        //throw new System.NotImplementedException();

        return State.Success;
    }
}
