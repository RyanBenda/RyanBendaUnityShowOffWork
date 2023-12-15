using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[NodeCategory("Can See [Blank]?")]
public class CanPlayerSeeChar : ActionNode
{ 
    public int maxRays = 10;
    public float FOV = 60;

    float halfFOV;
    float max;
    float min;
    float rayEveryF;

    GameObject player;

    protected override void OnStart()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //FOV = Camera.main.fieldOfView;

        halfFOV = FOV / 2;
        rayEveryF = FOV / maxRays;

        max = attachedGameObject.transform.rotation.y + halfFOV;
        min = attachedGameObject.transform.rotation.y - halfFOV;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        float current = min;

        //Vector3 directon = attachedGameObject.transform.forward; 

        //directon.

        for (int i = 1; i < maxRays; i++)
        {

            var dir = Quaternion.Euler(-attachedGameObject.transform.position.y * Vector3.Distance(attachedGameObject.transform.position, player.transform.position), current, 0) * player.transform.forward;

            Debug.DrawRay(player.transform.position, dir * 10, Color.yellow);

            RaycastHit hit;
            if (Physics.Raycast(player.transform.position, dir, out hit, Mathf.Infinity))
            {
                Debug.DrawRay(player.transform.position, dir * 10, Color.magenta);
                if (hit.collider.gameObject)
                {
                    if (hit.collider.gameObject == attachedGameObject)
                    {
                        return State.Success;
                    }
                }
            }
            current += rayEveryF;

        }
        return State.Failure;
    }


}
