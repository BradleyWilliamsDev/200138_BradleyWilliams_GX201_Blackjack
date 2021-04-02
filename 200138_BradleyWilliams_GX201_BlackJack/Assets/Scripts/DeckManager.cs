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
}
