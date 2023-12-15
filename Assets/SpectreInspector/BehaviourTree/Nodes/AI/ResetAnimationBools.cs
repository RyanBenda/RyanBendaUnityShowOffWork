using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimationBools : ActionNode
{
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
        //throw new System.NotImplementedException();
    }

    protected override State OnUpdate()
    {

        for (int i = 0; i < animator.parameterCount; i++)
        {
            if (animator.parameters[i].type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(i, false);
            }
        }

        return State.Success;
    }
}
