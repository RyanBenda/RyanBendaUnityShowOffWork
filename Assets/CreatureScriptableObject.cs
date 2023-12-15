using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Creature/CreatureScriptableObject", order = 1)]
public class CreatureScriptableObject : ScriptableObject
{
    public string _Name;
    public string _Description;
    public Sprite _Sprite;
    public RenderTexture _Image;
    public GameObject _CreaturePrefab;
    public Sprite _NamePlate;
    public Sprite _CompendiumPlate;
}
