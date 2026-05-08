import { Card } from "./Card";

export class CardRenderer {
  static drawCard(ctx: CanvasRenderingContext2D, card: Card, x: number, y: number, width: number = 100, height: number = 140): void {
    const isRed = card.suit === "♥" || card.suit === "♦";
    const color = isRed ? "#FF0000" : "#000000";

    // Card background
    ctx.fillStyle = "#FFFFFF";
    ctx.strokeStyle = "#000000";
    ctx.lineWidth = 2;
    ctx.fillRect(x, y, width, height);
    ctx.strokeRect(x, y, width, height);

    // Top-left rank
    ctx.fillStyle = color;
    ctx.font = "bold 16px Arial";
    ctx.textAlign = "left";
    ctx.textBaseline = "top";
    ctx.fillText(card.rankText, x + 8, y + 8);

    // Center suit
    ctx.font = "48px Arial";
    ctx.textAlign = "center";
    ctx.textBaseline = "middle";
    ctx.fillText(card.suit, x + width / 2, y + height / 2);

    // Bottom-right rank (rotated)
    ctx.save();
    ctx.translate(x + width, y + height);
    ctx.rotate(Math.PI);
    ctx.font = "bold 16px Arial";
    ctx.fillText(card.rankText, 8, 8);
    ctx.restore();
  }

  static drawCardBack(ctx: CanvasRenderingContext2D, x: number, y: number, width: number = 100, height: number = 140): void {
    ctx.fillStyle = "#0b2d1b";
    ctx.strokeStyle = "#000000";
    ctx.lineWidth = 2;
    ctx.fillRect(x, y, width, height);
    ctx.strokeRect(x, y, width, height);

    ctx.strokeStyle = "#31a85e";
    ctx.lineWidth = 2;
    for (let i = 20; i < height; i += 14) {
      ctx.beginPath();
      ctx.moveTo(x + 12, y + i);
      ctx.lineTo(x + width - 12, y + i);
      ctx.stroke();
    }

    ctx.fillStyle = "#a7ffd6";
    ctx.font = "bold 14px Arial";
    ctx.textAlign = "center";
    ctx.textBaseline = "middle";
    ctx.fillText("CARD", x + width / 2, y + height / 2);
  }
}
