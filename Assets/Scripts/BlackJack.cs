using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlackJack : MonoBehaviour 
{
    public Button Hit_btn;
    public Button Stand_btn;
    public Button Deal_btn;
    public Button Back_btn;
    public Button Bet_btn;
    public Button Rest_btn;
    public Button Double_btn;
    
    private bool isDealed = false;
    private bool isDouble = false;

    [SerializeField] private Deck deck;
    [SerializeField] private Player Player;

    // Start is called before the first frame update
    void Start()
    {
        Hit_btn.onClick.AddListener( () => HitClicked());
        Stand_btn.onClick.AddListener( () => StandClicked());
        Deal_btn.onClick.AddListener( () => DealClicked());
        Back_btn.onClick.AddListener(() => BackClicked());
        Bet_btn.onClick.AddListener(() => BetClicked());
        Rest_btn.onClick.AddListener(() => RestClicked());
        Double_btn.onClick.AddListener(() => DoubleOrNothingClicked());
    }

    private void HitClicked()
    {
        if (isDealed) 
        {
            if (deck.Hit == 5)
            {
                Hit_btn.GetComponent<Button>().enabled = false;
            }
            else
            {
                deck.HitDrawCards();
            }
        }
    }
    private void StandClicked()
    {
        if (isDealed) 
        {
            deck.StandDrawCards();
            Stand_btn.GetComponent<Button>().enabled = false;
        }
    }
    
    private void DealClicked()
    {
        if (Player.Bet > 0 || isDouble) 
        {
            isDealed = true;
            deck.DealDrawCards();
            Deal_btn.GetComponent<Button>().enabled = false;
        }
    }

    private void BetClicked()
    {
        deck.BetRaise();
    }

    private void DoubleOrNothingClicked() 
    {
        deck.DoubleOrNothing();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isDouble = true;
    }

    private void RestClicked()
    {
        Player.Bet = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void BackClicked() 
    {
        Player.Bet = 0;
        SceneManager.LoadScene("Game");
    }
}
