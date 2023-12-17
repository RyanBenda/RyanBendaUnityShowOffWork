using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct TrackStruct
{
    public Vector3 pos;
    public Vector3 scale;
}


public class TracksComponent : MonoBehaviour
{
    List<TrackStruct> _Tracks = new List<TrackStruct>();

    public float _TimeTillFullyGrown = 1;
    float t;

    bool _RemoveTrack;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            TrackStruct transform = default;

            transform.pos = this.transform.GetChild(i).localPosition;
            transform.scale = this.transform.GetChild(i).localScale;

            _Tracks.Add(transform);


            this.transform.GetChild(i).localPosition = Vector3.zero;
            this.transform.GetChild(i).localScale = new Vector3(0, 1, 0);
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).localPosition = Vector3.zero;
            this.transform.GetChild(i).localScale = new Vector3(0, 1, 0);
        }

        t = 0;
        _RemoveTrack = false;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime / _TimeTillFullyGrown;

        if (t <= 1 && !_RemoveTrack)
        {
            for (int i = 0; i < _Tracks.Count; i++)
            {
                this.transform.GetChild(i).localPosition = Vector3.Lerp(this.transform.GetChild(i).localPosition, _Tracks[i].pos, t);
                this.transform.GetChild(i).localScale = Vector3.Lerp(this.transform.GetChild(i).localScale, _Tracks[i].scale, t);
            }
        }
        else if (t <= 1 && _RemoveTrack)
        {
            for (int i = 0; i < _Tracks.Count; i++)
            {
                this.transform.GetChild(i).localPosition = Vector3.Lerp(this.transform.GetChild(i).localPosition, Vector3.zero, t);
                this.transform.GetChild(i).localScale = Vector3.Lerp(this.transform.GetChild(i).localScale, new Vector3(0, 1, 0), t);
            }

        }
        else if (t > 1 && _RemoveTrack)
        {
            // REMOVE FROM GOGGLE LIST

            this.gameObject.SetActive(false);
        }
    }

    public void RemoveTrack()
    {
        _RemoveTrack = true;
        t = 0;
    }
}
