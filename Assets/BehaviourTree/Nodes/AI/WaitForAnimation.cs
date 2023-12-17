using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class WaitForAnimation : ActionNode
{
    CreatureBrain _Creature;
    public string _AnimName;
    bool _StartedAnim;

    protected override void OnStart()
    {
        if (_Creature == null)
            _Creature = attachedGameObject.GetComponent<CreatureBrain>();
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        //Debug.Log("Current Anim: " + _Creature._CreatureAnimator.c)
        

        if (AnimatorIsPlaying(_AnimName))
        {
            _StartedAnim = true;
        }
        
        if (_StartedAnim && !AnimatorIsPlaying(_AnimName))
        {
            _StartedAnim = false;
            return State.Success;
        }
        else
            return State.Running;
    }

    bool AnimatorIsPlaying()
    {
        return _Creature._CreatureAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1
            /*_Creature._CreatureAnimator.GetCurrentAnimatorStateInfo(0).length >
               _Creature._CreatureAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime*/;
    }

    bool AnimatorIsPlaying(string stateName)
    {
        return AnimatorIsPlaying() && _Creature._CreatureAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
}
