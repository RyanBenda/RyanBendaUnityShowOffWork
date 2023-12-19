using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CloneBrain : MonoBehaviour
{
    public SkinnedMeshRenderer _hat;
    public CreatureObjectPooling _Pooler;
    [HideInInspector]
    public bool _HasBeenCaught = false;

    [SerializeField] private GameObject _ParticleSystemPrefab;
    float T;
    bool removeClone;
    [HideInInspector]
    public bool _BeingSetInactive;

    public NavMeshAgent _Agent;
    [SerializeField] private CreatureBrain _Brain;
    bool _RemoveClone;

    [SerializeField] private SpawnTrackingComponent _SpawnTrackingComponent;

    [SerializeField] private bool _AllowAttack = true;

    // Start is called before the first frame update
    void Awake()
    {
        if ( _Agent == null )
            _Agent = GetComponent<NavMeshAgent>();
        if (_Brain == null)
            _Brain = GetComponent<CreatureBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        if (T > 0)
            T -= Time.deltaTime;
        else if (removeClone)
        {
            removeClone = false;
            _HasBeenCaught = true;
            _BeingSetInactive = false;
            GetComponent<BehaviourTreeRunner>().enabled = true;
            GetComponent<NavMeshAgent>().enabled = true;
            _RemoveClone = false;
            _Brain.isDizzy = false;

            if (_SpawnTrackingComponent == null)
                _SpawnTrackingComponent = this.GetComponent<SpawnTrackingComponent>();

            if (_SpawnTrackingComponent != null)
                _SpawnTrackingComponent.RemoveAllTracks();

            this.gameObject.SetActive(false);
            
        }
        if (!_RemoveClone && _Brain.isDizzy)
        {
            _RemoveClone = true;
            RemoveClone();
            SetClonesToAttack();
        }
    }

    public void RemoveClone()
    {
        if (this.gameObject.activeSelf != false)
        {
            GameObject particle = Instantiate(_ParticleSystemPrefab);
            particle.transform.position = this.transform.position;
            Destroy(particle, 5);

            _BeingSetInactive = true;
            removeClone = true;
            T = 1;
        }
        else
        {
            GetComponent<BehaviourTreeRunner>().enabled = true;
            GetComponent<NavMeshAgent>().enabled = true;
            _RemoveClone = false;
            _Brain.isDizzy = false;
        }
    }

    public void SetClonesToAttack()
    {

        if(_AllowAttack)
        {
            int inactiveclones = 0;

            for (int j = 0; j < _Pooler.pooledCreatures.Count; j++)
            {
                _Pooler.pooledCreatures[j].GetComponent<CreatureBrain>()._RunRandomly = false;

                if (_Pooler.pooledCreatures[j].gameObject.activeSelf == false || _Pooler.pooledCreatures[j].GetComponent<CloneBrain>()._BeingSetInactive)
                    inactiveclones++;
            }

            if (inactiveclones > 2)
            {
                _Pooler.gameObject.GetComponent<CreatureBrain>()._HasActiveClones = false;
                _Pooler.gameObject.GetComponent<CreatureBrain>()._RunRandomly = false;

                if (_Pooler._CreatureTeleports)
                    _Pooler._ChooseNewLocation = true;
            }
        }
    }
}
