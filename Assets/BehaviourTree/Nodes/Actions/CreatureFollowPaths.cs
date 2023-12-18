using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[NodeCategory("[~] Snax-Like")]

public class CreatureFollowPaths : ActionNode
{
    public PatrolZone PZ;
    public GameObject[] patrolPoints;
    List<PatrolZone> patrolZonesInScene = new List<PatrolZone>();

    int currentPatrolPoint = 0;
    NavMeshAgent agent;
    public float howCloseTo = 1.5f;



    public bool exclusivePathsEnabled = false; //keeping this off by default, so it wont break characters who havent been tweaked yet - Lee

    protected override void OnStart()
    {


        

        if (patrolZonesInScene.Count == 0)
        {
            var foundCanvasObjects = FindObjectsOfType<PatrolZone>();



            foreach (PatrolZone zone in foundCanvasObjects)
            { 

                
                if(exclusivePathsEnabled)
                {
                    CreatureScriptableObject creatureScriptable = attachedGameObject.GetComponent<CreatureBrain>()._CreatureIdentity;
                    for (int i = 0; i < zone.whoCanUsePath.Length; i++)
                    {
                        if (zone.whoCanUsePath[i] == creatureScriptable)
                        {
                            patrolZonesInScene.Add(zone);
                        }
                    }
                }
                else
                    patrolZonesInScene.Add(zone);
            }

        }


        PatrolZone closestPZ = patrolZonesInScene[0];

        foreach (PatrolZone zone in patrolZonesInScene)
        {
            if (Vector3.Distance(zone.transform.position, attachedGameObject.transform.position) < Vector3.Distance(closestPZ.transform.position, attachedGameObject.transform.position))
            {
                closestPZ = zone;
            }
        }

        PZ = closestPZ;

        patrolPoints = PZ.pathPoints;

        agent = attachedGameObject.GetComponent<NavMeshAgent>();
        agent.destination = patrolPoints[0].transform.position;


        if(currentPatrolPoint > patrolPoints.Length - 1)
        {
            currentPatrolPoint = Random.Range(0, patrolPoints.Length - 1);
        }
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {



        agent.destination = patrolPoints[currentPatrolPoint].transform.position;

        float dist = Vector3.Distance(attachedGameObject.transform.position, patrolPoints[currentPatrolPoint].transform.position);
        if (dist <= howCloseTo && currentPatrolPoint < patrolPoints.Length && currentPatrolPoint + 1 != patrolPoints.Length)
        {

            currentPatrolPoint++;
            agent.destination = patrolPoints[currentPatrolPoint].transform.position;
        }
        else if (dist <= howCloseTo && currentPatrolPoint == patrolPoints.Length - 1)
        {
            currentPatrolPoint = 0;
            return State.Success;
        }

        return State.Success;

    }
}
