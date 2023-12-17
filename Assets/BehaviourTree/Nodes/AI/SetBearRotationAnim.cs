using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBearRotationAnim : ActionNode
{

    Transform _Player;

    Animator _Animator;

    protected override void OnStart()
    {
        if (_Player == null)
        {
            _Player = FindObjectOfType<PhysicsPlayerController>().transform;
        }

        if (_Animator == null)
        {
            GameObject[] gameObjects = new GameObject[attachedGameObject.transform.childCount];

            for (int i = 0; i < attachedGameObject.transform.childCount; i++)
            {
                if (attachedGameObject.transform.GetChild(i).GetComponent<Animator>() != null)
                {
                    _Animator = attachedGameObject.transform.GetChild(i).GetComponent<Animator>();
                }
            }
        }


        


        Vector3 _DirOfAim;
        

        _DirOfAim = new Vector3(_Player.position.x, attachedGameObject.transform.position.y, _Player.position.z) - attachedGameObject.transform.position;

        Vector3 cross = Vector3.Cross(attachedGameObject.transform.forward, _DirOfAim);

        if (cross.y < -1)
        {
            //_Animator.SetBool("TurningRight", true);
        }
        else if (cross.y > 1)
        {
            //_Animator.SetBool("TurningLeft", true);
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
