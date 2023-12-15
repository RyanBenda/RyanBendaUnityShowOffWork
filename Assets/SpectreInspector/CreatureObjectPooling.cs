using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.AI;

public class CreatureObjectPooling : MonoBehaviour
{
    public List<GameObject> pooledCreatures;
    public GameObject creatureToPool;
    public int amountToPool;

    public List<Material> materials;

    // Below Bools are here for now, probably should be moved but don't want to make a new script
    public bool _CreatureTeleports = false;
    [HideInInspector]
    public bool _ChooseNewLocation = false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        pooledCreatures = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(creatureToPool);
            tmp.SetActive(false);
            CloneBrain cloneBrain = tmp.GetComponent<CloneBrain>();
            if (cloneBrain)
            {
                cloneBrain._hat.material = materials[i];
                cloneBrain._Pooler = this;
            }
            //ThrowObjectComponent TOC = tmp.GetComponent<ThrowObjectComponent>();
            //if (TOC)
            //{
            //    TOC._PlayerControl = GetComponent<CreatureBrain>()._Player.GetComponent<PlayerControl>();
            //}

            pooledCreatures.Add(tmp);

            
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledCreatures[i].activeInHierarchy)
            {
                return pooledCreatures[i];
            }
        }


        return null;
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("Dest: " + GetComponent<NavMeshAgent>().destination);
    }

    
}
