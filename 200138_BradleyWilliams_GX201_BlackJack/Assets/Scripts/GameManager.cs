using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Make buttons public so other scripts can acess them
    [Header("Buttons")]
    public Button dealButton, hitButton, stayButton, betButton;

    //If player clicks stand twice the hand is over
    private int standClicks = 0;
    private int playerCards = 0;

    // Have access to the player and dealers hand
    public PlayerAndDealerScript player;
    public PlayerAndDealerScript dealer;

    [Header("Text")]
    public Text stayButtonText, playerBankText, currentPotText, dealerHandText, playerHandText;

    [Header("Dealer Reveal")]
    public GameObject cardCover;

    int currentPot;

    void Start()
    {
        //Add listeners to the buttons to check for clicks on the buttons calls the function when clicked
        dealButton.onClick.AddListener(() => DealClicked());
        hitButton.onClick.AddListener(() => HitClicked());
        stayButton.onClick.AddListener(() => StayClicked());
        dealButton.gameObject.SetActive(true);
        hitButton.gameObject.SetActive(false);
        stayButton.gameObject.SetActive(false);
        dealerHandText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        HandValues();
        CharlieWinCheck();
    }

    void DealClicked()
    {
        GameObject.Find("CardDeck").GetComponent<DeckManager>().Shuffle();
        player.StartDealing();
        dealer.StartDealing();
        dealButton.gameObject.SetActive(false);
        hitButton.gameObject.SetActive(true);
        stayButton.gameObject.SetActive(true);
        stayButtonText.text = "Stay";
        playerCards = 2;


        //Minimal bet to be placed per hand
        currentPot = 40;
        currentPotText.text = "Current Pot\n" + currentPot.ToString();
        //player.UpdatePlayerBank(-20);
        //playerBankText = player.AddMoney().ToString();
    }

    void HitClicked()
    {
        //Checks if the player can still have more cards on the table
        if (player.DealCard() <= 5)
        {
            player.DealCard();
            playerCards++;
        }
    }

    void StayClicked()
    {
        hitButton.gameObject.SetActive(false);
        cardCover.gameObject.SetActive(false);
        standClicks++;
        if (standClicks > 1)
        {
            Debug.Log("End of Round");
        }
        HitDealer();
        stayButtonText.text = "Call";
        dealerHandText.gameObject.SetActive(true);
    }

    void HitDealer()
    {
        while (dealer.handValue < 16 && dealer.cardIndex < 5)
        {
            dealer.DealCard();
        }
    }

    void HandValues()
    {
        //Update players hand value showing the player how close they are to 21
        //Keeps track of the dealers hand but is still hidden from the player 
        playerHandText.text = "Player Hand Value\n" + player.handValue.ToString();
        dealerHandText.text = "Dealer Hand Value\n" + dealer.handValue.ToString();
    }

    void CharlieWinCheck()
    {
        if (playerCards == 5 && player.handValue < 21)
        {
            
        }
    }

    //Win check
     
}
