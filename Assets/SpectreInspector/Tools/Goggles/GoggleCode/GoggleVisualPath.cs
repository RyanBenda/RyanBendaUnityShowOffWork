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

    BearRoom[] _BearRooms;

    // Start is called before the first frame update
    private void OnEnable()
    {
        if (_Agent == null)
            _Agent = GetComponent<NavMeshAgent>();

        if (_TrailRenderer == null)
            _TrailRenderer = GetComponentInChildren<TrailRenderer>();

        _TrailRenderer.Clear();

        /*if (_GhostToGoTo == Ghosts.Rags)
        {
            if (_BearRooms == null)
                _BearRooms = FindObjectsOfType<BearRoom>();

            for (int i = 0; i < _BearRooms.Length; i++)
            {
                if (_BearRooms[i]._RoomInUse)
                {
                    _Agent.destination = _BearRooms[i].transform.position;
                    return;
                }
            }
        }
        else if (_GhostToGoTo == Ghosts.Larry)
        {
            if (_InsuranceRooms == null)
                _InsuranceRooms = FindObjectsOfType<InsuranceRoom>();

            for (int i = 0; i < _InsuranceRooms.Length; i++)
            {
                if (_InsuranceRooms[i]._RoomInUse)
                {
                    _Agent.destination = _InsuranceRooms[i].transform.position;
                    return;
                }
            }
        }*/


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
