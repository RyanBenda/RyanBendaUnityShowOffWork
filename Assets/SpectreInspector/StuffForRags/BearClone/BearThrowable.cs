using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearThrowable : MonoBehaviour
{
    Rigidbody rb;
    public float _MinCollSpeed = 2.5f;

    bool _HasHitPlayer = false;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PhysicsPlayerController>())
        {
            if (rb.velocity.magnitude > _MinCollSpeed && !_HasHitPlayer)
            {
                PlayerStats.instance.ReduceHealth();
                _HasHitPlayer = true;
            }
        }
    }
}
