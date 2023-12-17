using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


[NodeCategory("[~] Transform")]
public class LookAtPlayer : ActionNode
{
    GameObject player;
    public float speed = 1;
    protected override void OnStart()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        Vector3 vector3 = new Vector3 (player.transform.position.x, attachedGameObject.transform.position.y , player.transform.position.z);

        Vector3 dir = player.transform.position - attachedGameObject.transform.position;
        dir.y = 0; // keep the direction strictly horizontal
        Quaternion rot = Quaternion.LookRotation(dir);
        // slerp to the desired rotation over time
        attachedGameObject.transform.rotation = Quaternion.Slerp(attachedGameObject.transform.rotation, rot, speed * Time.deltaTime);
   
        return State.Success;
    }

}
