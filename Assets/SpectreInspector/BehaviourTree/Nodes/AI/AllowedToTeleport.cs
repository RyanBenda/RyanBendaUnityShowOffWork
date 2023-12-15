using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowedToTeleport : ActionNode
{
    CreatureObjectPooling _TpData;

    protected override void OnStart()
    {
        if (_TpData == null)
            _TpData = attachedGameObject.GetComponent<CreatureObjectPooling>();
    }

    protected override void OnStop()
    {
        /*if (_TpData)
            _TpData._ChooseNewLocation = false;
        else if (_TpData2)
            _TpData2._ChooseNewLocation = false;*/
    }

    protected override State OnUpdate()
    {
        if (_TpData)
        {
            if (_TpData._ChooseNewLocation)
                return State.Success;
        }

        return State.Failure;
    }  
}
