using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeCategory("[~] Holding Objects")]
public class BowlTheBall : ActionNode
{
    public Vector3 trajectory;

    public float force = 15f;
    int childrenCount;
    protected override void OnStart()
    {
        childrenCount = attachedGameObject.transform.childCount;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (childrenCount != 0)
        {
            for (int i = 0; i < childrenCount; i++)
            {
                if (attachedGameObject.transform.GetChild(i).tag == "PickUp")
                {
                    GameObject gameObject = attachedGameObject.transform.GetChild(i).gameObject;
                    gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    gameObject.GetComponent<Rigidbody>().AddForce(attachedGameObject.transform.forward + trajectory * force, ForceMode.Impulse);
                    gameObject.transform.parent = null;

                    return State.Success;
                }
            }
        }
        return State.Failure;
    }
}

