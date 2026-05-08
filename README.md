# Card Suite TypeScript Web App

A web-based casino-style card game suite built with TypeScript, Canvas, and vanilla HTML.

## Included Source Files
- `src/Card.ts` - Card and Suit/Rank enums
- `src/Deck.ts` - Deck management and shuffling
- `src/CardRenderer.ts` - Canvas-based card rendering
- `src/CardGameManager.ts` - Game logic for High Card, War, and Blackjack
- `src/index.ts` - Entry point and initialization

## Features
- **High Card** - Draw one card each, higher card wins
- **War** - Each round, higher card wins. On tie, draw tiebreaker.
- **Blackjack** - Get closest to 21 without busting
- Green casino table-style UI
- Canvas-based visual card rendering
- Deck shuffle, reset, and score tracking

## Build & Run

### Setup
```bash
npm install
```

### Development
```bash
npm run dev
```

### Build
```bash
npm run build
```

### Serve
```bash
npm run serve
```

Then open `http://localhost:8080` in your browser.

## Project Structure
```
card-studio/
├── src/                 # TypeScript source files
├── dist/                # Compiled JavaScript and HTML
├── package.json
├── tsconfig.json
└── README.md
```

## Notes
- Built with vanilla TypeScript (no frameworks)
- Uses HTML5 Canvas for card rendering
- Fully responsive green casino styling
- No external dependencies (except TypeScript compiler)
