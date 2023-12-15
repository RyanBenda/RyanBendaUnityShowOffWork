using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForceToChildGolf : ActionNode
{
    //public Vector3 trajectory = new Vector3 (0,0.5f,1);
    public Trackable tracker;
    public Trackable targetTag;

    public bool randomiseDirection = false;
    public float randMin = -1.5f;
    public float randMax = 1.5f;

    public GameObject target;

    public float force = 15f;
    public float upforce = 15f;
    int childrenCount;
    protected override void OnStart()
    {
        childrenCount = attachedGameObject.transform.childCount;

        TrackerHolder[] Objs;
        Objs = GameObject.FindObjectsOfType<TrackerHolder>();

        if (Objs.Length != 0)
        {
            float closest = Vector3.Distance(Objs[0].transform.position, attachedGameObject.transform.position);

            foreach (TrackerHolder holder in Objs)
            {
                foreach (Trackable b in holder.trackers)
                {
                    if (b.name == targetTag.name)
                    {
                        float a = Vector3.Distance(holder.transform.position, attachedGameObject.transform.position);

                        if (a <= closest)
                        {
                            closest = a;
                            target = holder.transform.gameObject;
                        }
                    }
                }
            }
        }
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
                        if (a == tracker)
                        {
                            GameObject gameObject = attachedGameObject.transform.GetChild(i).gameObject;
                            gameObject.GetComponent<Rigidbody>().isKinematic = false;

                            float distance = Vector3.Distance(attachedGameObject.transform.position, target.transform.position);
                            float tempForce = force * distance / 3.5f;
                            float tempForce2 = upforce * distance / 35f;

                            if (randomiseDirection)
                            {
                                float randMinDupe = randMin * distance;
                                float randMaxDupe = randMax * distance;
                                float rando = Random.Range(randMinDupe, randMaxDupe);

                                Debug.Log(randMinDupe + " & " + randMaxDupe);




                                gameObject.GetComponent<Rigidbody>().AddForce(attachedGameObject.transform.right * rando, ForceMode.Impulse);
                                gameObject.GetComponent<Rigidbody>().AddForce(attachedGameObject.transform.forward * tempForce, ForceMode.Impulse);
                                gameObject.GetComponent<Rigidbody>().AddForce(attachedGameObject.transform.up * tempForce2, ForceMode.Impulse);
                            }
                            else
                            {
                                gameObject.GetComponent<Rigidbody>().AddForce(attachedGameObject.transform.forward * tempForce, ForceMode.Impulse);
                                gameObject.GetComponent<Rigidbody>().AddForce(attachedGameObject.transform.up * tempForce2, ForceMode.Impulse);
                            }

                            gameObject.transform.parent = null;
                        }

                        return State.Success;
                    }
                }
            }
        }

        return State.Failure;
    }
}
