using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrackingComponent : MonoBehaviour
{
    //public GameObject _TrackingPrefab;

    public float _TimeBetweenSpawns = 5;
    float t = 0;

    public LayerMask _LayerMask;

    List<GameObject> _Tracks = new List<GameObject>();
    public float _TracksCount = 10;

    public bool _GoggleOnly;

    public bool Footprints = true;
    public bool Pages = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;

        if (t >= _TimeBetweenSpawns)
        {
            RaycastHit hit;

            if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 1000, _LayerMask))
            {
                //Debug.Log(hit.collider.name);

                while (_Tracks.Count >= _TracksCount)
                {
                    _Tracks[0].GetComponent<TracksComponent>().RemoveTrack();
                    _Tracks.RemoveAt(0);
                }

                GameObject newTrack = null;

                //if (Footprints)
                    //newTrack = FootprintObjectPools.FootprintInstance.GetPooledObject();
                //else if (Pages)
                    //newTrack = PagesObjectPool.PagesInstance.GetPooledObject();

                if (newTrack != null)
                {
                    newTrack.transform.position = hit.point;
                    newTrack.transform.forward = this.transform.forward;
                    newTrack.SetActive(true);

                    if (_GoggleOnly && Camera.main.transform.GetComponentInChildren<GoggleTool>(true).gameObject.activeSelf == false)
                    {
                        // send track to Goggles
                        newTrack.GetComponentInChildren<SpriteRenderer>().enabled = false;
                        //Camera.main.transform.GetComponentInChildren<GoggleTool>()._FootPrints.Add(newTrack.GetComponentInChildren<SpriteRenderer>());
                    }

                    _Tracks.Add(newTrack);
                }
                
            }

            t = 0;
        }
    }
}
