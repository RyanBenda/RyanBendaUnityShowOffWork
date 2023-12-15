using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureAnimations : MonoBehaviour
{
    public CreatureBrain _Crea;
    public BearFollowingComponent _Bear;

    public GameObject _Phone;
    public Animator _Duck;
    public GameObject _DuckObject;

    public Animator _Box;
    public GameObject _BoxObject;

    public void EndSpin()
    {
        _Crea.GetComponent<NavMeshAgent>().enabled = true;
        _Crea._PerformingAction = false;
        _Crea._CreatureAnimator.SetBool("Spin", false);
    }

    public void StartDuck()
    {
        //_Duck.gameObject.SetActive(true);
        _Duck.SetBool("Animating", true);
        //_Crea._CanBeTrappedCurrently = true;
    }

    public void ShowDuck()
    {
        _DuckObject.SetActive(true);
    }   
    
    public void HideDuck()
    {
        _DuckObject.SetActive(false);
    }

    public void EndDuck()
    {
        //BearFollowingComponent b = _Crea.gameObject.GetComponent<BearFollowingComponent>();
        //_Bear._BearBase.SetActive(true);
        //_Bear._BearDuck.SetActive(false);

        
        //_Bear._Animator.SetBool("Idle", true);
        _Bear._Animator.SetBool("Duck", false);

        _Crea.GetComponent<NavMeshAgent>().enabled = true;
        _Crea._PerformingAction = false;
        //_Duck.gameObject.SetActive(false);
        _Duck.SetBool("Animating", false);
        //_Crea._CanBeTrappedCurrently = false;
    }

    public void StartPhone()
    {
        _Phone.SetActive(true);
        //_Crea._CanBeTrappedCurrently = true;
    }

    public void EndPhone()
    {
        //BearFollowingComponent b = _Crea.gameObject.GetComponent<BearFollowingComponent>();
        //_Bear._BearBase.SetActive(true);
        //_Bear._BearDuck.SetActive(false);


        //_Bear._Animator.SetBool("Idle", true);
        _Bear._Animator.SetBool("Phone", false);

        _Crea.GetComponent<NavMeshAgent>().enabled = true;
        _Crea._PerformingAction = false;
        _Phone.SetActive(false);
        //_Duck.SetBool("Animating", false);
        //_Crea._CanBeTrappedCurrently = false;
    }

    public void StartToyBox()
    {
        //_Duck.gameObject.SetActive(true);
        _Box.SetBool("Animating", true);
        //_Crea._CanBeTrappedCurrently = true;
    }

    public void ShowToyBox()
    {
        _BoxObject.SetActive(true);
    }

    public void HideToyBox()
    {
        _BoxObject.SetActive(false);
    }

    public void EndToyBox()
    {
        //BearFollowingComponent b = _Crea.gameObject.GetComponent<BearFollowingComponent>();
        //_Bear._BearBase.SetActive(true);
        //_Bear._BearDuck.SetActive(false);


        //_Bear._Animator.SetBool("Idle", true);
        _Bear._Animator.SetBool("ToyBox", false);

        _Crea.GetComponent<NavMeshAgent>().enabled = true;
        _Crea._PerformingAction = false;
        //_Duck.gameObject.SetActive(false);
        _Box.SetBool("Animating", false);
        //_Crea._CanBeTrappedCurrently = false;
    }
}
