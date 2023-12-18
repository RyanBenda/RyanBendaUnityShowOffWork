using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBool : ActionNode
{
    public string _BoolName;
    public bool _Active;

    Animator animator;
    protected override void OnStart()
    {
        if (animator == null)
        {
            GameObject[] gameObjects = new GameObject[attachedGameObject.transform.childCount];

            for (int i = 0; i < attachedGameObject.transform.childCount; i++)
            {
                if (attachedGameObject.transform.GetChild(i).GetComponent<Animator>() != null)
                {
                    animator = attachedGameObject.transform.GetChild(i).GetComponent<Animator>();
                }
            }
        }
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {

        animator.SetBool(_BoolName, _Active);

        return State.Success;
    }
}