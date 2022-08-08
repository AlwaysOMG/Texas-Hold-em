using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFlow : MonoBehaviour
{
    // Variable
    private bool actionOver = false;
    private int gameWinner = -1;

    // Parameter
    private const int numCardsOnTable = 9;
    private Vector3[] chipsPotSlot = new Vector3[10];

    // Data structure
    private Hashtable valueOfCards = new Hashtable();
    private Stack potChips = new Stack();
    
    // Declare the objects
    [SerializeField] GameObject player;
    [SerializeField] GameObject dealer;
    [SerializeField] GameObject deck;
    [SerializeField] GameObject[] cards;
    [SerializeField] Button startButton;
    [SerializeField] Button betButton;
    [SerializeField] Button passButton;
    [SerializeField] Button foldButton;

    // Declare and initialize the scripts
    DeckScript deckScript;
    CardScript[] cardScripts = new CardScript[numCardsOnTable];
    PlayerScript playerScript;
    DealerScript dealerScript;
    Showdown showdown;

    // Start is called before the first frame update
    void Start()
    {
        // Add onclick listeners to the buttons
        startButton.onClick.AddListener(() => StartClicked());
        betButton.onClick.AddListener(() => BetClicked());
        passButton.onClick.AddListener(() => PassClicked());
        foldButton.onClick.AddListener(() => FoldClicked());
        betButton.gameObject.SetActive(false);
        passButton.gameObject.SetActive(false);
        foldButton.gameObject.SetActive(false);

        // Load the Scripts
        deckScript = deck.GetComponent<DeckScript>();
        for (int i = 0; i < numCardsOnTable; i++)  
            cardScripts[i] = cards[i].GetComponent<CardScript>();
        playerScript = player.GetComponent<PlayerScript>();
        dealerScript = dealer.GetComponent<DealerScript>();
        showdown = GetComponent<Showdown>();

        // Initialize the chipsPotSlot
        for (int i = 0; i < 5; i++)
        {
            chipsPotSlot[i] = new Vector3(-8+i, 0.3f, 0);
            chipsPotSlot[i+5] = new Vector3(-8+i, -0.3f, 0);
        }
        
        // Initialize the chips
        GameObject[] blackChips = GameObject.FindGameObjectsWithTag("pChips");
        GameObject[] blueChips = GameObject.FindGameObjectsWithTag("dChips");
        for (int i = 0; i < 5; i++)
        {
            playerScript.putChip(blackChips[i]);
            dealerScript.putChip(blueChips[i]);
            playerScript.playerChips.Push(blackChips[i]);
            dealerScript.dealerChips.Push(blueChips[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Position a chip to pot
    public void putChip(GameObject chip)
    {
        chip.transform.position = chipsPotSlot[potChips.Count];
    }

    // Once startButton Clicked
    void StartClicked()
    {
        if (playerScript.playerChips.Count == 0 || dealerScript.dealerChips.Count == 0)
            Application.Quit();
        else
        {
            StartCoroutine(Play());
            startButton.gameObject.SetActive(false);
        }
    }

    // Once betButton Clicked
    void BetClicked()
    {
        // Deactivate other buttons
        passButton.gameObject.SetActive(false);
        foldButton.gameObject.SetActive(false);

        // Player sends a chip to the pot and dealer calls
        GameObject chip1 = (GameObject) playerScript.playerChips.Pop();
        putChip(chip1);
        potChips.Push(chip1);
        Debug.Log("Player bets");
        GameObject chip2 = (GameObject) dealerScript.dealerChips.Pop();
        putChip(chip2);
        potChips.Push(chip2);
        Debug.Log("Dealer bets");
        actionOver = true;
    }

    // Once passButton Clicked
    void PassClicked()
    {
        // Deactivate other buttons
        betButton.gameObject.SetActive(false);
        foldButton.gameObject.SetActive(false);
        
        // Pass
        Debug.Log("Player passes");
        Debug.Log("Dealer passes");
        actionOver = true;
    }

    // Once FoldButton Clicked
    void FoldClicked()
    {
        // Deactivate other buttons
        betButton.gameObject.SetActive(false);
        passButton.gameObject.SetActive(false);

        // Dealer takes the pot
        while(potChips.Count > 0)
        {
            GameObject chip = (GameObject) potChips.Pop();
            dealerScript.putChip(chip);
            dealerScript.dealerChips.Push(chip);
        }
        Debug.Log("Player folds");
        Debug.Log("Dealer wins");
        
        // Game over
        StopCoroutine(Play());
        startButton.gameObject.SetActive(true);
    }

    // Action between the player and the dealer
    void Round()
    {
        // If one of them have no chips, do nothing
        // Otherwise, activate the button
        if (playerScript.playerChips.Count == 0 || dealerScript.dealerChips.Count == 0)
            actionOver = true;
        else
        {
        betButton.gameObject.SetActive(true);
        passButton.gameObject.SetActive(true);
        foldButton.gameObject.SetActive(true);
        }
    }

    // Check for winner and loser
    void RoundOver()
    {
        // Deactivate the button
        betButton.gameObject.SetActive(false);
        passButton.gameObject.SetActive(false);
        foldButton.gameObject.SetActive(false);
        
        // Showdown
        cardScripts[2].Open();
        cardScripts[3].Open();
        gameWinner = showdown.funcShowdown(valueOfCards);

        // Transfer the chips according to the result
        if (gameWinner == 1)
        {
            // Player takes the pot
            while(potChips.Count > 0)
            {
                GameObject chip = (GameObject) potChips.Pop();
                playerScript.putChip(chip);
                playerScript.playerChips.Push(chip);
            }
            Debug.Log("Player wins");
        }
        if (gameWinner == 0)
        {
            // Dealer takes the pot
            while(potChips.Count > 0)
            {
                GameObject chip = (GameObject) potChips.Pop();
                dealerScript.putChip(chip);
                dealerScript.dealerChips.Push(chip);
            }
            Debug.Log("Dealer wins");
        }
        if (gameWinner == -1)
        {
            // Take their chips back
            while(potChips.Count > 0)
            {
                GameObject chip1 = (GameObject) potChips.Pop();
                GameObject chip2 = (GameObject) potChips.Pop();
                dealerScript.putChip(chip1);
                dealerScript.dealerChips.Push(chip1);
                playerScript.putChip(chip2);
                playerScript.playerChips.Push(chip2);
            }
            Debug.Log("Tie");
        }

        // Game over
        StopCoroutine(Play());
        startButton.gameObject.SetActive(true);
    }

    // Game flow
    IEnumerator Play()
    {
        // Get the value of cards on the table and assign them
        // Show the cards of the player
        for (int i = 0; i < numCardsOnTable; i++)  cardScripts[i].Close();
        valueOfCards = deckScript.Deal();
        for (int i = 0; i < numCardsOnTable; i++)  
            cardScripts[i].GetValue((int) valueOfCards[i]);
        
        // Pre-flop round
        cardScripts[0].Open();
        cardScripts[1].Open();
        Round();
        yield return new WaitUntil(() => (actionOver == true));
        actionOver = false;

        // Flop round
        cardScripts[4].Open();
        cardScripts[5].Open();
        cardScripts[6].Open();
        Round();
        yield return new WaitUntil(() => (actionOver == true));
        actionOver = false;

        // Turn round
        cardScripts[7].Open();
        Round();
        yield return new WaitUntil(() => (actionOver == true));
        actionOver = false;

        // River round
        cardScripts[8].Open();
        Round();
        yield return new WaitUntil(() => (actionOver == true));
        actionOver = false;

        // Round over
        RoundOver();
    }
}
