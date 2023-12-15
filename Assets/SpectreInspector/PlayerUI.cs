using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image Player;
    [SerializeField] private Image DrinkMeter;
    [SerializeField] private Image Hearts;

    [SerializeField] private Sprite[] PlayerSprites;
    [SerializeField] private Sprite[] DrinkSprites;
    [SerializeField] private Sprite[] HeartsSprites;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdatePlayer(int sprite)
    {
        Player.sprite = PlayerSprites[sprite];
    }
    public void UpdateDrink(int sprite)
    {
        if (sprite < DrinkSprites.Length)
            DrinkMeter.sprite = DrinkSprites[sprite];
    }
    public void UpdateHearts(int sprite)
    {
        Hearts.sprite = HeartsSprites[sprite];
    }
}
