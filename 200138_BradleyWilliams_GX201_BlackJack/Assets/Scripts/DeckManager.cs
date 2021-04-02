using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{

    public Sprite[] cardsToBeDealt;
    private int[] cardValues = new int[53];
    int deckStartingNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        ValueOfCard();
    }

    // Update is called once per frame
    void ValueOfCard()
    {
        int num = 0;
        //This loop goes through the deck and assigns each value to the cards
        for (int i = 0; i < cardsToBeDealt.Length; i++)
        {
            //This makes sure the program goes through every card
            num = i;
            //Mod 13 takes the remainder of the card position and that is the cards Value.
            num %= 13;
            if (num > 10 || num == 0)
            {
                num = 10;
            }
            cardValues[i] = num++;
        }
        deckStartingNum = 1;
    }

    //Make function public so that other classes can access it
    public void Shuffle()
    {
        //The loop goes backwards through the deck swapping cards in the array around -1 to get the correct number of elements to exclude the back of the card
        for (int i = cardsToBeDealt.Length - 1; i > 0; i--)
        {
            //FloorToInt takes away the decimal in a float and makes it an int, gets a random number in the deck to be the value of j, 
            //then swaps i and j around and finally sets the value and card face of J
            int j = Mathf.FloorToInt(Random.Range(0.0f, 1.0f) * cardValues.Length - 1) + 1;
            Sprite cardFace = cardsToBeDealt[i];
            cardsToBeDealt[i] = cardsToBeDealt[j];
            cardsToBeDealt[j] = cardFace;

            int cardValue = cardValues[i];
            cardValues[i] = cardValues[j];
            cardValues[j] = cardValue;
        }
    }

    // pulblic for other classes to access
    public int DealCard(CardScript cardScript)
    {
        //Use card script to deal and set the card sprite
        cardScript.SetCardSprite(cardsToBeDealt[deckStartingNum]);
        //Gets the card value then moves to the next position in the deck
        cardScript.SetCardValue(cardValues[deckStartingNum++]);
        return cardScript.GetValueOfCard();
    }

    public Sprite GetCardBack()
    {
        return cardsToBeDealt[0];
    }
}
