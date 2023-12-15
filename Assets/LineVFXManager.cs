using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class LineVFXManager : MonoBehaviour
{
    public VisualEffect[] _ElectAsset;
    [HideInInspector]
    public bool _InUse;

    [HideInInspector]
    public bool _Obtaining;

    public CreatureBrain _Creature;
    public TrapTool _TrapTool;

    
    public List<Transform> _BendPoints = new List<Transform>();

    public bool _GravGunVFX;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_InUse)
        {
            if (!_GravGunVFX)
            {
                for (int i = 0; i < _ElectAsset.Length; i++)
                {
                    _ElectAsset[i].SetVector3("Pos1", _BendPoints[0].position);
                    _ElectAsset[i].SetVector3("Pos2", _BendPoints[1].position);
                    _ElectAsset[i].SetVector3("Pos3", _BendPoints[2].position);
                    _ElectAsset[i].SetVector3("Pos4", _BendPoints[3].position);
                    _ElectAsset[i].SetVector3("Pos5", _BendPoints[4].position);
                    _ElectAsset[i].SetVector3("Pos6", _BendPoints[5].position);
                    if (!_Obtaining)
                        _ElectAsset[i].SetVector3("Pos7", _BendPoints[6].position);
                }
            }
            else
            {
                for (int i = 0; i < _ElectAsset.Length; i++)
                {
                    _ElectAsset[i].SetVector3("Pos1", _BendPoints[0].position);
                    _ElectAsset[i].SetVector3("Pos2", _BendPoints[1].position);
                    _ElectAsset[i].SetVector3("Pos3", _BendPoints[2].position);
                }
            }

        }
    } 
    
    public void SetUpVFX()
    {
        if (!_GravGunVFX)
        {
            for (int i = 0; i < _ElectAsset.Length; i++)
            {
                _ElectAsset[i].SetVector3("Pos1", _BendPoints[0].position);
                _ElectAsset[i].SetVector3("Pos2", _BendPoints[1].position);
                _ElectAsset[i].SetVector3("Pos3", _BendPoints[2].position);
                _ElectAsset[i].SetVector3("Pos4", _BendPoints[3].position);
                _ElectAsset[i].SetVector3("Pos5", _BendPoints[4].position);
                _ElectAsset[i].SetVector3("Pos6", _BendPoints[5].position);
                _ElectAsset[i].SetVector3("Pos7", _BendPoints[6].position);
            }
        }
        else
        {
            for (int i = 0; i < _ElectAsset.Length; i++)
            {
                _ElectAsset[i].SetVector3("Pos1", _BendPoints[0].position);
                _ElectAsset[i].SetVector3("Pos2", _BendPoints[1].position);
                _ElectAsset[i].SetVector3("Pos3", _BendPoints[2].position);
            }
        }

        _InUse = true;
    }

    public void HideAlpha(float t)
    {
        int val = 200 - (int)Mathf.Floor(t * 200);

        if (val < 0)
            val = 0;

        for (int i = 0; i < _ElectAsset.Length; i++)
        {
            _ElectAsset[i].SetInt("VisibleCount", val);
        }
    }

    public void ShowAlpha(float t)
    {
        int val = (int)Mathf.Floor(t * 200);

        if (val > 200)
            val = 200;

        for (int i = 0; i < _ElectAsset.Length; i++)
        {
            _ElectAsset[i].SetInt("VisibleCount", val);
        }
    }

}
