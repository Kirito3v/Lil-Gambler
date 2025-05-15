using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Deck2 : MonoBehaviour
{
    [Header("Card Suit")] 
    [SerializeField] Sprite[] Hearts;
    [SerializeField] Sprite[] Diamonds;
    [SerializeField] Sprite[] Clubs;
    [SerializeField] Sprite[] Spades;

    private List<Card> deck = new List<Card>();
    private List<Card> PlayerHand = new List<Card>();
    private List<Card> DealerHand = new List<Card>();

    [Header("UI Cards")]
    [SerializeField] private GameObject[] StartCards;
    [SerializeField] private GameObject[] DrawCards;

    [Header("Compoenets")]
    [SerializeField] private Player Player;
    [SerializeField] private Poker Poker;

    // Hand Ranking
    private enum HandRanking 
    {
        High_Card,
        One_Pair,
        Two_Pair,
        Three_of_a_Kind,
        Straight,
        Flush,
        Full_House,
        Four_of_a_Kind,
        Straight_Flush,
        Royal_Flush
    }

    // Initial values
    public int bet = 0; 
    int round = 0; // counter for each round
    int Sbet = 5; // The bet you pay to start the game

    [SerializeField] Text raise;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        Shuffle();
        Deal();
    }

    # region Initialize the Deck 
    private void Init()
    {
        deck.Clear();
        AddCardsToDeck("Hearts", Hearts);
        AddCardsToDeck("Diamonds", Diamonds);
        AddCardsToDeck("Clubs", Clubs);
        AddCardsToDeck("Spades", Spades);
    }

    private void AddCardsToDeck(string suit, Sprite[] sprites) 
    {
        for (int i = 0; i < sprites.Length; i++) 
            deck.Add(new Card(suit, i + 2, sprites[i]));
    }
    
    private void Shuffle() 
    {
        for (int i = deck.Count - 1; i > 0; i--) 
        {
            int randID = Random.Range(0, i + 1);
            (deck[i], deck[randID]) = (deck[randID], deck[i]);
        }
    }
    #endregion

    #region Button Functions
    // Draw Cards at the start of the game
    private void Deal() 
    {
        PlayerHand = deck.Take(2).ToList();
        DealerHand = deck.Skip(2).Take(5).ToList();
        deck = deck.Skip(7).ToList();

        Player.Score -= Sbet;

        for (int i = 0;i < PlayerHand.Count;i++)
            StartCards[i].GetComponent<SpriteRenderer>().sprite = PlayerHand[i].Sprite;

        for (int i = 0; i < DealerHand.Count; i++)
            StartCards[i + 2].GetComponent<SpriteRenderer>().sprite = DealerHand[i].Sprite;
    }

    // Draw Cards
    public void Check() 
    {
        if (round >= 5) 
        {
            checkRanks();
            Poker.ScoreTxt.text = Player.Score.ToString();
        }
        else 
        {
            PlayerHand.AddRange(deck.Take(1).ToArray());
            deck = deck.Skip(1).ToList();
            DrawCards[round].GetComponent<SpriteRenderer>().sprite = PlayerHand[round + 2].Sprite;
            Poker.PlayerHandRank.text = calRanks(PlayerHand).ToString();
            round++;
        }
    }

    public void Fold() 
    {
        checkRanks(true);
    }

    // Raise the Bet
    public void Raise() 
    {
        bet += int.Parse(raise.text);
        Player.Score -= bet;
    }

    // Now or Never
    public void AllIn() 
    {
        bet += Player.Score;

        checkRanks(false, true);
    }
    #endregion

    #region Check final Hand Rank for Player and Dealer
    private void checkRanks(bool isFold = false, bool isAllIn = false) 
    {
        var PlayerRank = calRanks(PlayerHand);
        var DealerRank = calRanks(DealerHand);

        Poker.PlayerHandRank.text = PlayerRank.ToString();
        Poker.DealerHandRank.text = DealerRank.ToString();

        if (isFold) 
        {

        }
        else if (isAllIn)
        {
            WhoIsHigher(PlayerRank, DealerRank);
        }
        else
        {
            WhoIsHigher(PlayerRank, DealerRank);
        }

        Poker.Check_btn.GetComponent<Button>().enabled = false;
        Poker.Fold_btn.GetComponent<Button>().enabled = false;
        Poker.Raise_btn.GetComponent<Button>().enabled = false;
        Poker.AllIn_btn.GetComponent<Button>().enabled = false;

        Poker.Rest_btn.gameObject.SetActive(true);
    }

    private void WhoIsHigher(HandRanking PlayerRank, HandRanking DealerRank)
    {
        if (PlayerRank > DealerRank)
        {
            Player.Score += (bet + Sbet) * 2;
            Poker.ScoreTxt.text = Player.Score.ToString();
        }
        else if (PlayerRank < DealerRank)
        {

        }
        else
        {
            Player.Score += (bet + Sbet);
        }
    }
    #endregion

    // Calculate Hand Rank
    private HandRanking calRanks(List<Card> hand) 
    {
        var rankCount = hand.
            GroupBy(card => card.Rank).
            ToDictionary(group => group.Key, group => group.Count());
        var suitCount = hand.
            GroupBy(card => card.Suit).
            ToDictionary(group => group.Key, group => group.Count());
        
        var sortedRanks = rankCount.Keys.OrderBy(rank => rank).ToList();

        bool isFlush = suitCount.Any(suit => suit.Value == 5);
        bool isStraight = sortedRanks.Count == 5 && sortedRanks[4] - sortedRanks[0] == 4;
        bool isAceLowStright = sortedRanks.SequenceEqual(new List<int> { 2, 3, 4, 5, 14 });
        bool isRoyalFlush = isFlush && sortedRanks.SequenceEqual(new List<int> { 10, 11, 12, 13, 14 });

        if (isRoyalFlush) return HandRanking.Royal_Flush;
        if (isFlush && isStraight) return HandRanking.Straight_Flush;
        if (rankCount.ContainsValue(4)) return HandRanking.Four_of_a_Kind;
        if (rankCount.ContainsValue(3) && rankCount.ContainsValue(2)) return HandRanking.Full_House;
        if (isFlush) return HandRanking.Flush ;
        if (isStraight || isAceLowStright) return HandRanking.Straight;
        if (rankCount.ContainsValue(3)) return HandRanking.Three_of_a_Kind;
        if (rankCount.Values.Count(v => v == 2) == 2) return HandRanking.Two_Pair;
        if (rankCount.ContainsValue(2)) return HandRanking.One_Pair;
        
        return HandRanking.High_Card;
    }

    // For testing process 
    private string HandToString(List<Card> hand) 
    {
        return string.Join(", ", hand.Select(card => $"{card.Rank} of {card.Suit}"));
    }
}
