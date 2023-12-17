using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureInteractionTrigger : MonoBehaviour
{
    public CreatureInteractable _Interactor;

    private void OnTriggerEnter(Collider other)
    {
        BearFollowingComponent bear = other.gameObject.GetComponent<BearFollowingComponent>();
        

        if (bear != null && _Interactor._DelayTillReUseReal <= 0)
        {
            CreatureBrain crea = other.gameObject.GetComponent<CreatureBrain>();
            NavMeshAgent creaAgent = other.gameObject.GetComponent<NavMeshAgent>();

            crea._PerformingAction = true;


            Vector3 dest = _Interactor.FindClosestPos(other.transform.position);


            // Why wont it work unless I call it twice???????????
            //creaAgent.destination = dest;
            //creaAgent.destination = dest;
            creaAgent.destination = dest;
            creaAgent.destination = dest;
            _Interactor._CreaAgent = creaAgent;

            _Interactor._InUse = true;
            _Interactor._Creature = crea;
            //_Interactor._CreaturePoint = dest;
            //_Interactor.BeginRotLerp();
            //creaAgent.angularSpeed = 0;
        }
    }
}
