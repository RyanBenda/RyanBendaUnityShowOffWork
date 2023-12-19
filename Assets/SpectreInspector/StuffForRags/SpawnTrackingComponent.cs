using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrackingComponent : MonoBehaviour
{
    [SerializeField] private float _TimeBetweenSpawns = 5;
    float t = 0;

    [SerializeField] private LayerMask _LayerMask;

    List<GameObject> _Tracks = new List<GameObject>();
    [SerializeField] private float _TracksCount = 10;

    public bool _GoggleOnly;

    void Update()
    {
        t += Time.deltaTime;

        if (t >= _TimeBetweenSpawns)
        {
            RaycastHit hit;

            if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 1000, _LayerMask))
            {
                while (_Tracks.Count >= _TracksCount)
                {
                    _Tracks[0].GetComponent<TracksComponent>().RemoveTrack();
                    _Tracks.RemoveAt(0);
                }

                GameObject newTrack = FootprintObjectPools.FootprintInstance.GetPooledObject();

                if (newTrack != null)
                {
                    newTrack.transform.position = hit.point;
                    newTrack.transform.forward = this.transform.forward;
                    newTrack.SetActive(true);

                    if (_GoggleOnly && ToolManager.instance._GoggleTool.gameObject.activeSelf == false)
                    {
                        newTrack.GetComponentInChildren<SpriteRenderer>().enabled = false;
                    }

                    _Tracks.Add(newTrack);
                }
            }

            t = 0;
        }
    }

    public void RemoveAllTracks()
    {
        foreach (GameObject track in _Tracks)
        {
            track.GetComponent<TracksComponent>().RemoveTrack();
        }
    }
}
