using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureBrain : MonoBehaviour
{
    public bool inRangeOfTrap = false;
    public bool isDizzy = false;
    public bool _PerformingAction = false;
    public GameObject _DizzyGO;
    public float _dizzyForTime = 5;
    public int PhysPropHitsTillDizzy = 1;
    public int CurrentHitTotal = 0;



    public bool _CanBeStunnedWithCamera = false;
    public bool _CanBeTrappedCurrently = true;

    //public Trackable[] _Likes;
    //public Trackable[] _Dislikes;

    float timer;

    public CreatureScriptableObject _CreatureIdentity;

    public CreatureEmotions _CurEmotion;
    public Animator _CreatureAnimator;


    public GameObject _Player;
    public CreatureObjectPooling _CreatureObjectPool;
    public bool _HasActiveClones;
    public bool _RunRandomly;
    public bool _IsClone;

    public bool _IsFrightened = false;
    public bool _AutoCapture = false;

    //public BaseObjective PhotoQuest;

    

    [HideInInspector]
    public bool _InsuranceThrowCheck = false;
    bool _Rotate;
    Vector3 _PrevDir;
    Vector3 _Dir;
    float _T;
    public LayerMask _ThrowDirection;
    public GhostRoom _CurRoom;

    bool _WaitTillNewHit = false;
    float _WaitASec = 0;
    

    private void OnDestroy()
    {
        TrackerTool.CameraInstance._Creatures.Remove(this);
    }

    public bool IsInFrustumPlanes()
    {
        var bounds = GetComponent<Collider>().bounds;

        if (GeometryUtility.TestPlanesAABB(TrackerTool.CameraInstance.planes, bounds))
        {
            return true;
        }
        else
            return false;
    }

    private void Start()
    {
        if(_Player == null)
        {
            _Player = GameObject.FindGameObjectWithTag("Player");

        }
        TrackerTool.CameraInstance._Creatures.Add(this);
    }


    public void Update()
    {
        if (_WaitTillNewHit)
        {
            _WaitASec += Time.deltaTime;

            if (_WaitASec >= 1)
            {
                _WaitTillNewHit = false;
                _WaitASec = 0;
            }
        }

        if(isDizzy && !_DizzyGO.activeInHierarchy)
        {
            _DizzyGO.SetActive(true);
        }
        else if(!isDizzy && _DizzyGO.activeInHierarchy)
        {
            _DizzyGO.SetActive(false);
        }

        if (isDizzy && timer < _dizzyForTime)
        {
            timer += Time.deltaTime;
        }
        else if(timer >= _dizzyForTime)
        {
            isDizzy = false;
            CurrentHitTotal = 0;
            timer = 0;
            if (_CreatureAnimator)
                _CreatureAnimator.SetBool("Dizzy", false);
        }   

        if (_InsuranceThrowCheck)
        {
            _T += Time.deltaTime * 2;

            this.transform.forward = Vector3.Lerp(_PrevDir, _Dir, _T);

            if (_T >= 1)
                _InsuranceThrowCheck = false;
        }

        if(CurrentHitTotal >= PhysPropHitsTillDizzy)
        {
            CurrentHitTotal = 0;
            isDizzy = true;
            //_CreatureAnimator.SetBool("Dizzy", true);
            if (_HasActiveClones)
            {
                CreatureObjectPooling _pooledCrea = this.GetComponent<CreatureObjectPooling>();

                if (_pooledCrea)
                {
                    for (int k = 0; k < _pooledCrea.pooledCreatures.Count; k++)
                    {
                        //_pooledCrea.pooledCreatures[k].GetComponent<CloneBrain>()._HasBeenCaught = true;
                        //_pooledCrea.pooledCreatures[k].gameObject.SetActive(false);
                        _pooledCrea.pooledCreatures[k].GetComponent<NavMeshAgent>().enabled = false;
                        //_pooledCrea.pooledCreatures[k].GetComponent<BehaviourTreeRunner>().enabled = false;
                        _pooledCrea.pooledCreatures[k].GetComponent<CloneBrain>().RemoveClone();

                    }
                    _HasActiveClones = false;
                    _RunRandomly = false;
                }

                
            }

            
        }
    }

    public void CameraStun()
    {
        if (_CanBeStunnedWithCamera)
        {
            isDizzy = true;
        }


        //CheckPhotoQuest();

    }

    

    public void ThrowCheck()
    {
        _PrevDir = transform.forward;
        _Dir = -transform.forward;
        RaycastHit hit;

        for (int i = 0; i < 8; i++)
        {
            Vector3 newDir = Quaternion.AngleAxis(45 * i, transform.up) * transform.forward;

            Debug.DrawRay(this.transform.position, _Dir, Color.red);

            if (!Physics.Raycast(this.transform.position, newDir, out hit, 2, _ThrowDirection))
            {
                _Dir = newDir;
                break;
            }
            else
            {
                Debug.Log("HitNAME:" + hit.transform.name);
            }
        }

        _InsuranceThrowCheck = true;
        _T = 0;
    }


    // So the behaviour tree can call for a mission objective to complete depending on the action of a ghost.
    // For example: spooking peebo can complete an objective to guide the player to the answer of a puzzle. - Lee
    


    private void OnCollisionEnter(Collision collision)
    {
        if (!_WaitTillNewHit && _CreatureIdentity._Name == "GiGi" && collision.gameObject.tag == "Player")
        {
            if (_CreatureAnimator.GetBool("Punching") == true || _CreatureAnimator.GetBool("Scared"))
            {
                _WaitTillNewHit = true;
                PlayerStats.instance.ReduceHealth();
            }
        }
    }
}
