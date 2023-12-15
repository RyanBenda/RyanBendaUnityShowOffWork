using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAndPlayParticleEffect : ActionNode
{
    public float duration = 1;
    float startTime;

    ParticleSystem particleSystem;
    protected override void OnStart()
    {
        startTime = Time.time;

        if (particleSystem == null)
            particleSystem = attachedGameObject.GetComponent<ParticleSystem>();

        particleSystem.Play();
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (Time.time - startTime > duration)
        {
            return State.Success;
        }

        return State.Running;
    }
}
