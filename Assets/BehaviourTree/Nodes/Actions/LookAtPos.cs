using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[NodeCategory("[~] Transform")]
public class LookAtPos : ActionNode
{
    public Vector3 pos;
    public float speed = 1;
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {

        Vector3 dir = pos - attachedGameObject.transform.position;
        dir.y = 0; // keep the direction strictly horizontal
        Quaternion rot = Quaternion.LookRotation(dir);
        // slerp to the desired rotation over time
        attachedGameObject.transform.rotation = Quaternion.Slerp(attachedGameObject.transform.rotation, rot, speed * Time.deltaTime);

        return State.Success;
    }
}
