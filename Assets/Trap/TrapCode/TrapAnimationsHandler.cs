using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAnimationsHandler : MonoBehaviour
{
    public TrapTool _Trap;
    //public GameObject _TrapModel;

    int i = 0;

    List<Transform> _LineRendererChildren = new List<Transform>();

    public void ActivateBounce()
    {
        _Trap._TrapLineRenderer._IsAnimating = false;

        for (int i = 0; i < _LineRendererChildren.Count; i++)
        {
            _LineRendererChildren[i].parent = _Trap._TrapLineRenderer.transform;
        }
        _LineRendererChildren.Clear();

        _Trap._TrapAnimator.SetBool("Capturing", false);
        if (_Trap._CreaturesCaught.Count > 0)
            _Trap._TrapAnimator.SetBool("Caught", true);
    }

    public void ChangeLineReneder()
    {
        _Trap._TrapLineRenderer._IsAnimating = true;
        for (int i = 0; i < _Trap._TrapLineRenderer.transform.childCount;)
        {
            _LineRendererChildren.Add(_Trap._TrapLineRenderer.transform.GetChild(0));
            _Trap._TrapLineRenderer.transform.GetChild(0).parent = null;
        }
    }

    public void TrapExploded()
    {
       
            
    }

    public void ReleaseGhosts()
    {
        if (_Trap._TrapAnimator.GetBool("Escape") == true)
            _Trap.RetrieveTrap();
    }
}
