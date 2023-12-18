using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TripShotCollider : MonoBehaviour
{
    TripShotTool _Tripshot;
    void Start()
    {
        _Tripshot = FindObjectOfType<TripShotTool>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CreatureBrain>() != null)
        {
           _Tripshot._Plunger.GetComponent<PlungerComponent>().SpawnElectrocute(other.ClosestPoint(this.transform.position));

            CreatureBrain crea = other.gameObject.GetComponent<CreatureBrain>();

            crea.isDizzy = true;

            // Removes Clone Creatures when you catch the real Creature
            CreatureObjectPooling _pooledCrea = other.gameObject.GetComponent<CreatureObjectPooling>();

            if (_pooledCrea)
            {
                for (int k = 0; k < _pooledCrea.pooledCreatures.Count; k++)
                {
                    _pooledCrea.pooledCreatures[k].GetComponent<NavMeshAgent>().enabled = false;
                    _pooledCrea.pooledCreatures[k].GetComponent<BehaviourTreeRunner>().enabled = false;
                    _pooledCrea.pooledCreatures[k].GetComponent<CloneBrain>().RemoveClone();

                }
                crea._HasActiveClones = false;
                crea._RunRandomly = false;
            }

            _Tripshot.PlungerDestroy();
        }
        else if(other.gameObject.GetComponent<PhysicsPlayerController>() != null || other.gameObject.tag == "Player")
        {
            _Tripshot._Plunger.GetComponent<PlungerComponent>().SpawnElectrocute(other.ClosestPoint(this.transform.position));

            _Tripshot.PlungerDestroy();
        }
    }
}
