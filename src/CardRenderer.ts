import { Card } from "./Card";

export class CardRenderer {
  static drawCard(ctx: CanvasRenderingContext2D, card: Card, x: number, y: number, width: number = 100, height: number = 140): void {
    const isRed = card.suit === "♥" || card.suit === "♦";
    const color = isRed ? "#E63946" : "#1A1A1A";
    const lightColor = isRed ? "#FF6B7A" : "#4A4A4A";

    // Draw shadow
    ctx.fillStyle = "rgba(0, 0, 0, 0.3)";
    ctx.fillRect(x + 2, y + 3, width, height);

    // Draw gradient background
    const gradient = ctx.createLinearGradient(x, y, x, y + height);
    gradient.addColorStop(0, "#FFFFFF");
    gradient.addColorStop(1, "#F5F5F5");
    ctx.fillStyle = gradient;
    ctx.fillRect(x, y, width, height);

    // Draw border
    ctx.strokeStyle = "#D0D0D0";
    ctx.lineWidth = 1.5;
    ctx.strokeRect(x, y, width, height);

    // Add subtle shine effect
    ctx.fillStyle = "rgba(255, 255, 255, 0.4)";
    ctx.fillRect(x, y, width, height * 0.3);

    // Top-left rank and suit
    ctx.save();
    ctx.translate(x + 8, y + 8);
    
    ctx.fillStyle = color;
    ctx.font = "bold 18px 'Poppins', Arial";
    ctx.textAlign = "left";
    ctx.textBaseline = "top";
    ctx.fillText(card.rankText, 0, 0);
    
    ctx.fillStyle = color;
    ctx.font = "22px Arial";
    ctx.fillText(card.suit, 0, 18);
    
    ctx.restore();

    // Center large suit (main focus)
    ctx.save();
    ctx.translate(x + width / 2, y + height / 2);
    ctx.fillStyle = color;
    ctx.font = "90px Arial";
    ctx.textAlign = "center";
    ctx.textBaseline = "middle";
    ctx.globalAlpha = 0.15;
    ctx.fillText(card.suit, 0, 0);
    ctx.globalAlpha = 1;
    ctx.restore();

    // Bottom-right rank and suit (rotated 180)
    ctx.save();
    ctx.translate(x + width - 8, y + height - 8);
    ctx.rotate(Math.PI);
    
    ctx.fillStyle = color;
    ctx.font = "bold 18px 'Poppins', Arial";
    ctx.textAlign = "left";
    ctx.textBaseline = "top";
    ctx.fillText(card.rankText, 0, 0);
    
    ctx.fillStyle = color;
    ctx.font = "22px Arial";
    ctx.fillText(card.suit, 0, 18);
    
    ctx.restore();
  }

  static drawCardBack(ctx: CanvasRenderingContext2D, x: number, y: number, width: number = 100, height: number = 140): void {
    // Draw shadow
    ctx.fillStyle = "rgba(0, 0, 0, 0.3)";
    ctx.fillRect(x + 2, y + 3, width, height);

    // Draw gradient background
    const gradient = ctx.createLinearGradient(x, y, x, y + height);
    gradient.addColorStop(0, "#1e5f3f");
    gradient.addColorStop(0.5, "#155a38");
    gradient.addColorStop(1, "#0f4d30");
    ctx.fillStyle = gradient;
    ctx.fillRect(x, y, width, height);

    // Draw border
    ctx.strokeStyle = "#31a85e";
    ctx.lineWidth = 2;
    ctx.strokeRect(x, y, width, height);

    // Add pattern
    ctx.strokeStyle = "rgba(49, 168, 94, 0.4)";
    ctx.lineWidth = 1;
    const spacing = 8;
    for (let i = 0; i < height; i += spacing) {
      ctx.beginPath();
      ctx.moveTo(x + 8, y + i);
      ctx.lineTo(x + width - 8, y + i);
      ctx.stroke();
    }

    // Center text
    ctx.fillStyle = "#48ce6b";
    ctx.font = "bold 20px 'Poppins', Arial";
    ctx.textAlign = "center";
    ctx.textBaseline = "middle";
    ctx.fillText("♠", x + width / 2, y + height / 2 - 8);
    ctx.fillText("CARD", x + width / 2, y + height / 2 + 15);
  }
}
