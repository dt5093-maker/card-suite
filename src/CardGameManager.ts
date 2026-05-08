import { Deck } from "./Deck";
import { Card } from "./Card";
import { CardRenderer } from "./CardRenderer";

type GameMode = "High Card" | "War" | "Blackjack";

export class CardGameManager {
  private deck: Deck;
  private playerScore: number = 0;
  private computerScore: number = 0;
  private roundsPlayed: number = 0;
  private currentMode: GameMode = "High Card";
  private canvas: HTMLCanvasElement;
  private ctx: CanvasRenderingContext2D;
  private blackjackPlayerCards: Card[] = [];
  private blackjackDealerCards: Card[] = [];
  private blackjackGameActive: boolean = false;

  constructor(canvasId: string) {
    this.deck = new Deck();
    this.canvas = document.getElementById(canvasId) as HTMLCanvasElement;
    this.ctx = this.canvas.getContext("2d")!;
    this.setupEventListeners();
    this.render();
  }

  private setupEventListeners(): void {
    const buttons = {
      highCard: document.getElementById("highCardBtn") as HTMLButtonElement,
      war: document.getElementById("warBtn") as HTMLButtonElement,
      blackjack: document.getElementById("blackjackBtn") as HTMLButtonElement,
      play: document.getElementById("playBtn") as HTMLButtonElement,
      reset: document.getElementById("resetBtn") as HTMLButtonElement,
      shuffle: document.getElementById("shuffleBtn") as HTMLButtonElement,
      hit: document.getElementById("hitBtn") as HTMLButtonElement,
      stand: document.getElementById("standBtn") as HTMLButtonElement,
      closeBJ: document.getElementById("closeBlackjackBtn") as HTMLButtonElement
    };

    buttons.highCard.onclick = () => this.setMode("High Card");
    buttons.war.onclick = () => this.setMode("War");
    buttons.blackjack.onclick = () => this.setMode("Blackjack");
    buttons.play.onclick = () => this.play();
    buttons.reset.onclick = () => this.resetGame();
    buttons.shuffle.onclick = () => this.shuffleDeck();
    buttons.hit.onclick = () => this.blackjackHit();
    buttons.stand.onclick = () => this.blackjackStand();
    buttons.closeBJ.onclick = () => this.closeBlackjack();
  }

  private setMode(mode: GameMode): void {
    this.currentMode = mode;
    this.updateStatus(`Switched to ${mode}. Press Play.`);
    this.updateRules();
    this.highlightModeButtons();
  }

  private play(): void {
    if (this.currentMode === "High Card") {
      this.playHighCard();
    } else if (this.currentMode === "War") {
      this.playWar();
    } else {
      this.startBlackjack();
    }
    this.render();
  }

  private playHighCard(): void {
    this.ensureDeck();
    const playerCard = this.deck.draw();
    const computerCard = this.deck.draw();
    this.roundsPlayed++;

    let result = "Tie round. No points awarded.";
    if (playerCard.value > computerCard.value) {
      this.playerScore++;
      result = "Player wins this round!";
    } else if (playerCard.value < computerCard.value) {
      this.computerScore++;
      result = "Computer wins this round!";
    }

    this.updateStatus(result);
    this.updateScore();
    this.drawGameState(playerCard, computerCard);
  }

  private playWar(): void {
    this.ensureDeck();
    const playerCard = this.deck.draw();
    const computerCard = this.deck.draw();
    this.roundsPlayed++;

    let result = "";
    if (playerCard.value > computerCard.value) {
      this.playerScore++;
      result = "Player wins the round!";
    } else if (playerCard.value < computerCard.value) {
      this.computerScore++;
      result = "Computer wins the round!";
    } else {
      result = "War! Draw breaker cards...";
      this.resolveWar();
    }

    this.updateStatus(result);
    this.updateScore();
    this.drawGameState(playerCard, computerCard);
  }

  private resolveWar(): void {
    if (this.deck.getCardsLeft() < 2) {
      this.deck.reset();
    }
    const playerBonus = this.deck.draw();
    const computerBonus = this.deck.draw();

    if (playerBonus.value > computerBonus.value) {
      this.playerScore++;
      this.updateStatus("Player wins the war!");
    } else if (playerBonus.value < computerBonus.value) {
      this.computerScore++;
      this.updateStatus("Computer wins the war!");
    } else {
      this.updateStatus("Another tie in war!");
    }

    this.updateScore();
    this.drawGameState(playerBonus, computerBonus);
  }

