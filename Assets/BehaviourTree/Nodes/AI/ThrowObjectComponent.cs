using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObjectComponent : MonoBehaviour
{
    public GameObject[] _ThrowableItems;
    public bool _AttackPlayer;
    //[HideInInspector]
    //public bool _IgnoreAttack;
    //[HideInInspector]
    //public PlayerControl _PlayerControl;

    public float _ForceMultiplier = 2;
    public Transform _ThrowableStartPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_AttackPlayer /*&& !_IgnoreAttack*/)
        {
            int r = Random.Range(0, _ThrowableItems.Length);
            GameObject throwable = Instantiate(_ThrowableItems[r], this.transform);
            throwable.transform.parent = null;
            throwable.transform.position = _ThrowableStartPos.position; //transform.position + transform.forward / 2;

            //throwable.transform.position = new Vector3(throwable.transform.position.x - 0.25f, throwable.transform.position.y, throwable.transform.position.z);


            Vector3 throwaim = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 1, Camera.main.transform.position.z);

            Vector3 dir = (throwaim - throwable.transform.position);
            throwable.GetComponent<Rigidbody>().AddForce(dir * _ForceMultiplier);
            Destroy(throwable, 3);

            _AttackPlayer = false;
        }
        /*else if (_IgnoreAttack && _AttackPlayer)
        {
            _AttackPlayer = false;
            _IgnoreAttack = false;
        }*/
    }
}
