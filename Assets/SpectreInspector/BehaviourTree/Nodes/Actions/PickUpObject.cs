using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeCategory("[~] Holding Objects")]
public class PickUpObject : ActionNode
{
    GameObject closestObject;
    public float radius;


    protected override void OnStart()
    {
        GameObject[] taggedObjs;
        taggedObjs = GameObject.FindGameObjectsWithTag("PickUp");
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
            closestObject.transform.parent = attachedGameObject.transform;
            closestObject.transform.localPosition = new Vector3(0, 0, 1);
            closestObject.GetComponent<Rigidbody>().isKinematic = true;
            closestObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            return State.Success;
            

        }
        return State.Failure;

    }
}



