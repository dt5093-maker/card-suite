export enum Suit {
  Spades = "♠",
  Hearts = "♥",
  Diamonds = "♦",
  Clubs = "♣"
}

export enum Rank {
  Two = 2,
  Three = 3,
  Four = 4,
  Five = 5,
  Six = 6,
  Seven = 7,
  Eight = 8,
  Nine = 9,
  Ten = 10,
  Jack = 11,
  Queen = 12,
  King = 13,
  Ace = 14
}

export interface ICard {
  suit: Suit;
  rank: Rank;
}

export class Card implements ICard {
  suit: Suit;
  rank: Rank;

  constructor(suit: Suit, rank: Rank) {
    this.suit = suit;
    this.rank = rank;
  }

  get value(): number {
    return this.rank;
  }

  get rankText(): string {
    switch (this.rank) {
      case Rank.Jack:
        return "J";
      case Rank.Queen:
        return "Q";
      case Rank.King:
        return "K";
      case Rank.Ace:
        return "A";
      default:
        return this.rank.toString();
    }
  }

  toString(): string {
    return `${this.rankText}${this.suit}`;
  }
}
