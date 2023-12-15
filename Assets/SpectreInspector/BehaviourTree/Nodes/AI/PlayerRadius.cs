using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRadius : MonoBehaviour
{
    float t;

    public float _TimeTillChange = 1;
    public float _MaxDistAway = 5;
    public float _MinDistAway = 0;


    Transform _Player;
    PhysicsPlayerController _PlayerControl;
    Vector3 _OldPlayerPos;

    public LayerMask _WatchingPosRaycast;

    bool _HasMoved = false;


    // Start is called before the first frame update
    void Awake()
    {
        _Player = Camera.main.transform.root;
        _PlayerControl = _Player.GetComponent<PhysicsPlayerController>();
        this.transform.parent = _Player;
        this.transform.localPosition = Vector3.zero;

        _OldPlayerPos = _Player.position;

        ChangePos();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += _Player.position - _OldPlayerPos;
        this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);

        if (_HasMoved == true && _PlayerControl.moving == false)
        {
            _HasMoved = false;
            ChangePos();
        }
        else if (_PlayerControl.moving == true)
        {
            _HasMoved = true;
        }

        Debug.DrawLine(this.transform.position, _Player.position);

        _OldPlayerPos = _Player.position;
    }

    public void ChangePos()
    {
        bool suiSpot = false;
        int checks = 0;

        while (!suiSpot)
        {
            checks++;
            float newx = Random.Range(-_MaxDistAway, _MaxDistAway);
            float newz = Random.Range(-_MaxDistAway, _MaxDistAway);

            this.transform.parent = _Player;
            this.transform.localPosition = new Vector3(newx, 0, newz);


            float temp = Vector3.Distance(this.transform.position, this.transform.parent.position);

            while (temp > _MaxDistAway || temp < _MinDistAway)
            {
                newx = Random.Range(-_MaxDistAway, _MaxDistAway);
                newz = Random.Range(-_MaxDistAway, _MaxDistAway);


                this.transform.localPosition = new Vector3(newx, 0, newz);
                temp = Vector3.Distance(this.transform.position, this.transform.parent.position);
            }

            Debug.Log("Distance: " + Vector3.Distance(this.transform.position, this.transform.parent.position));
            this.transform.parent = null;

            this.transform.position = new Vector3(this.transform.position.x, 0.05f, this.transform.position.z);

            t = 0;

            RaycastHit hit;
            RaycastHit hit1;
            if (Physics.Raycast(_Player.position, this.transform.position - _Player.position, out hit, Vector3.Distance(this.transform.position, _Player.position), _WatchingPosRaycast))
            {
                
                suiSpot = false;
                Debug.Log("hitwallSphere " + hit.transform.name);
                    
               

            }
            else
            {
                suiSpot = true;
                Debug.Log("SPHERECASTSUCCESS");
            }





            if (checks == 100)
                suiSpot = true;
        }
    }
}
