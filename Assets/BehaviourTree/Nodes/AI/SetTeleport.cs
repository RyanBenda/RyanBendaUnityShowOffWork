using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTeleport : ActionNode
{
    CreatureObjectPooling _TpData;

    public bool _CanTeleport = false;

    protected override void OnStart()
    {
        if (_TpData == null)
            _TpData = attachedGameObject.GetComponent<CreatureObjectPooling>();

        if (_TpData)
        {
            _TpData._ChooseNewLocation = _CanTeleport;
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