  private startBlackjack(): void {
    this.blackjackGameActive = true;
    this.blackjackPlayerCards = [];
    this.blackjackDealerCards = [];
    (document.getElementById("blackjackPanel") as HTMLDivElement).style.display = "block";
    (document.getElementById("hitBtn") as HTMLButtonElement).disabled = false;
    (document.getElementById("standBtn") as HTMLButtonElement).disabled = false;

    this.ensureDeck();
    this.blackjackPlayerCards.push(this.deck.draw());
    this.blackjackPlayerCards.push(this.deck.draw());
    this.blackjackDealerCards.push(this.deck.draw());
    this.blackjackDealerCards.push(this.deck.draw());

    this.updateBlackjackDisplay();
  }

  private blackjackHit(): void {
    if (!this.blackjackGameActive) return;
    this.ensureDeck();
    this.blackjackPlayerCards.push(this.deck.draw());

    if (this.getBlackjackTotal(this.blackjackPlayerCards) > 21) {
      this.updateStatus("Bust! Dealer wins.");
      this.endBlackjack();
    }
    this.updateBlackjackDisplay();
  }

  private blackjackStand(): void {
    if (!this.blackjackGameActive) return;

    while (this.getBlackjackTotal(this.blackjackDealerCards) < 17) {
      this.ensureDeck();
      this.blackjackDealerCards.push(this.deck.draw());
    }

    const playerTotal = this.getBlackjackTotal(this.blackjackPlayerCards);
    const dealerTotal = this.getBlackjackTotal(this.blackjackDealerCards);

    if (playerTotal > 21) {
      this.updateStatus("You bust! Dealer wins.");
      this.computerScore++;
    } else if (dealerTotal > 21) {
      this.updateStatus("Dealer busts! You win!");
      this.playerScore++;
    } else if (playerTotal > dealerTotal) {
      this.updateStatus("You win!");
      this.playerScore++;
    } else if (playerTotal < dealerTotal) {
      this.updateStatus("Dealer wins!");
      this.computerScore++;
    } else {
      this.updateStatus("Push. It's a tie.");
    }

    this.updateScore();
    this.endBlackjack();
  }

  private endBlackjack(): void {
    this.blackjackGameActive = false;
    (document.getElementById("hitBtn") as HTMLButtonElement).disabled = true;
    (document.getElementById("standBtn") as HTMLButtonElement).disabled = true;
  }

  private closeBlackjack(): void {
    (document.getElementById("blackjackPanel") as HTMLDivElement).style.display = "none";
    this.updateStatus("Blackjack session closed.");
  }

  private getBlackjackTotal(cards: Card[]): number {
    let total = 0;
    let aces = 0;

    for (const card of cards) {
      if (card.rank === 11 || card.rank === 12 || card.rank === 13) {
        total += 10;
      } else if (card.rank === 14) {
        total += 11;
        aces++;
      } else {
        total += card.rank;
      }
    }

    while (total > 21 && aces > 0) {
      total -= 10;
      aces--;
    }

    return total;
  }

  private updateBlackjackDisplay(): void {
    const playerTotal = this.getBlackjackTotal(this.blackjackPlayerCards);
    const dealerTotal = this.getBlackjackTotal(this.blackjackDealerCards);
    (document.getElementById("playerTotal") as HTMLDivElement).textContent = `Player: ${playerTotal}`;
    (document.getElementById("dealerTotal") as HTMLDivElement).textContent = `Dealer: ${dealerTotal}`;
    this.drawBlackjackCards();
  }

  private drawBlackjackCards(): void {
    const bCanvas = document.getElementById("blackjackCanvas") as HTMLCanvasElement;
    const bCtx = bCanvas.getContext("2d")!;
    bCtx.fillStyle = "#0b3d0b";
    bCtx.fillRect(0, 0, bCanvas.width, bCanvas.height);

    bCtx.fillStyle = "white";
    bCtx.font = "16px Arial";
    bCtx.fillText("Player:", 20, 30);

    for (let i = 0; i < this.blackjackPlayerCards.length; i++) {
      CardRenderer.drawCard(bCtx, this.blackjackPlayerCards[i], 20 + i * 120, 50, 100, 140);
    }

    bCtx.fillText("Dealer:", 20, 220);
    for (let i = 0; i < this.blackjackDealerCards.length; i++) {
      CardRenderer.drawCard(bCtx, this.blackjackDealerCards[i], 20 + i * 120, 240, 100, 140);
    }
  }

  private ensureDeck(): void {
    if (this.deck.getCardsLeft() < 4) {
      this.deck.reset();
      this.updateStatus("Deck reshuffled.");
    }
  }

