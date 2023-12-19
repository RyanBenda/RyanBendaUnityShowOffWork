using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Ghosts
{
    Peebo,
    Rags,
    Larry,
    Knight
}

[RequireComponent(typeof(NavMeshAgent))]
public class GoggleVisualPath : MonoBehaviour
{
    public Ghosts _GhostToGoTo;
    public NavMeshAgent _Agent;
    public TrailRenderer _TrailRenderer;

    // Start is called before the first frame update
    private void OnEnable()
    {
        if (_Agent == null)
            _Agent = GetComponent<NavMeshAgent>();

        if (_TrailRenderer == null)
            _TrailRenderer = GetComponentInChildren<TrailRenderer>();

        _TrailRenderer.Clear();
    }
}
