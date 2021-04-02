using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Make buttons public so other scripts can acess them
    public Button dealButton, hitButton, stayButton, betButton;

    // Have access to the player and dealers hand
    public PlayerAndDealerScript player;
    public PlayerAndDealerScript dealer;
    void Start()
    {
        //Add listeners to the buttons to check for clicks on the buttons calls the function when clicked
        dealButton.onClick.AddListener(() => DealClicked());
        hitButton.onClick.AddListener(() => HitClicked());
        stayButton.onClick.AddListener(() => StayClicked());
        dealButton.gameObject.SetActive(true);
        hitButton.gameObject.SetActive(false);
        stayButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DealClicked()
    {
        GameObject.Find("CardDeck").GetComponent<DeckManager>().Shuffle();
        player.StartDealing();
        dealer.StartDealing();
        dealButton.gameObject.SetActive(false);
        hitButton.gameObject.SetActive(true);
        stayButton.gameObject.SetActive(true);
    }

    void HitClicked()
    {

    }

    void StayClicked()
    {

    }
}
