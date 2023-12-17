using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public enum CreatureActions
{
    PickupDuck,
    PickupToyBox,
    CallPhone
}

public class CreatureInteractable : MonoBehaviour
{
    CreatureInteractionManager _CreatureInteractionManager;

    public CreatureActions _Action;

    public List<Transform> _CreaturePosition = new List<Transform>();

    [HideInInspector]
    public bool _InUse;
    [HideInInspector]
    public CreatureBrain _Creature;
    [HideInInspector]
    public Vector3 _CreaturePoint;
    public Quaternion _CreatureRotation;

    Vector3 _CreatureStartPos;
    Quaternion _CreatureStartRot;
    protected float startTime;
    protected float journeyLength;

    [HideInInspector]
    public float _InterpSped = 7;

    bool _Rotating;
    bool _Moving;
    bool _Facing;
    bool _LerpsDone;

    float t;
    float t2;

    public float _DelayTillReUse;
    [HideInInspector]
    public float _DelayTillReUseReal;

    [HideInInspector]
    public NavMeshAgent _CreaAgent;

    private void Awake()
    {
        _CreatureInteractionManager = FindObjectOfType<CreatureInteractionManager>();
        
    }

    public Vector3 FindClosestPos(Vector3 creaturePos)
    {
        float largeDist = 100;
        Vector3 goToPos = _CreaturePosition[0].position;

        for (int i = 0; i < _CreaturePosition.Count; i++)
        {
            float dist = Vector3.Distance(creaturePos, _CreaturePosition[i].position);

            if (dist < largeDist)
            {
                largeDist = dist;
                goToPos = _CreaturePosition[i].position;
            }    
        }

        Debug.Log("GoToPos: " + goToPos);
        return goToPos;
    }

    private void Update()
    {
        if (_InUse)
        {
            /*if (_Rotating)
            {
                //float distCovered = (Time.time - startTime) * _InterpSped;
                //float fractionOfJourney = distCovered / journeyLength;
                t += Time.deltaTime;


                _Creature.transform.rotation = Quaternion.Slerp(_Creature.transform.rotation, _CreatureRotation, t);

                if (t >= 1 || _Creature.transform.rotation == _CreatureRotation)
                {
                    BeginMoveLerp();
                }
            }
            else if (_Moving)
            {
                float distCovered = (Time.time - startTime) * _InterpSped;
                float fractionOfJourney = distCovered / journeyLength;

                float yVal = _Creature.transform.position.y;
                _Creature.transform.position = Vector3.Lerp(_CreatureStartPos, _CreaturePoint, fractionOfJourney);
                _Creature.transform.position = new Vector3(_Creature.transform.position.x, yVal, _Creature.transform.position.z);

                if (_Creature.transform.position.x == _CreaturePoint.x && _Creature.transform.position.z == _CreaturePoint.z)
                {
                    BeginFaceLerp();
                }
            }
            else if (_Facing)
            {
                t2 += Time.deltaTime;

                var lookPos = Camera.main.transform.position - _Creature.transform.position;
                lookPos.y = 0;
                Quaternion curRot = Quaternion.LookRotation(lookPos);

                _Creature.transform.rotation = Quaternion.Slerp(_Creature.transform.rotation, curRot, t2);

                if (t2 >= 1 || _Creature.transform.rotation == _CreatureRotation)
                {
                    _Facing = false;
                    _LerpsDone = true;
                }
            }
            else if (_LerpsDone)
            {
                //_Creature.transform.LookAt(Camera.main.transform);
                _CreatureInteractionManager.DetermineAction(_Creature, _Action);
                t2 = 0;
                _InUse = false;
                _LerpsDone = false;
                _DelayTillReUseReal = _DelayTillReUse;
            }*/

            if (!_CreaAgent.pathPending)
            {
                if (_CreaAgent.enabled == true && _CreaAgent.remainingDistance <= _CreaAgent.stoppingDistance)
                {
                    if (!_CreaAgent.hasPath || _CreaAgent.velocity.sqrMagnitude == 0f)
                    {
                        _CreatureInteractionManager.DetermineAction(_Creature, _Action);
                        _InUse = false;
                        _DelayTillReUseReal = _DelayTillReUse;
                    }
                }
            }
        }
        else if (_DelayTillReUseReal >= 0)
        {
            _DelayTillReUseReal -= Time.deltaTime;
        }
    }

    public void BeginRotLerp()
    {
        
        //_InUse = true;
        _Rotating = true;

        var lookPos = _CreaturePoint - _Creature.transform.position;
        lookPos.y = 0;
        _CreatureRotation = Quaternion.LookRotation(lookPos);
        //_CreatureStartRot = _Creature.transform.rotation;
        //startTime = Time.time;
        //journeyLength = Vector3.Distance(_CreatureStartPos, _CreaturePoint);
        
    }

    void BeginMoveLerp()
    {
        t = 0;
        _Rotating = false;
        _Moving = true;

        _CreatureStartPos = _Creature.transform.position;
        startTime = Time.time;
        journeyLength = Vector3.Distance(_CreatureStartPos, _CreaturePoint);
    }

    void BeginFaceLerp()
    {
        _Moving = false;
        _Facing = true;
    }
}
