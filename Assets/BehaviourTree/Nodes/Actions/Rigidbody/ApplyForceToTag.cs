using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeCategory("[~] Rigidbody/ Unity Tag System")]
public class ApplyForceToTag : ActionNode
{
    public Vector3 trajectory;
    public float force = 5;
    public float forceToSelf = 5;
    public string tagOfTargetedObj = "Player";

    GameObject gameObject;

    Rigidbody body;
    protected override void OnStart()
    {
        if (attachedGameObject.GetComponent<Rigidbody>() != null)
        {
            body = attachedGameObject.GetComponent<Rigidbody>();
        }

        gameObject = GameObject.FindGameObjectWithTag(tagOfTargetedObj);
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {


        //Vector3 Force2Obj = (attachedGameObject.transform.forward * trajectory.x) + (attachedGameObject.transform.up * trajectory.y).normalized * force * Time.deltaTime;


        gameObject.GetComponent<Rigidbody>().AddForce(attachedGameObject.transform.forward * trajectory.x * force);
        gameObject.GetComponent<Rigidbody>().AddForce(attachedGameObject.transform.up * trajectory.y * force);

        return State.Success;
    }
}
