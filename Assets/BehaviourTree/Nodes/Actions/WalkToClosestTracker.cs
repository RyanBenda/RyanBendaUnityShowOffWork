using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


[NodeCategory("[~] Holding Objects")]
public class WalkToClosestTracker : ActionNode
{
    NavMeshAgent agent;

    public Trackable tracker;
    public Trackable avoidTracker;

    //public bool prioriseTrackers = false;
    public float maxDistance = 5;

    public bool isPheromone = false;

    GameObject closestObject;
    //public float radius;


    protected override void OnStart()
    {
        if (avoidTracker == null)
        {
            avoidTracker = ScriptableObject.CreateInstance<Trackable>();
            avoidTracker.name = "DoesntUseAvoid";
        }



        agent = attachedGameObject.GetComponent<NavMeshAgent>();

        TrackerHolder[] Objs;
        Objs = GameObject.FindObjectsOfType<TrackerHolder>();

        if (Objs.Length != 0)
        {
            float closest = Vector3.Distance(Objs[0].transform.position, attachedGameObject.transform.position);

            foreach (TrackerHolder holder in Objs)
            {
                foreach (Trackable b in holder.trackers)
                {
                    if (b != null)
                    {
                        if (b.name == tracker.name)
                        {
                            if (closestObject == null &&  !isPheromone)
                            {
                                closestObject = holder.gameObject;

                            }

                            float a = Vector3.Distance(holder.transform.position, attachedGameObject.transform.position);


                            if (a <= maxDistance)
                            {
                                if (a <= closest && !holder.gameObject.transform.parent)
                                {
                                    if (!isPheromone)
                                    {
                                        //if (!prioriseTrackers || prioriseTrackers && closestObject == null)
                                        //{
                                        closest = a;

                                        closestObject = holder.transform.gameObject;
                                    }
                                    //else if (prioriseTrackers)
                                    //{
                                    //    if (b.priority >= closestObject..priority)
                                    //    {
                                    //        closest = a;
                                    //        closestObject = holder.transform.gameObject;
                                    else if (isPheromone)
                                    {
                                        if (closestObject.GetComponent<PheromoneLifeTime>().time > holder.GetComponent<PheromoneLifeTime>().time)
                                        {
                                            closest = a;
                                            closestObject = holder.transform.gameObject;
                                        }
                                    }
                                }
                            }

                           

                        }
                        else if (b.name == avoidTracker.name)
                        {
                            closestObject = null;
                            break;
                        }
                    }

                }


            }
        }

    }

    protected override void OnStop()
    {
        closestObject = null;
    }

    protected override State OnUpdate()
    {
        if (closestObject != null)
        {
            if (agent.enabled == true)
                agent.destination = closestObject.transform.position;
            return State.Success;
        }
        else
        {
            return State.Failure;
        }


    }
}
