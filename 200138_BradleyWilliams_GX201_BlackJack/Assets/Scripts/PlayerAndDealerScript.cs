using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAndDealerScript : MonoBehaviour
{
    //This class will be using functions from these other scripts so you must declare them
    public CardScript cardScript;
    public DeckManager deckScript;

    //Keeping total value of the player and dealers hands
    public int handValue = 0;
    //Players starting money 
    public int startMoney = 1000;

    //Cards on the table
    public GameObject[] hand;

    //Card index for next cards
    public int cardIndex = 0;

    //tracking aces for 1 to 11 conversions
    List<CardScript> aceList = new List<CardScript>();
    public void StartDealing()
    {
        DealCard();
        DealCard();
    }

    //Add a hand to the player and dealers hands
    public int DealCard()
    {
        //Gets the card to be dealts value and assigns a sprite to a card that will be in play
        int cardValue = deckScript.DealCard(hand[cardIndex].GetComponent<CardScript>());
        //Actually physically display the card on the screen
        hand[cardIndex].GetComponent<Renderer>().enabled = true;
        //Add the card value to the player/dealers hand
        handValue += cardValue;
        //Check for aces
        if (cardValue == 1)
        {
            aceList.Add(hand[cardIndex].GetComponent<CardScript>());
        }
        //AceCheck checks player or dealers hand to know whether or not to use a 1 or 11 as it's value
        AceCheck();
        cardIndex++;
        return handValue;
    }

    // Checks for aces and decides on the value of the ace
    public void AceCheck()
    {
        foreach (CardScript ace in aceList)
        {
            //Checks for every ace in the list
            if (handValue + 10 < 22 && ace.GetValueOfCard() == 1)
            {
                //if converting, adjust card value and hand value
                ace.SetCardValue(11);
                handValue += 10;
            }
            else if (handValue > 21 && ace.GetValueOfCard() == 11)
            {
                ace.SetCardValue(1);
                handValue -= 10;
            }
        }
    }

    // change the players bank balance
    public void UpdatePlayerBank(int amount)
    {
        startMoney += amount;
    }

    //Output the players current amount of money
    public int GetMoney()
    {
        return startMoney;
    }

    public void ResetTableHands()
    {
        for (int i = 0; i < hand.Length; i++)
        {
            hand[i].GetComponent<CardScript>().ResetCard();
            hand[i].GetComponent<Renderer>().enabled = false;
        }
        cardIndex = 0;
        handValue = 0;
        aceList = new List<CardScript>();
    }
}
