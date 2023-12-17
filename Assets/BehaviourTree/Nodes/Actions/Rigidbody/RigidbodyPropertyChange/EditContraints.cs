using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[NodeCategory("[~] Rigidbody/[!]Edit Properties")]
public class EditContraints : ActionNode
{

    public bool TransformX;
    public bool TransformY;
    public bool TransformZ;

    public bool RotationX;
    public bool RotationY;
    public bool RotationZ;

    Rigidbody rigidbody;

    protected override void OnStart()
    {
        if (rigidbody == null)
            rigidbody = attachedGameObject.GetComponent<Rigidbody>();


        rigidbody.constraints = RigidbodyConstraints.None;

        if(TransformX)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
        }
        if (TransformY)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        }
        if(TransformZ)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
        }
        if(RotationX)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationX;
        }
        if(RotationY)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationY;
        }
        if(RotationZ)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
        }

    }

    protected override void OnStop()
    {
       
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }


}
