using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerMoving : ActionNode
{
    GameObject player;
    public float _MinimumSpeed;

    protected override void OnStart()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

    }

    protected override void OnStop()
    {
        //throw new System.NotImplementedException();
    }

    protected override State OnUpdate()
    {
        float speed = player.GetComponent<Rigidbody>().velocity.magnitude;

        if (speed >= _MinimumSpeed)
        {
            //Debug.Log("player is near me. " + distance);
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }

 
}
