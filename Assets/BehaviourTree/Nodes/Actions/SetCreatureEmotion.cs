using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCreatureEmotion : ActionNode
{
    CreatureEmotionComponent creatureEmotionComponent;
    ClearEmotionCanvas EmotionCanvas;

    public CreatureEmotions creatureEmotion;
    public float _TimeTillEmotionsDissapears = 2;

    bool _HasGottenEmotion = false;

    protected override void OnStart()
    {
        if (!_HasGottenEmotion)
        {
            creatureEmotionComponent = attachedGameObject.GetComponent<CreatureEmotionComponent>();
            EmotionCanvas = creatureEmotionComponent._EmotionPrefab.GetComponent<ClearEmotionCanvas>();
            _HasGottenEmotion = true;
        }
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {

        for (int i = 0; i < creatureEmotionComponent._EmotionsList.Count; i++)
        {
            if (creatureEmotionComponent._EmotionsList[i]._Emotion == creatureEmotion)
            {
                EmotionCanvas._TimeTillInactive = _TimeTillEmotionsDissapears;
                creatureEmotionComponent._EmotionsList[i].DisplayEmotion(attachedGameObject.GetComponent<CreatureBrain>());
                
            }
        }

        return State.Success;

    }

}
