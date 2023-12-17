using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForceToChild : ActionNode
{
    //public Vector3 trajectory = new Vector3 (0,0.5f,1);
    public Trackable tracker;

    public bool randomiseDirection = false;
    public float randMin = -1.5f;
    public float randMax = 1.5f;


    public float force = 15f;
    public float upForce = 0;
    int childrenCount;
    protected override void OnStart()
    {
        childrenCount = attachedGameObject.transform.childCount;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {


        if (childrenCount != 0)
        {
            for (int i = 0; i < childrenCount; i++)
            {
                if (attachedGameObject.transform.GetChild(i).GetComponent<TrackerHolder>())
                {
                    TrackerHolder TH = attachedGameObject.transform.GetChild(i).GetComponent<TrackerHolder>();
                    foreach (Trackable a in TH.trackers)
                    {
                        if (a != null)
                        {
                            if (a == tracker)
                            {
                                GameObject gameObject = attachedGameObject.transform.GetChild(i).gameObject;
                                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                                if (randomiseDirection)
                                {

                                    float rando = Random.Range(randMin, randMax);


                                    gameObject.GetComponent<Rigidbody>().AddForce(attachedGameObject.transform.right * rando, ForceMode.Impulse);
                                    gameObject.GetComponent<Rigidbody>().AddForce(attachedGameObject.transform.forward * force, ForceMode.Impulse);
                                    gameObject.GetComponent<Rigidbody>().AddForce(attachedGameObject.transform.up * upForce, ForceMode.Impulse);
                                }
                                else
                                {
                                    gameObject.GetComponent<Rigidbody>().AddForce(attachedGameObject.transform.up * upForce, ForceMode.Impulse);
                                    gameObject.GetComponent<Rigidbody>().AddForce(attachedGameObject.transform.forward * force, ForceMode.Impulse);
                                }

                                gameObject.transform.parent = null;
                            }


                        }
                         

                        return State.Success;
                    }
                }
            }
        }

        return State.Failure;
    }
}
