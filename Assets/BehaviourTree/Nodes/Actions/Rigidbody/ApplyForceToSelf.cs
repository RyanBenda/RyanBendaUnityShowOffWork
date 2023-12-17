using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[NodeCategory("[~] Rigidbody")]
public class ApplyForceToSelf : ActionNode
{
    GameObject gameObject;
    public float sideForce;
    public float forwardForce;


    public float randMin = -2.5f;
    public float randMax = 2.5f;

    protected override void OnStart()
    {
        gameObject = attachedGameObject;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {

        float rando = Random.Range(randMin, randMax);


        gameObject.GetComponent<Rigidbody>().AddForce(attachedGameObject.transform.right * (sideForce + rando), ForceMode.Impulse);
        gameObject.GetComponent<Rigidbody>().AddForce(attachedGameObject.transform.forward * forwardForce, ForceMode.Impulse);

        return State.Success;
    }

}
