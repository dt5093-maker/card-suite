import { Card, Suit, Rank } from "./Card";

export class Deck {
  private cards: Card[] = [];

  constructor() {
    this.reset();
  }

  reset(): void {
    this.cards = [];
    const suits = Object.values(Suit) as Suit[];
    const ranks = Object.values(Rank).filter(v => typeof v === 'number') as Rank[];

    for (const suit of suits) {
      for (const rank of ranks) {
        this.cards.push(new Card(suit, rank));
      }
    }
    this.shuffle();
  }

  shuffle(): void {
    for (let i = this.cards.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      [this.cards[i], this.cards[j]] = [this.cards[j], this.cards[i]];
    }
  }

  draw(): Card {
    if (this.cards.length === 0) {
      this.reset();
    }
    return this.cards.pop()!;
  }

  getCardsLeft(): number {
    return this.cards.length;
  }
}
