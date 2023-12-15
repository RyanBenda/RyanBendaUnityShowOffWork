using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureInteractionManager : MonoBehaviour
{
    void SpinAction(CreatureBrain Crea)
    {
        Crea._CreatureAnimator.SetBool("Spin", true);
    }

    void PickupDuckAction(CreatureBrain Crea)
    {
        BearFollowingComponent b = Crea.GetComponent<BearFollowingComponent>();
        //b._BearBase.SetActive(false);
        //b._BearDuck.SetActive(true);
        b._Animator.SetBool("Duck", true);
    }

    void PickupToyBoxAction(CreatureBrain Crea)
    {
        BearFollowingComponent b = Crea.GetComponent<BearFollowingComponent>();
        b._Animator.SetBool("ToyBox", true);
    }

    void CallPhoneAction(CreatureBrain Crea)
    {
        BearFollowingComponent b = Crea.GetComponent<BearFollowingComponent>();
        //b._BearBase.SetActive(false);
        //b._BearDuck.SetActive(true);
        b._Animator.SetBool("Phone", true);
    }

    public void DetermineAction(CreatureBrain Crea, CreatureActions Action)
    {
        if (Action == CreatureActions.PickupDuck)
            PickupDuckAction(Crea);
        if (Action == CreatureActions.PickupToyBox)
            PickupToyBoxAction(Crea);
        if (Action == CreatureActions.CallPhone)
            CallPhoneAction(Crea);
    }
}
