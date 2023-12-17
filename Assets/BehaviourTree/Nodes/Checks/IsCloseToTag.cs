using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeCategory("[?] Is Close To [Blank]?")]
public class IsCloseToTag : ActionNode
{
    public string Tag;
    GameObject closestObject;
    public float radius;


    protected override void OnStart()
    {
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

            float distance = Vector3.Distance(closestObject.transform.position, attachedGameObject.transform.position);

            if (distance <= radius)
            {
                return State.Success;
            }

        }
        return State.Failure;
        
    }
}
