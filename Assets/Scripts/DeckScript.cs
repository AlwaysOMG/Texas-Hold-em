using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    // Variable
    private System.Random randomCard = new System.Random();
    
    // Parameter
    private const int numDeck = 52;
    private const int numCardsOnTable = 9;
    
    // Cards On Table
    public Hashtable valueOfCards = new Hashtable();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Return the hashtable
    public Hashtable Deal()
    {
        // Pick 9 cards for the game
        valueOfCards.Clear();
        int keyCounter = 0;
        while (valueOfCards.Count < numCardsOnTable)
        {
            int value = randomCard.Next(0, numDeck);
            if (!valueOfCards.ContainsValue(value))
            {
                valueOfCards.Add(keyCounter, value);
                keyCounter++;
            }
        }

        // Assign Cards with specific value
        /*
        valueOfCards.Add(0, 2);
        valueOfCards.Add(1, 4);
        valueOfCards.Add(2, 3);
        valueOfCards.Add(3, 5);
        valueOfCards.Add(4, 14);
        valueOfCards.Add(5, 7);
        valueOfCards.Add(6, 21);
        valueOfCards.Add(7, 23);
        valueOfCards.Add(8, 24);
        */
        return valueOfCards;
    }
}
