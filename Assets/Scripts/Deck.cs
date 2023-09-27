using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    // arrays for cards
    [SerializeField] private Sprite[] cardSprites;
    [SerializeField] private GameObject[] DealCards;
    [SerializeField] private GameObject[] HitCards;
    [SerializeField] private GameObject[] StandCards;

    // texts for scores
    [SerializeField] private Text PScoreTxt;
    [SerializeField] private Text DScoreTxt;
    [SerializeField] private Text ScoreTxt;
    [SerializeField] private Text BetTxt;

    // gameobject
    [SerializeField] private Player Player;
    [SerializeField] private BlackJack BlackJack;

    // intial values
    public int Hit = 0;
    int Pscore, Dscore = 0;

    // Start is called before the first frame update
    void Start()
    {
        ScoreTxt.text = Player.Score.ToString();
        BetTxt.text = Player.Bet.ToString();
    }

    // get card number
    private int GetCardValue(int cardNum)
    {
        cardNum %= 13;

        if (cardNum > 10 || cardNum == 0)
        {
            cardNum = 10;
            return cardNum;
        }
        else
        {
            return cardNum;
        }
    }

    // draw cards when press deal
    public void DealDrawCards()
    {
        for (int i = 0; i < DealCards.Length; i++)
        {
            int rand = Random.Range(1, 53);
            int Cardnum = GetCardValue(rand);
            DealCards[i].GetComponent<SpriteRenderer>().sprite = cardSprites[rand];

            // win and lose conditions
            if (i == 0) 
            {
                Pscore += GetCardValue(rand);
            }
            else if (i == 1) 
            {
                Pscore += GetCardValue(rand);
                PScoreTxt.text = Pscore.ToString();
            }
            else 
            {
                Dscore += GetCardValue(rand);
                DScoreTxt.text = Dscore.ToString();
            }
        }
        BlackJack.Bet_btn.GetComponent<Button>().enabled = false;
    }

    // draw cards when press hit
    public void HitDrawCards() 
    {
        int rand = Random.Range(1, 53);
        int Cardnum = GetCardValue(rand);
        HitCards[Hit].SetActive(true);
        HitCards[Hit].GetComponent<SpriteRenderer>().sprite = cardSprites[rand];
        Pscore += GetCardValue(rand);
        PScoreTxt.text = Pscore.ToString();
        Hit++;

        // win and lose conditions
        if (Pscore > 21)
        {
            BlackJack.Hit_btn.GetComponent<Button>().enabled = false;
            BlackJack.Rest_btn.gameObject.SetActive(true);

            Player.Score -= Player.Bet;
            ScoreTxt.text = Player.Score.ToString();
        }
        else if (Pscore == 21) 
        {
            BlackJack.Hit_btn.GetComponent<Button>().enabled = false;
            BlackJack.Rest_btn.gameObject.SetActive(true);
            BlackJack.Double_btn.gameObject.SetActive(true);

            Player.Score += Player.Bet;
            ScoreTxt.text = Player.Score.ToString();
        }
    }

    // draw cards when press stand
    public void StandDrawCards() 
    {
        StartCoroutine(TimeToDraw());
    }

    // wait dealer to play
    IEnumerator TimeToDraw() 
    {
        for (int i = 0; i < StandCards.Length; i++) 
        {
            int rand = Random.Range(1, 53);
            int Cardnum = GetCardValue(rand);

            StandCards[i].SetActive(true);
            StandCards[i].GetComponent<SpriteRenderer>().sprite = cardSprites[rand];

            Dscore += GetCardValue(rand);
            DScoreTxt.text = Dscore.ToString();

            // win and lose conditions
            if (Dscore > 21) 
            {
                BlackJack.Rest_btn.gameObject.SetActive(true);
                BlackJack.Double_btn.gameObject.SetActive(true);

                Player.Score += Player.Bet;
                ScoreTxt.text = Player.Score.ToString();
                break;
            }
            else if (Dscore > Pscore) 
            {
                BlackJack.Rest_btn.gameObject.SetActive(true);

                Player.Score -= Player.Bet;
                ScoreTxt.text = Player.Score.ToString();
                break;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    // incress the bet
    public void BetRaise()
    {
        Player.Bet += 20;
        BetTxt.text = Player.Bet.ToString();
    }
}
