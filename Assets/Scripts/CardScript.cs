using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    // Card variable
    private int value;

    // Card Sprites
    [SerializeField] Sprite cardBack;
    [SerializeField] Sprite[] cardSprites;
    [SerializeField] SpriteRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Get the value of card
    public void GetValue(int num)
    {
        value = num;
    }

    // Open the card
    public void Open()
    {
        rend.sprite = cardSprites[value];
    }

    // Close the card
    public void Close()
    {
        rend.sprite = cardBack;
    }
}
