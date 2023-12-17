using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BearFollowingComponent : MonoBehaviour
{

    public NavMeshAgent _Agent;
    [HideInInspector]
    public Animator _Animator;
    Transform _Player;
    CreatureBrain _Brain;

    public float _RotateSpeed = 2;
    public float _RadiansSpeed = 2;
    public float _DeltaSpeed = 2;

    bool _AtDest;

    public GameObject _BearBase;
    public GameObject _BearDuck;



    //public Animator DuckAnimator;

    // Start is called before the first frame update
    void Start()
    {
        if (_Agent == null)
            _Agent = GetComponent<NavMeshAgent>();

        _Brain = GetComponent<CreatureBrain>();

        _Player = _Brain._Player.transform;

        _Animator = _Brain._CreatureAnimator;
    }

    
    // Update is called once per frame
    void Update()
    {
        if (_Brain._PerformingAction)
        {
            Vector3 _DirOfAim;
            Vector3 newDirection;

            _DirOfAim = new Vector3(_Player.position.x, this.transform.position.y, _Player.position.z) - this.transform.position;

            newDirection = Vector3.RotateTowards(this.transform.forward, _DirOfAim, _RadiansSpeed, _DeltaSpeed);

            Vector3 cross = Vector3.Cross(this.transform.forward, _DirOfAim);

            if (_Animator.GetBool("TurningLeft") == false && _Animator.GetBool("TurningRight") == false)
            {
                _Animator.SetBool("Walking", false);



                if (cross.y < 0)
                {
                    _Animator.SetBool("TurningRight", true);
                }
                else if (cross.y > 0)
                {
                    _Animator.SetBool("TurningLeft", true);
                }
            }



            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(newDirection), _RotateSpeed);

            if (this.transform.rotation == Quaternion.LookRotation(newDirection))
            {
                _Brain._PerformingAction = false;
            }
        }

        //Debug.Log("anim playing: " + AnimatorIsPlaying("Bear_Phone_Anim").ToString());

        /*if (!_Agent.pathPending)
        {
            Vector3 _DirOfAim;
            Vector3 newDirection;

            if (_Agent.enabled == true && _Agent.remainingDistance <= _Agent.stoppingDistance)
            {
                if (!_Agent.hasPath || _Agent.velocity.sqrMagnitude == 0f)
                {
                    _AtDest = true;

                    _DirOfAim = new Vector3(_Player.position.x, this.transform.position.y, _Player.position.z) - this.transform.position;

                    newDirection = Vector3.RotateTowards(this.transform.forward, _DirOfAim, _RadiansSpeed, _DeltaSpeed);

                    Vector3 cross = Vector3.Cross(this.transform.forward, _DirOfAim);

                    if (_Animator.GetBool("TurningLeft") == false && _Animator.GetBool("TurningRight") == false)
                    {
                        _Animator.SetBool("Walking", false);

                        

                        if (cross.y > 0)
                        {
                            _Animator.SetBool("TurningRight", true);
                        }
                        else if (cross.y < 0)
                        {
                            _Animator.SetBool("TurningLeft", true);
                        }
                    }

                    

                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(newDirection), _RotateSpeed);

                    if (cross.y <= 0.5f && cross.y >= -0.5f)
                    {
                        _Animator.SetBool("TurningRight", false);
                        _Animator.SetBool("TurningLeft", false);
                        _Animator.SetBool("Idle", true);
                    }
                }
                else
                {
                    _AtDest = false;
                    _Animator.SetBool("TurningRight", false);
                    _Animator.SetBool("TurningLeft", false);
                }
            }
            else
            {
                _AtDest = false;
                _Animator.SetBool("TurningRight", false);
                _Animator.SetBool("TurningLeft", false);
            }


            if (!_AtDest && _Brain._PerformingAction == false)
            {
                _DirOfAim = new Vector3(_Agent.destination.x, this.transform.position.y, _Agent.destination.z) - this.transform.position;

                newDirection = Vector3.RotateTowards(this.transform.forward, _DirOfAim, _RadiansSpeed, _DeltaSpeed);

                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(newDirection), _RotateSpeed);
            }

        }*/
    }
}
