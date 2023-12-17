using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HasPlayerStoppedMoving : ActionNode
{
    PhysicsPlayerController _Player;

    bool _IsMoving;

    protected override void OnStart()
    {
        if (_Player == null)
            _Player = attachedGameObject.GetComponent<CreatureBrain>()._Player.GetComponent<PhysicsPlayerController>();
    }

    protected override void OnStop()
    {
        //throw new System.NotImplementedException();
    }

    protected override State OnUpdate()
    {
        if (_IsMoving == true && _Player.moving == false)
        {
            _IsMoving = false;
            return State.Success;
        }
        else if (_Player.moving == true)
        {
            _IsMoving = true;
        }

        return State.Failure;
        //throw new System.NotImplementedException();
    }


}
