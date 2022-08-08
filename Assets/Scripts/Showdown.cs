using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Showdown : MonoBehaviour
{
    // Parameter
    private const int numCardsOnTable = 9;

    // Data Structure
    private int[] playerHand = new int[7];
    private int[] dealerHand = new int[7]; 
    private Hashtable dictPlayerHand = new Hashtable();
    private Hashtable dictDealerHand = new Hashtable();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Initialize dictionary
    void DictInitialize()
    {
        dictPlayerHand.Clear();
        dictDealerHand.Clear();
        for (int i = 1; i < 15; i++)
        {
            dictPlayerHand.Add(i, 0);
            dictDealerHand.Add(i, 0);
        }
        dictPlayerHand.Add("C", 0);
        dictPlayerHand.Add("D", 0);
        dictPlayerHand.Add("H", 0);
        dictPlayerHand.Add("S", 0);
        dictDealerHand.Add("C", 0);
        dictDealerHand.Add("D", 0);
        dictDealerHand.Add("H", 0);
        dictDealerHand.Add("S", 0);
    }

    // Make dictionary of poker hands
    void DictHand(Hashtable valueOfCards)
    {
        // Collect poker hands
        for (int i = 0; i < numCardsOnTable; i++)
        {
            if (i == 0 || i == 1)
            {
                playerHand[i] = (int) valueOfCards[i];
                continue;
            }
            if (i == 2 || i == 3)
            {
                dealerHand[i-2] = (int) valueOfCards[i];
                continue;
            }
            playerHand[i-2] = (int) valueOfCards[i];
            dealerHand[i-2] = (int) valueOfCards[i];
        }

        // Make Dictionary
        DictInitialize();       
        for (int i = 0; i < 7; i++)
        {
            int pValue = (playerHand[i] % 13) + 1;
            int pSuit = playerHand[i] / 13;
            int dValue = (dealerHand[i] % 13) + 1; 
            int dSuit = dealerHand[i] / 13;
            // Value
            dictPlayerHand[pValue] = (int) dictPlayerHand[pValue] + 1;
            dictDealerHand[dValue] = (int) dictDealerHand[dValue] + 1;
            // Suit
            switch (pSuit)
            {
                case 0:
                    dictPlayerHand["C"] = (int) dictPlayerHand["C"] + 1;
                    break;
                case 1:
                    dictPlayerHand["D"] = (int) dictPlayerHand["D"] + 1;
                    break;
                case 2:
                    dictPlayerHand["H"] = (int) dictPlayerHand["H"] + 1;
                    break;
                case 3:
                    dictPlayerHand["S"] = (int) dictPlayerHand["S"] + 1;
                    break;
            }
            switch (dSuit)
            {
                case 0:
                    dictDealerHand["C"] = (int) dictDealerHand["C"] + 1;
                    break;
                case 1:
                    dictDealerHand["D"] = (int) dictDealerHand["D"] + 1;
                    break;
                case 2:
                    dictDealerHand["H"] = (int) dictDealerHand["H"] + 1;
                    break;
                case 3:
                    dictDealerHand["S"] = (int) dictDealerHand["S"] + 1;
                    break;
            }
        }
        // One equals to ace
        dictPlayerHand[14] = dictPlayerHand[1];
        dictDealerHand[14] = dictDealerHand[1];
    }

    // Check flush or not
    bool isFlush(Hashtable DictHand)
    {
        if ((int) DictHand["C"] > 4)  return true;
        if ((int) DictHand["D"] > 4)  return true;
        if ((int) DictHand["H"] > 4)  return true;
        if ((int) DictHand["S"] > 4)  return true;
        return false;
    }

    // Return straight high card
    int StraightHighCard(Hashtable DictHand)
    {
        for (int i = 14; i > 4; i--)
        {
            for (int j = 0; j < 5; j++)
            {
                if ((int) DictHand[i-j] < 1)  break;
                if (j == 4)  return i;
            }
        }
        return 0;
    }

    // Return the card with specific frequency
    int CardCount(Hashtable DictHand, int frequency, int but = 0)
    {
        for (int i = 14;  i > 1; i--)
        {
            if (i == but)  continue;
            if ((int) DictHand[i] == frequency)  return i;
        }
        return 0;
    }

    // Return the rank of poker hands
    int HandRank(Hashtable DictHand, string playername)
    {
        // Straight flush
        if (isFlush(DictHand) && StraightHighCard(DictHand) > 0)
        {
            Debug.Log(playername + ": Straight Flush");
            return 8;
        }
        // Four of a kind
        if (CardCount(DictHand, 4) > 0)
        {
            Debug.Log(playername + ": Four of a kind");
            return 7;
        }
        // Full House
        if (CardCount(DictHand, 3) > 0 && CardCount(DictHand, 2, CardCount(DictHand, 3)) > 0)
        {
            Debug.Log(playername + ": Full House");
            return 6;
        }
        // Flush
        if (isFlush(DictHand))
        {
            Debug.Log(playername + ": Flush");
            return 5;
        }
        // Straight
        if (StraightHighCard(DictHand) > 0)
        {
            Debug.Log(playername + ": Straight");
            return 4;
        }
        // Three of a kind
        if (CardCount(DictHand, 3) > 0)
        {
            Debug.Log(playername + ": Three of a kind");
            return 3;
        }
        // Two Pairs
        if (CardCount(DictHand, 2) > 0 && CardCount(DictHand, 2, CardCount(DictHand, 2)) > 0)
        {
            Debug.Log(playername + ": Two Pairs");
            return 2;
        }
        // Pair
        if (CardCount(DictHand, 2) > 0)
        {
            Debug.Log(playername + ": Pair");
            return 1;
        }
        // High card
        Debug.Log(playername + ": High Card");
        return 0;
    }

    // Compare one by one
    int Single(Hashtable dictPlayerHand, Hashtable dictDealerHand, 
                int times = 1, int highCard = 0, int lowCard = 0)
    {
        for (int i = 14; i > 1 && times > 0; i--)
        {
            if (i == highCard || i == lowCard)  continue;
            if ((int) dictPlayerHand[i] > (int) dictDealerHand[i])  return 1;
            else if ((int) dictPlayerHand[i] < (int) dictDealerHand[i])  return 0;
            else if ((int) dictPlayerHand[i] == 1)  times--;
        }
        return -1;
    }

    // Compare if rank tie
    int Compare(Hashtable dictPlayerHand, Hashtable dictDealerHand, int rank)
    {
        // Declare variables
        int pHighCard;
        int dHighCard;
        int pLowCard;
        int dLowCard;
        
        // Compare according to the rank
        switch (rank)
        {
            // Straight flush
            case 8:
                pHighCard = StraightHighCard(dictPlayerHand);
                dHighCard = StraightHighCard(dictDealerHand);
                if (pHighCard > dHighCard)  return 1;
                else if (pHighCard < dHighCard)  return 0;
                else  return -1;
            // Four of a kind
            case 7:
                pHighCard = CardCount(dictPlayerHand, 4);
                dHighCard = CardCount(dictDealerHand, 4);
                if (pHighCard > dHighCard)  return 1;
                else if (pHighCard < dHighCard)  return 0;
                break;
            // Full house
            case 6:
                pHighCard = CardCount(dictPlayerHand, 3);
                dHighCard = CardCount(dictDealerHand, 3);
                pLowCard = CardCount(dictPlayerHand, 2, CardCount(dictPlayerHand, 3));
                dLowCard = CardCount(dictDealerHand, 2, CardCount(dictPlayerHand, 3));
                if (pHighCard > dHighCard)  return 1;
                else if (pHighCard < dHighCard)  return 0;
                else
                {
                    if (pLowCard > dLowCard)  return 1;
                    else if (pLowCard < dLowCard)  return 0;
                    else  return -1;
                }
            // Flush
            case 5:
                // Variable
                int suitFlush = -1;
                int[] playerFlushHand = new int[7];
                int[] dealerFlushHand = new int[7];
                // Find the suit of flush
                if ((int) dictPlayerHand["C"] > 4)  suitFlush = 0;
                if ((int) dictPlayerHand["D"] > 4)  suitFlush = 1;
                if ((int) dictPlayerHand["H"] > 4)  suitFlush = 2;
                if ((int) dictPlayerHand["S"] > 4)  suitFlush = 3;
                // Find the card in the suit of flush
                for (int i = 0; i < 7; i++)
                {
                    if (playerHand[i] / 13 == suitFlush)
                    {
                        if (playerHand[i] % 13 == 0)  playerFlushHand[i] = 14;
                        else  playerFlushHand[i] = (playerHand[i] % 13) + 1;
                    }
                    if (dealerHand[i] / 13 == suitFlush)
                    {
                        if (dealerHand[i] % 13 == 0)  dealerFlushHand[i] = 14;
                        else  dealerFlushHand[i] = (dealerHand[i] % 13) + 1;
                    }
                }
                Array.Sort(playerFlushHand);
                Array.Sort(dealerFlushHand);
                // Single of flush
                for (int i = 6; i > 1; i--)
                {
                    if (playerFlushHand[i] > dealerFlushHand[i])  return 1;
                    else if (playerFlushHand[i] < dealerFlushHand[i])  return 0;
                }
                return -1;
            // Straight
            case 4:
                pHighCard = StraightHighCard(dictPlayerHand);
                dHighCard = StraightHighCard(dictDealerHand);
                if (pHighCard > dHighCard)  return 1;
                else if (pHighCard < dHighCard)  return 0;
                else  return -1;
            // Three of a kind
            case 3:
                pHighCard = CardCount(dictPlayerHand, 3);
                dHighCard = CardCount(dictDealerHand, 3);
                if (pHighCard > dHighCard)  return 1;
                else if (pHighCard < dHighCard)  return 0;
                else  return Single(dictPlayerHand, dictDealerHand, 2, pHighCard);
            // Two pairs
            case 2:
                pHighCard = CardCount(dictPlayerHand, 2);
                dHighCard = CardCount(dictDealerHand, 2);
                pLowCard = CardCount(dictPlayerHand, 2, CardCount(dictPlayerHand, 2));
                dLowCard = CardCount(dictDealerHand, 2, CardCount(dictPlayerHand, 2));
                if (pHighCard > dHighCard)  return 1;
                else if (pHighCard < dHighCard)  return 0;
                else
                {
                    if (pLowCard > dLowCard)  return 1;
                    else if (pLowCard < dLowCard)  return 0;
                    else  return Single(dictPlayerHand, dictDealerHand, 1, pHighCard, pLowCard);
                }
            // Pair
            case 1:
                pHighCard = CardCount(dictPlayerHand, 2);
                dHighCard = CardCount(dictDealerHand, 2);
                if (pHighCard > dHighCard)  return 1;
                else if (pHighCard < dHighCard)  return 0;
                else  return Single(dictPlayerHand, dictDealerHand, 3, pHighCard);
            // High card
            case 0:
                return Single(dictPlayerHand, dictDealerHand, 5);
        }
        return -1;
    }

    // Showdown
    public int funcShowdown(Hashtable valueOfCards)
    {
        // tie: -1, dealer win: 0, player win: 1
        int gameWinner = -1;
        DictHand(valueOfCards);
        int playerRank = HandRank(dictPlayerHand, "Player");
        int dealerRank = HandRank(dictDealerHand, "Dealer");
        if (playerRank > dealerRank)  gameWinner = 1;
        else if (playerRank < dealerRank)  gameWinner = 0;
        else  gameWinner = Compare(dictPlayerHand, dictDealerHand, playerRank);
        return gameWinner;
    }
}
