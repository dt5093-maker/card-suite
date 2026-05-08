# Unity Card Suite App

This workspace contains the C# Unity implementation of a casino-style card suite game.

## Included Scripts
- `Assets/Scripts/CardData.cs`
- `Assets/Scripts/Deck.cs`
- `Assets/Scripts/CardUIFactory.cs`
- `Assets/Scripts/CardGameManager.cs`

## Features
- High Card
- War
- Blackjack
- Green casino table-style UI
- Visual card rendering using Unity UI
- Deck shuffle, reset, and score tracking

## Setup in Unity
1. Open Unity Hub and create a new 2D project.
2. Copy this workspace into the Unity project folder or open this folder as the Unity project.
3. Create a `Canvas` and add UI elements:
   - `Dropdown` for game mode selection
   - `Buttons` for Play, Reset, Shuffle, Hit, Stand, and Close
   - `Text` objects for status, score, and Blackjack totals
   - `Empty GameObjects` for `PlayerCardArea`, `ComputerCardArea`, `BlackjackPlayerArea`, and `BlackjackDealerArea`
4. Create an empty `GameObject` named `CardGameManager`.
5. Attach the `CardGameManager` script to it.
7. If you do not wire UI references manually, the script will build the Canvas and all needed UI elements at runtime.
8. Save the scene and press Play.

## Notes
- `CardUIFactory` generates card visuals at runtime, so no external art is required.
- `CardGameManager` now also creates the UI automatically if the controls are not already assigned.
- The game logic is implemented entirely in C# for Unity.
- You can improve the visuals later by adding sprite-based card art and animated transitions.
