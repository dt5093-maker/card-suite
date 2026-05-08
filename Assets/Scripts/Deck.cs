using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    public List<CardData> Cards { get; private set; }

    public Deck()
    {
        Reset();
    }

    public void Reset()
    {
        Cards = new List<CardData>();

        foreach (Suit suit in System.Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in System.Enum.GetValues(typeof(Rank)))
            {
                Cards.Add(new CardData { Suit = suit, Rank = rank });
            }
        }

        Shuffle();
    }

    public void Shuffle()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            int j = Random.Range(i, Cards.Count);
            var tmp = Cards[i];
            Cards[i] = Cards[j];
            Cards[j] = tmp;
        }
    }

    public CardData Draw()
    {
        if (Cards.Count == 0)
        {
            Reset();
        }

        var card = Cards[Cards.Count - 1];
        Cards.RemoveAt(Cards.Count - 1);
        return card;
    }
}
