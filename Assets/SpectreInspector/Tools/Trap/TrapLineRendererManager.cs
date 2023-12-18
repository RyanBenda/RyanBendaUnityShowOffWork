using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using System.Linq;
//using UnityEditor.PackageManager;
//using UnityEditor.IMGUI.Controls;

[RequireComponent(typeof(LineRenderer))]
public class TrapLineRendererManager : MonoBehaviour
{
    [HideInInspector]
    public LineRenderer _LineRenderer;

    public TrapTool _TrapTool;

    [SerializeField] private float _LineBends = 2;

    public List<Transform> _BendPoints = new List<Transform>();

    public CreatureBrain _Creature;

    [SerializeField] private int _Resolution = 50;

    [SerializeField] private float _MidPointHeight = 20;

    [SerializeField] private float _PathTravelTime = 2;
    [SerializeField] private float _LineDrawTime = 2;

    bool _FollowingPath = false;

    float _T;
    float _LineT;

    float _DurDivRes;

    List<Vector3> _LineRendererPath = new List<Vector3>();

    [SerializeField] private LayerMask _CollisionRaycast;

    [HideInInspector]
    public bool _VerifiedPath = false;

    [SerializeField] private int _NumOfChecks = 100;

    public bool _InUse;
    bool _LineFormed;
    bool _ClosedTrap;

    public bool _IsAnimating;

    public LineVFXManager _LineVFX;

    void Awake()
    {
        _LineRenderer = GetComponent<LineRenderer>();
        _LineRenderer.positionCount = _Resolution;

        for (int i = 0; i < _LineBends; i++)
        {
            GameObject objToSpawn = new GameObject("Bend" + i);
            objToSpawn.transform.parent = this.transform;
            objToSpawn.transform.localPosition = Vector3.zero;
            _BendPoints.Add(objToSpawn.transform);
        }  
    }

    void Update()
    {
        if (_InUse)
        {
            // Tries to form a path around obstacles to pull ghost in
            // if it fails to find a path within a set amount it will go with default route
            int checks = 0;
            while (!_VerifiedPath)
            {
                _LineRenderer.positionCount = _Resolution;
                SetBends();
                DrawLineRendererPath();
                CheckPath();

                checks++;

                if (checks == _NumOfChecks)
                {
                    SetStraightBends();
                    DrawLineRendererPath();
                    _VerifiedPath = true;
                }
            }

            // Drawing line to destination
            if (_VerifiedPath && _LineRenderer.positionCount != 0 && _LineFormed == false)
            {
                _LineT += Time.deltaTime / _LineDrawTime;

                Gradient c = _LineRenderer.colorGradient;

                GradientAlphaKey[] alphaKeys = c.alphaKeys;

                alphaKeys[alphaKeys.Length - 1].time = Mathf.Lerp(0, 1, _LineT);

                c.alphaKeys = alphaKeys;

                _LineRenderer.colorGradient = c;

                _LineVFX.ShowAlpha(_LineT);

                if (_LineT >= 1)
                {
                    _LineFormed = true;
                    GenerateTweenPath();
                    _LineVFX._Obtaining = true;

                    _LineVFX._BendPoints.Clear();
                    _LineVFX._BendPoints.Add(this.transform);
                    for (int j = 0; j < _BendPoints.Count; j++)
                        _LineVFX._BendPoints.Add(_BendPoints[j]);
                    _LineVFX._BendPoints.Add(_Creature.transform);

                    _LineVFX.SetUpVFX();
                }
            }

            // pulls creature along path
            if (_FollowingPath && _T < 1)
            {
                _T += Time.deltaTime / _PathTravelTime;

                _Creature.transform.position = AnyraticLerp(_LineRendererPath, _T);

                _LineVFX.HideAlpha(_T);

                while (_T >= _DurDivRes / _PathTravelTime)
                {
                    _DurDivRes += _PathTravelTime / _Resolution;

                    if (_LineRenderer.positionCount > 0)
                        _LineRenderer.positionCount = _LineRenderer.positionCount - 1;

                }

                if (_T > 0.8f && !_ClosedTrap)
                {
                    _ClosedTrap = true;
                    _TrapTool.CatchCreatures();
                }

                if (_LineRenderer.positionCount == 0)
                {
                    _LineRenderer.positionCount = 0;
                }

                if (_T >= 1)
                {
                    _InUse = false;
                }
            }

            DrawLineRendererPath();
        }
    }

    void GenerateTweenPath()
    {
        if (_LineRenderer.positionCount != 0)
        {
            _FollowingPath = true;
            _DurDivRes = _PathTravelTime / _Resolution;
            _T = 0;

            _Creature.GetComponent<NavMeshAgent>().enabled = false;

            _LineRenderer.positionCount = _Resolution;
            _LineRendererPath.Clear();

            for (int i = 0; i < _Resolution; i++)
            {
                _LineRendererPath.Add(_LineRenderer.GetPosition(_LineRenderer.positionCount - (i + 1)));
            }
            _LineRenderer.positionCount = _LineRenderer.positionCount - 1;

            Vector3 scale = _Creature.transform.localScale;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(_Creature.transform.DOScale(Vector3.zero, _PathTravelTime));
            sequence.Append(_Creature.transform.DOScale(scale, 0));
            sequence.Play();
        }
    }