  private drawGameState(playerCard: Card, computerCard: Card): void {
    this.ctx.fillStyle = "#051a07";
    this.ctx.fillRect(0, 0, this.canvas.width, this.canvas.height);

    const gradient = this.ctx.createLinearGradient(0, 0, 0, this.canvas.height);
    gradient.addColorStop(0, "#0a3d14");
    gradient.addColorStop(0.5, "#051a07");
    gradient.addColorStop(1, "#0a3d14");
    this.ctx.fillStyle = gradient;
    this.ctx.fillRect(0, 0, this.canvas.width, this.canvas.height);

    this.ctx.strokeStyle = "rgba(31, 207, 94, 0.2)";
    this.ctx.lineWidth = 1;
    this.ctx.strokeRect(20, 20, this.canvas.width - 40, this.canvas.height - 40);

    this.ctx.fillStyle = "#1fcf5e";
    this.ctx.font = "bold 18px 'Poppins', Arial";
    this.ctx.textAlign = "center";
    this.ctx.textBaseline = "top";
    this.ctx.fillText("PLAYER", this.canvas.width / 4, 30);
    this.ctx.fillText("COMPUTER", (3 * this.canvas.width) / 4, 30);

    const cardY = 90;
    const playerCardX = this.canvas.width / 4 - 65;
    const computerCardX = (3 * this.canvas.width) / 4 - 65;

    CardRenderer.drawCard(this.ctx, playerCard, playerCardX, cardY, 130, 180);
    CardRenderer.drawCard(this.ctx, computerCard, computerCardX, cardY, 130, 180);
  }

  private render(): void {
    this.ctx.fillStyle = "#051a07";
    this.ctx.fillRect(0, 0, this.canvas.width, this.canvas.height);

    const gradient = this.ctx.createLinearGradient(0, 0, 0, this.canvas.height);
    gradient.addColorStop(0, "#0a3d14");
    gradient.addColorStop(0.5, "#051a07");
    gradient.addColorStop(1, "#0a3d14");
    this.ctx.fillStyle = gradient;
    this.ctx.fillRect(0, 0, this.canvas.width, this.canvas.height);

    this.ctx.strokeStyle = "rgba(31, 207, 94, 0.2)";
    this.ctx.lineWidth = 1;
    this.ctx.strokeRect(20, 20, this.canvas.width - 40, this.canvas.height - 40);

    this.ctx.fillStyle = "#1fcf5e";
    this.ctx.font = "bold 24px 'Playfair Display', Arial";
    this.ctx.textAlign = "center";
    this.ctx.textBaseline = "middle";
    this.ctx.fillText("Card Suite", this.canvas.width / 2, this.canvas.height / 2);
  }

  private updateStatus(message: string): void {
    const el = document.getElementById("status") as HTMLDivElement;
    if (el) el.textContent = message;
  }

  private updateScore(): void {
    const playerEl = document.getElementById("playerScore") as HTMLDivElement;
    const computerEl = document.getElementById("computerScore") as HTMLDivElement;
    const roundsEl = document.getElementById("roundsScore") as HTMLDivElement;
    const cardsEl = document.getElementById("cardsLeft") as HTMLDivElement;

    if (playerEl) playerEl.textContent = this.playerScore.toString();
    if (computerEl) computerEl.textContent = this.computerScore.toString();
    if (roundsEl) roundsEl.textContent = this.roundsPlayed.toString();
    if (cardsEl) cardsEl.textContent = this.deck.getCardsLeft().toString();
  }

  private updateRules(): void {
    const el = document.getElementById("rules") as HTMLDivElement;
    if (!el) return;
    if (this.currentMode === "High Card") {
      el.textContent = "Draw one card each. Higher card wins.";
    } else if (this.currentMode === "War") {
      el.textContent = "Each round, higher card wins. On tie, draw tiebreaker.";
    } else {
      el.textContent = "Get closest to 21 without busting.";
    }
  }

  private highlightModeButtons(): void {
    const buttons = ["highCardBtn", "warBtn", "blackjackBtn"] as const;
    buttons.forEach(btn => {
      const el = document.getElementById(btn) as HTMLButtonElement;
      if (btn === "highCardBtn" && this.currentMode === "High Card") {
        el.style.backgroundColor = "#18c934";
      } else if (btn === "warBtn" && this.currentMode === "War") {
        el.style.backgroundColor = "#18c934";
      } else if (btn === "blackjackBtn" && this.currentMode === "Blackjack") {
        el.style.backgroundColor = "#18c934";
      } else {
        el.style.backgroundColor = "#1e8a3c";
      }
    });
  }

  private shuffleDeck(): void {
    this.deck.shuffle();
    this.updateStatus("Deck shuffled.");
    this.updateScore();
  }

  private resetGame(): void {
    this.deck = new Deck();
    this.playerScore = 0;
    this.computerScore = 0;
    this.roundsPlayed = 0;
    this.updateScore();
    this.updateStatus("Game reset.");
    this.render();
  }
}
