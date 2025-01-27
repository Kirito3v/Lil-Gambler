using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card 
{
    public string Suit { get; set; }
    public int Rank { get; set; }
    public Sprite Sprite  { get; set; }
	public Card(string suit, int rank, Sprite sprite)
	{
        Suit = suit;
        Rank = rank;
        Sprite = sprite;
	}
}
