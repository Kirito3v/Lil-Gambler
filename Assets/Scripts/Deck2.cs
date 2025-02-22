using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck2 : MonoBehaviour
{
    [SerializeField] Sprite[] Hearts;
    [SerializeField] Sprite[] Diamonds;
    [SerializeField] Sprite[] Clubs;
    [SerializeField] Sprite[] Spades;

    private List<Card> deck = new List<Card>();
    private List<Card> PlayerHand = new List<Card>();
    private List<Card> DealerHand = new List<Card>();

    // Start is called before the first frame update
    void Start()
    {
        Init();
        Shuffle();
        Deal();
        checkRanks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    private void Deal() 
    {
        PlayerHand = deck.Take(5).ToList();
        DealerHand = deck.Skip(5).Take(5).ToList();
        deck = deck.Skip(10).ToList();
    }

    private void checkRanks() 
    {
        Debug.Log("PLayer's Hand: " + HandToString(PlayerHand));
        Debug.Log("Dealer's Hand: " + HandToString(DealerHand));


        var PlayerRank = calRanks(PlayerHand);
        var DealerRank = calRanks(DealerHand);

        Debug.Log($"Player's Rank: {PlayerRank}");
        Debug.Log($"Dealer's Rank: {DealerRank}");
    }

    private string calRanks(List<Card> hand) 
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

        if (isRoyalFlush) return "Royal Flush";
        if (isFlush && isStraight) return "Straight Flush";
        if (rankCount.ContainsValue(4)) return "Four of a Kind";
        if (rankCount.ContainsValue(3) && rankCount.ContainsValue(2)) return "Full House";
        if (isFlush) return "Flush";
        if (isStraight || isAceLowStright) return "Straight";
        if (rankCount.ContainsValue(3)) return "Three of a Kind";
        if (rankCount.Values.Count(v => v == 2) == 2) return "Two Pair";
        if (rankCount.ContainsValue(2)) return "One Pair";
        
        return "High Card";
    }

    private string HandToString(List<Card> hand) 
    {
        return string.Join(", ", hand.Select(card => $"{card.Rank} of {card.Suit}"));
    }
}
