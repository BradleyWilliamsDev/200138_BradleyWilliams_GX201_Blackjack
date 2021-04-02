using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Make buttons public so other scripts can acess them
    [Header("Buttons")]
    public Button dealButton, hitButton, stayButton, betButton, loseButton;

    //If player clicks stand twice the hand is over
    private int stayClicks = 0;
    private int playerCards = 0;

    // Have access to the player and dealers hand
    public PlayerAndDealerScript player;
    public PlayerAndDealerScript dealer;

    [Header("Text")]
    public Text stayButtonText, playerBankText, currentPotText, dealerHandText, playerHandText, winText, loseText, bothBustText, totalLoss;

    [Header("Dealer Reveal")]
    public GameObject cardCover;

    int currentPot;

    private bool playerBust, dealerBust, player21, dealer21, player5Card, roundOver;

    void Start()
    {
        //Add listeners to the buttons to check for clicks on the buttons calls the function when clicked
        dealButton.onClick.AddListener(() => DealClicked());
        hitButton.onClick.AddListener(() => HitClicked());
        stayButton.onClick.AddListener(() => StayClicked());
        betButton.onClick.AddListener(() => BetClick());
        loseButton.onClick.AddListener(() => RestartGame());
        loseButton.gameObject.SetActive(false);
        dealButton.gameObject.SetActive(true);
        hitButton.gameObject.SetActive(false);
        stayButton.gameObject.SetActive(false);
        dealerHandText.gameObject.SetActive(false);
        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
        bothBustText.gameObject.SetActive(false);
        totalLoss.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        HandValues();
        CharlieWinCheck();
    }

    void DealClicked()
    {

        // Check if player has enough funds
        //Reset the table once the round is over and deal is clicked again
        player.ResetTableHands();
        dealer.ResetTableHands();

        // Resetting win lose bools

        player21 = false;
        player5Card = false;
        playerBust = false;
        dealer21 = false;
        dealerBust = false;

        dealerHandText.gameObject.SetActive(false);
        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
        bothBustText.gameObject.SetActive(false);
        cardCover.gameObject.SetActive(true);
        dealButton.gameObject.SetActive(false);
        hitButton.gameObject.SetActive(true);
        stayButton.gameObject.SetActive(true);
        GameObject.Find("CardDeck").GetComponent<DeckManager>().Shuffle();
        player.StartDealing();
        dealer.StartDealing();
        stayButtonText.text = "Stay";
        playerCards = 2;


        //Minimal bet to be placed per hand
        currentPot = 40;
        currentPotText.text = "Current Pot\n" + currentPot.ToString();
        player.UpdatePlayerBank(-20);
        playerBankText.text = player.GetMoney().ToString() + " chips";
    }

    void HitClicked()
    {
        //Checks if the player can still have more cards on the table
        if (player.cardIndex <= 5)
        {
            player.DealCard();
            playerCards++;
            if (player.handValue > 20)
            {
                RoundEndCheck();
            }
        }
    }

    void StayClicked()
    {
        hitButton.gameObject.SetActive(false);
        cardCover.gameObject.SetActive(false);
        stayClicks++;
        if (stayClicks > 1)
        {
            RoundEndCheck();
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
            if (dealer.handValue > 20)
            {
                RoundEndCheck();
            }
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
            player5Card = true;

        }
    }

    //Win check
    void RoundEndCheck()
    {
        if (player.handValue > 21)
        {
            playerBust = true;
        }
        else if (dealer.handValue > 21)
        {
            dealerBust = true;
        }
        else if (player.handValue == 21)
        {
            player21 = true;
        }
        else if (dealer.handValue == 21)
        {
            dealer21 = true;
        }

        //If stand has been clicked less than 2 times, and there are no 21's or busts
        if (stayClicks < 2 && !playerBust && !dealerBust && !player21 && !dealer21)
        {
            return;
        }
        roundOver = true;
        //if both bust 
        if (playerBust && dealerBust)
        {
            bothBustText.gameObject.SetActive(true);
            player.UpdatePlayerBank(currentPot / 2);
        }
        // of player busts, or has less value in hand than the dealer
        if (playerBust || (!dealerBust && dealer.handValue > player.handValue))
        {
            loseText.gameObject.SetActive(true);
        }
        else if (dealerBust || (!playerBust && player.handValue > dealer.handValue))
        {
            winText.gameObject.SetActive(true);
            player.UpdatePlayerBank(currentPot);
        }
        else if ((!playerBust && !dealerBust) && player.handValue == dealer.handValue)
        {
            player.UpdatePlayerBank(currentPot / 2);
        }
        else if (player5Card == true)
        {
            winText.gameObject.SetActive(true);
            player.UpdatePlayerBank(currentPot * 2);
        }
        else
        {
            roundOver = false;
        }
        //Checks if the round is over and the player does not have enough money for the next round
        if (roundOver && player.startMoney < 20)
        {
            dealButton.gameObject.SetActive(false);
            hitButton.gameObject.SetActive(false);
            stayButton.gameObject.SetActive(false);
            betButton.gameObject.SetActive(false);
            totalLoss.gameObject.SetActive(true);
            loseButton.gameObject.SetActive(true);
            loseText.gameObject.SetActive(false);

        }
        //Update and reset UI for next round
        else if (roundOver)
        {
            stayButton.gameObject.SetActive(false);
            dealButton.gameObject.SetActive(true);

            cardCover.gameObject.SetActive(false);
            playerBankText.text = "Player current Balance\n" + player.GetMoney().ToString() + " chips";
            stayClicks = 0;
        }
    }

    void BetClick()
    {
        // Increase teh pot by 50;
        int bet = 50;
        if (player.startMoney < 50)
        {
            player.startMoney = player.startMoney;
        }
        else
        {
            player.UpdatePlayerBank(-bet);
            playerBankText.text = "Player current Balance\n" + player.GetMoney().ToString() + " chips";
            // *2 as dealer has to match player bet
            currentPot += (bet * 2);
            currentPotText.text = "Current Pot\n" + currentPot.ToString();
        }

    }

    void RestartGame()
    {
        player.startMoney = 1000;
        playerBankText.text = "Player current Balance\n" + player.GetMoney().ToString() + " chips";
        betButton.gameObject.SetActive(true);
        dealButton.gameObject.SetActive(true);
        totalLoss.gameObject.SetActive(false);
        loseButton.gameObject.SetActive(false);
    }
}