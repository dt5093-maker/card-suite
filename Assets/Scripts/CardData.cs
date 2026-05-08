using UnityEngine;

public enum Suit
{
    Spades,
    Hearts,
    Diamonds,
    Clubs
}

public enum Rank
{
    Two = 2,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Ace
}

[System.Serializable]
public struct CardData
{
    public Suit Suit;
    public Rank Rank;

    public int Value => (int)Rank;
    public string RankText => RankToString(Rank);
    public string SuitText => SuitToSymbol(Suit);

    public override string ToString()
    {
        return $"{RankText}{SuitText}";
    }

    public static string RankToString(Rank rank)
    {
        switch (rank)
        {
            case Rank.Ten: return "10";
            case Rank.Jack: return "J";
            case Rank.Queen: return "Q";
            case Rank.King: return "K";
            case Rank.Ace: return "A";
            default: return ((int)rank).ToString();
        }
    }

    public static string SuitToSymbol(Suit suit)
    {
        switch (suit)
        {
            case Suit.Spades: return "♠";
            case Suit.Hearts: return "♥";
            case Suit.Diamonds: return "♦";
            case Suit.Clubs: return "♣";
            default: return "?";
        }
    }
}
