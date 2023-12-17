using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeCategory("[~] Transform")]
public class WaitNRotate : ActionNode
{
    public float duration = 1;
    float startTime;

    public Trackable Tracker;

    public GameObject pos;

    public float speed = 1;

    TrackerHolder[] Objs;

    public bool _DurationMatchesRotation = false;
    public float _RotationLenience = 2;
    //CreatureBrain _brain;

    protected override void OnStart()
    {
        startTime = Time.time;

        if (Objs == null)
        {
            Objs = GameObject.FindObjectsOfType<TrackerHolder>();
        }


        if (Objs.Length != 0)
        {
            float closest = Vector3.Distance(Objs[0].transform.position, attachedGameObject.transform.position);

            foreach (TrackerHolder holder in Objs)
            {
                foreach (Trackable b in holder.trackers)
                {
                    if (b != null)
                    {
                        if (b.name == Tracker.name)
                        {
                            if (pos == null)
                            {
                                pos = holder.gameObject;
                            }

                            float a = Vector3.Distance(holder.transform.position, attachedGameObject.transform.position);

                            if (holder.GetComponent<Rigidbody>())
                            {
                                if (a <= closest && holder.GetComponent<Rigidbody>().velocity.magnitude <= 1.5)
                                {
                                    closest = a;
                                    pos = holder.transform.gameObject;
                                }
                            }
                            else if (!holder.GetComponent<Rigidbody>())
                            {
                                if (a <= closest)
                                {
                                    closest = a;
                                    pos = holder.transform.gameObject;
                                }
                            }

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
        Vector3 dir = pos.transform.position - attachedGameObject.transform.position;
        dir.y = 0; // keep the direction strictly horizontal
        Quaternion rot = Quaternion.LookRotation(dir);

        if (!_DurationMatchesRotation)
        {
            if (Time.time - startTime > duration)
            {

                return State.Success;
            }
        }
        else
        {
            if (Quaternion.Angle(attachedGameObject.transform.rotation, rot) < _RotationLenience)
                return State.Success;
        }

        
        // slerp to the desired rotation over time
        attachedGameObject.transform.rotation = Quaternion.Slerp(attachedGameObject.transform.rotation, rot, speed * Time.deltaTime);

     

        ///if (Quaternion.Angle(attachedGameObject.transform.rotation, rot) < 0)
            //Debug.Log("Angle AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");

        return State.Running;
    }

   
}
