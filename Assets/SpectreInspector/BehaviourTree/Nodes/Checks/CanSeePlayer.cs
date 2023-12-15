using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


[NodeCategory("Can See [Blank]?")]
public class CanSeePlayer : ActionNode
{

    public int maxRays = 10;
    public float FOV = 60;

    float halfFOV;
    float max;
    float min;
    float rayEveryF;

    GameObject player;

    public LayerMask _CheckForPlayer;

    protected override void OnStart()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

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

            var dir = Quaternion.Euler(-player.transform.position.y * Vector3.Distance(player.transform.position, attachedGameObject.transform.position), current, 0) * attachedGameObject.transform.forward;

            Debug.DrawRay(attachedGameObject.transform.position, dir  * 10, Color.yellow);

            RaycastHit hit;
            if (Physics.Raycast(attachedGameObject.transform.position, dir, out hit, Mathf.Infinity, _CheckForPlayer))
            {
                Debug.DrawRay(attachedGameObject.transform.position, dir * 10, Color.magenta);
                if (hit.collider.gameObject)
                {
                    if (hit.collider.gameObject.tag == "Player")
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
