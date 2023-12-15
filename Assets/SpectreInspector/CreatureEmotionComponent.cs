using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CreatureEmotions
{
    Angry,
    Dazed,
    Disappointed,
    Disgusted,
    Happy,
    Love,
    Neutral,
    Playful,
    Scared,
    Uncomfortable,
    Yum
}

[System.Serializable]
public struct CreatureEmotionsStruct
{
    public CreatureEmotions _Emotion;
    public Sprite _EmotionSprite;

    public void DisplayEmotion(CreatureBrain creature)
    {
        CreatureEmotionComponent CEC = creature.GetComponent<CreatureEmotionComponent>();

        CEC._EmotionPrefab.SetActive(true);
        //CEC._EmotionPrefab.GetComponent<ClearEmotionCanvas>()._Timer = 0;
        CEC._EmotionImage.sprite = _EmotionSprite;
        creature._CurEmotion = _Emotion;

        // set Creatures current Emotion as this Emotion

        // Display the Emotions Sprite
    }

}


public class CreatureEmotionComponent : MonoBehaviour
{
    public List<CreatureEmotionsStruct> _EmotionsList = new List<CreatureEmotionsStruct>();

    public GameObject _EmotionPrefab;
    public Image _EmotionImage;

    private void Update()
    {
        if (_EmotionPrefab.activeSelf == true)
        {
            _EmotionPrefab.transform.LookAt(Camera.main.transform);
        }

        /*if (Input.GetKeyDown(KeyCode.U))
        {
            int index = Random.Range(0, _EmotionsList.Count);

            _EmotionsList[index].DisplayEmotion(this.GetComponent<CreatureBrain>());

            /*for (int i = 0; i < _EmotionsList.Count; i++)
            {
                if (_EmotionsList[i]._Emotion == CreatureEmotions.Cheerful)
                    _EmotionsList[i].DisplayEmotion(this.GetComponent<CreatureBrain>());
            }
        }*/
    }
}