    void CheckPath()
    {
        for (int i = 0; i < _LineRenderer.positionCount - 1; i++)
        {
            // RayCast cuts the path early if it collides with another object
            RaycastHit hit;
            if (Physics.SphereCast(_LineRenderer.GetPosition(i), 0.1f, _LineRenderer.GetPosition(i + 1) - _LineRenderer.GetPosition(i), out hit, Vector3.Distance(_LineRenderer.GetPosition(i + 1), _LineRenderer.GetPosition(i)), _CollisionRaycast))
            {
                _VerifiedPath = false;

                return;
            }
            else if (i == _Resolution - 2)
            {
                _VerifiedPath = true;

                _LineVFX._BendPoints.Clear();
                _LineVFX._BendPoints.Add(this.transform);
                for (int j = 0; j < _BendPoints.Count; j++)
                    _LineVFX._BendPoints.Add(_BendPoints[j]);
                _LineVFX._BendPoints.Add(_Creature.transform);

                _LineVFX.SetUpVFX();
            }
        }
    }

    void DrawLineRendererPath()
    {
        Vector3 launchpos = this.transform.position;
        List<Vector3> vector3s = new List<Vector3>();

        vector3s.Add(this.transform.position);

        for (int i = 0; i < _BendPoints.Count; i++)
            vector3s.Add(_BendPoints[i].position);

        Vector3 previousDrawPoint = launchpos;

        // sets positions for Line Renderer
        for (int i = 0; i < _LineRenderer.positionCount - 1; i++)
        {
            float simulationTime = i / (float)_Resolution;

            Vector3 drawPoint = AnyraticLerp(vector3s, simulationTime);

            _LineRenderer.SetPosition(i, previousDrawPoint);
            _LineRenderer.SetPosition(i + 1, drawPoint);

            previousDrawPoint = drawPoint;
        }     
    }

    // randomly generates anchors for lerp path to make the effect look more interesting
    void SetBends()
    {
        float val = _BendPoints.Count / 2;
        val = Mathf.Ceil(val);

        float yHeight = _MidPointHeight / val;

        for (int i = 0; i < _BendPoints.Count; i++)
        {
            float xPos = Random.Range(this.transform.position.x, _Creature.transform.position.x);

            float yPos;
            if (i > val)
                yPos = Random.Range(this.transform.position.y, _Creature.transform.position.y);
            else
            {
                yPos = this.transform.position.y + yHeight;

                yHeight += yHeight;
            }

            float zPos = Random.Range(this.transform.position.z, _Creature.transform.position.z);

            _BendPoints[i].position = new Vector3(xPos, yPos, zPos);
        }

        Gradient c = _LineRenderer.colorGradient;

        GradientAlphaKey[] alphaKeys = c.alphaKeys;

        alphaKeys[alphaKeys.Length - 1].time = 0;

        c.alphaKeys = alphaKeys;

        _LineRenderer.colorGradient = c;

        _VerifiedPath = false;

        _LineT = 0;

        _LineFormed = false;
    }

    // Default anchor points
    void SetStraightBends()
    {
        float val = _BendPoints.Count / 2;
        val = Mathf.Ceil(val);

        float yHeight = _MidPointHeight / val;

        for (int i = 0; i < _BendPoints.Count; i++)
        {
            float xPos = Mathf.Lerp(this.transform.position.x, _Creature.transform.position.x, i + 1/ _BendPoints.Count);

            float yPos;
            if (i > val)
                yPos = Random.Range(this.transform.position.y, _Creature.transform.position.y);
            else
            {
                yPos = this.transform.position.y + yHeight;

                yHeight += yHeight;
            }

            float zPos = Mathf.Lerp(this.transform.position.z, _Creature.transform.position.z, i + 1 / _BendPoints.Count);

            _BendPoints[i].position = new Vector3(xPos, yPos, zPos);
        }

        Gradient c = _LineRenderer.colorGradient;

        GradientAlphaKey[] alphaKeys = c.alphaKeys;

        alphaKeys[alphaKeys.Length - 1].time = 0;

        c.alphaKeys = alphaKeys;

        _LineRenderer.colorGradient = c;

        _VerifiedPath = false;

        _LineT = 0;

        _LineFormed = false;
    }

    // goes through a list of any size containing the start, end and
    // anchor positions and finds the point on the path at the specificied time
    Vector3 AnyraticLerp(List<Vector3> _posList, float t)
    {
        List<Vector3> List1 = new List<Vector3>();
        List<Vector3> List2 = new List<Vector3>();

        for (int i = 0; i < _posList.Count; i++)
            List1.Add(_posList[i]);

        List1.Add(_Creature.transform.position);

        while (List1.Count != 2)
        {
            for (int i = 0; i < List1.Count - 1; i++)
            {
                Vector3 Lerped = Vector3.Lerp(List1[i], List1[i + 1], t);

                List2.Add(Lerped);
            }

            List1.Clear();

            for (int i = 0; i < List2.Count; i++)
            {
                List1.Add(List2[i]);
            }

            List2.Clear();
        }

        return Vector3.Lerp(List1[0], List1[1], t);
    }
}
