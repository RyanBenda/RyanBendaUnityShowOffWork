using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[NodeCategory("Sound")]
public class PlayRandomSound : ActionNode
{
    AudioSource audioSource;
    public AudioClip[] clips;
    public bool interuptSoundsToPlay = false;

    public bool _PitchShift = false;
    protected override void OnStart()
    {
        audioSource = attachedGameObject.GetComponent<AudioSource>();
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        int number = Random.Range(0, clips.Length);

        if (_PitchShift)
            audioSource.pitch = Random.Range(0.75f, 1);

        if (!audioSource.isPlaying && !interuptSoundsToPlay)
        {
            audioSource.clip = clips[number];
            audioSource.Play();
        }
        else if (interuptSoundsToPlay)
        {
            audioSource.clip = clips[number];
            audioSource.Play();
        }


        return State.Success;
    }
}
