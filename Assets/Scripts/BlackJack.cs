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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HitClicked()
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
    private void StandClicked()
    {
        deck.StandDrawCards();
        Stand_btn.GetComponent<Button>().enabled = false;
    }
    
    private void DealClicked()
    {
        deck.DealDrawCards();
        Deal_btn.GetComponent<Button>().enabled = false;
    }

    private void BetClicked()
    {
        deck.BetRaise();
    }

    private void DoubleOrNothingClicked() 
    {
        Player.Bet *= 2;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
