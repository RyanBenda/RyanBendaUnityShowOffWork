using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeCategory("[~] Rigidbody")]
public class ApplyForceTowardPlayer : ActionNode
{
    GameObject gameObject;
    GameObject player;


    public float sideForce;
    public float forwardForce;
    public float upForce;
    protected override void OnStart()
    {
        gameObject = attachedGameObject;

        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(attachedGameObject.transform.right * sideForce, ForceMode.Impulse);
        gameObject.GetComponent<Rigidbody>().AddForce(attachedGameObject.transform.forward * forwardForce, ForceMode.Impulse);
        gameObject.GetComponent<Rigidbody>().AddForce(attachedGameObject.transform.up * upForce, ForceMode.Impulse);

        return State.Success;
    }
}
