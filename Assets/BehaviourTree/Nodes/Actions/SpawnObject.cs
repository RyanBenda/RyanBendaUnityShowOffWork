using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeCategory("Spawner")]

public class SpawnObject : ActionNode
{
    public GameObject spawnObject;
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        Instantiate(spawnObject, attachedGameObject.transform.position, Quaternion.identity);

        return State.Success;
    }

}
