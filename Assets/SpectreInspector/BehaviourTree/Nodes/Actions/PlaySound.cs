using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeCategory("Sound")]
public class PlaySound : ActionNode
{
    AudioSource audioSource;
    public AudioClip clip;
    public bool interuptSoundsToPlay = false;
    protected override void OnStart()
    {
        audioSource = attachedGameObject.GetComponent<AudioSource>();
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (!audioSource.isPlaying && !interuptSoundsToPlay)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else if (interuptSoundsToPlay)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }


        return State.Success;
    }
}
