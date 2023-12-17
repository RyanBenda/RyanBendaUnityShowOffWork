using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripShotCollider : MonoBehaviour
{
    TripShotTool _Tripshot;
    // Start is called before the first frame update
    void Start()
    {
        _Tripshot = FindObjectOfType<TripShotTool>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGERED");

        if (other.gameObject.GetComponent<CreatureBrain>() != null)
        {
           _Tripshot._Plunger.GetComponent<PlungerComponent>().SpawnElectrocute(other.ClosestPoint(this.transform.position));

            other.gameObject.GetComponent<CreatureBrain>().isDizzy = true;
            _Tripshot.PlungerDestroy();
        }
        else if(other.gameObject.GetComponent<PhysicsPlayerController>() != null || other.gameObject.tag == "Player")
        {
            _Tripshot._Plunger.GetComponent<PlungerComponent>().SpawnElectrocute(other.ClosestPoint(this.transform.position));

            _Tripshot.PlungerDestroy();
        }
    }
}
