using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    //This is a class that acts as a struct to store the data of the cards

    public int cardValue = 0;

    public int GetValueOfCard()
    {
        return cardValue;
    }

    public void SetCardValue(int newValue)
    {
        cardValue = newValue;
    }

    public string GetCardName()
    {
        return GetComponent<SpriteRenderer>().sprite.name;
    }

    public void SetCardSprite(Sprite newCardSprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = newCardSprite;
    }

    //When the player wants to play again, or if the player whats to restart
    //we need to reset the card and place it in the deck
    public void ResetCard()
    {
        Sprite back = GameObject.Find("CardDeck").GetComponent<DeckManager>().GetCardBack();
        gameObject.GetComponent<SpriteRenderer>().sprite = back;
        cardValue = 0;
    }
}
