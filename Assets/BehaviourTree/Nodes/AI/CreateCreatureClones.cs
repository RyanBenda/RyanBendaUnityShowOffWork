using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreateCreatureClones : ActionNode
{
    CreatureObjectPooling _Pool;
    CreatureBrain _Creature;


    protected override void OnStart()
    {
        if (_Pool == null)
            _Pool = attachedGameObject.GetComponent<CreatureObjectPooling>();

        if (_Creature == null)
            _Creature = attachedGameObject.GetComponent<CreatureBrain>();

        if (_Pool != null && !_Creature._HasActiveClones)
        {
            for (int i = 0; i < _Pool.pooledCreatures.Count; i++)
            {
                _Pool.pooledCreatures[i].SetActive(true);
                _Pool.pooledCreatures[i].transform.position = attachedGameObject.transform.position;
                _Pool.pooledCreatures[i].transform.rotation = attachedGameObject.transform.rotation;
                _Pool.pooledCreatures[i].transform.localScale = attachedGameObject.transform.localScale;
                _Pool.pooledCreatures[i].gameObject.GetComponent<CloneBrain>()._Agent.enabled = true;
                _Creature._HasActiveClones = true;
            }
        }
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }


}
