using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardGameManager : MonoBehaviour
{
    public RectTransform PlayerCardArea;
    public RectTransform ComputerCardArea;
    public Text StatusText;
    public Text ScoreText;
    public Text RulesText;
    public Button HighCardButton;
    public Button WarButton;
    public Button BlackjackButton;
    public Button PlayButton;
    public Button ResetButton;
    public Button ShuffleButton;
    public GameObject BlackjackPanel;
    public RectTransform BlackjackPlayerArea;
    public RectTransform BlackjackDealerArea;
    public Text BlackjackStatusText;
    public Text BlackjackPlayerTotalText;
    public Text BlackjackDealerTotalText;
    public Button HitButton;
    public Button StandButton;
    public Button CloseBlackjackButton;

    private Deck deck;
    private int playerScore;
    private int computerScore;
    private int roundsPlayed;
    private string currentMode = "High Card";
    private List<GameObject> currentCards = new List<GameObject>();
    private List<CardData> blackjackPlayerCards = new List<CardData>();
    private List<CardData> blackjackDealerCards = new List<CardData>();

    private void Awake()
    {
        EnsureUI();

        deck = new Deck();
        BlackjackPanel.SetActive(false);
        PlayButton.onClick.AddListener(OnPlayPressed);
        ResetButton.onClick.AddListener(OnResetPressed);
        ShuffleButton.onClick.AddListener(OnShufflePressed);
        HitButton.onClick.AddListener(OnHitPressed);
        StandButton.onClick.AddListener(OnStandPressed);
        CloseBlackjackButton.onClick.AddListener(CloseBlackjackPanel);
        HighCardButton.onClick.AddListener(() => SetMode("High Card"));
        WarButton.onClick.AddListener(() => SetMode("War"));
        BlackjackButton.onClick.AddListener(() => SetMode("Blackjack"));

        SetMode(currentMode);
        UpdateStatus("Ready to play. Select a game and press Play.");
        UpdateScore();
        ResetTable();
    }

    private void EnsureUI()
    {
        if (PlayerCardArea != null && ComputerCardArea != null && StatusText != null && ScoreText != null && RulesText != null && HighCardButton != null && WarButton != null && BlackjackButton != null && PlayButton != null && ResetButton != null && ShuffleButton != null && BlackjackPanel != null && BlackjackPlayerArea != null && BlackjackDealerArea != null && BlackjackStatusText != null && BlackjackPlayerTotalText != null && BlackjackDealerTotalText != null && HitButton != null && StandButton != null && CloseBlackjackButton != null)
            return;

        BuildRuntimeUI();
    }

    private void BuildRuntimeUI()
    {
        EnsureEventSystem();
        var canvas = CreateCanvas();

        CreateBackground(canvas.transform as RectTransform);
        CreateTitle(canvas.transform as RectTransform);
        CreateControls(canvas.transform as RectTransform);
        CreateStatusPanel(canvas.transform as RectTransform);
        CreateCardAreas(canvas.transform as RectTransform);
        CreateScorePanel(canvas.transform as RectTransform);
        CreateBlackjackPanel(canvas.transform as RectTransform);
    }

    private void EnsureEventSystem()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            var esObj = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            esObj.transform.SetParent(transform, false);
        }
    }

    private Canvas CreateCanvas()
    {
        var canvasGO = new GameObject("CardGameCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasGO.transform.SetParent(transform, false);
        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        var rect = canvasGO.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        return canvas;
    }

    private void CreateBackground(RectTransform parent)
    {
        var background = CreateUIObject("Background", parent);
        background.anchorMin = Vector2.zero;
        background.anchorMax = Vector2.one;
        background.offsetMin = Vector2.zero;
        background.offsetMax = Vector2.zero;
        var image = background.gameObject.AddComponent<Image>();
        image.color = new Color(0.05f, 0.25f, 0.09f);
    }

    private void CreateTitle(RectTransform parent)
    {
        var title = CreateText("TitleText", parent, "Card Suite Free Games", 40, TextAnchor.MiddleCenter, new Vector2(800, 80));
        title.rectTransform.anchoredPosition = new Vector2(0, 450);
        title.color = Color.white;
    }

    private void CreateControls(RectTransform parent)
    {
        var controlPanel = CreateUIObject("ControlPanel", parent);
        controlPanel.sizeDelta = new Vector2(1100, 120);
        controlPanel.anchoredPosition = new Vector2(0, 340);

        HighCardButton = CreateButton(controlPanel, "High Card", new Vector2(200, 60), new Vector2(-360, 0));
        WarButton = CreateButton(controlPanel, "War", new Vector2(200, 60), new Vector2(-120, 0));
        BlackjackButton = CreateButton(controlPanel, "Blackjack", new Vector2(200, 60), new Vector2(120, 0));
        PlayButton = CreateButton(controlPanel, "Play", new Vector2(180, 60), new Vector2(360, 0));
        ResetButton = CreateButton(controlPanel, "Reset", new Vector2(180, 60), new Vector2(520, 0));
        ShuffleButton = CreateButton(controlPanel, "Shuffle", new Vector2(180, 60), new Vector2(680, 0));
    }

    private void CreateStatusPanel(RectTransform parent)
    {
        var statusPanel = CreateUIObject("StatusPanel", parent);
        statusPanel.sizeDelta = new Vector2(1100, 110);
        statusPanel.anchoredPosition = new Vector2(0, 240);

        var panelImage = statusPanel.gameObject.AddComponent<Image>();
        panelImage.color = new Color(0.1f, 0.4f, 0.16f, 0.8f);

        StatusText = CreateText("StatusText", statusPanel, "Ready to play. Select a game and press Play.", 24, TextAnchor.MiddleLeft, new Vector2(1040, 90));
        StatusText.rectTransform.anchoredPosition = new Vector2(-20, 0);
        StatusText.color = Color.white;

        RulesText = CreateText("RulesText", statusPanel, GetRulesText(), 20, TextAnchor.LowerLeft, new Vector2(1040, 60));
        RulesText.rectTransform.anchoredPosition = new Vector2(-20, -20);
        RulesText.color = Color.white;
    }

    private void CreateCardAreas(RectTransform parent)
    {
        var cardsPanel = CreateUIObject("CardsPanel", parent);
        cardsPanel.sizeDelta = new Vector2(1100, 230);
        cardsPanel.anchoredPosition = new Vector2(0, 70);

        PlayerCardArea = CreateCardArea(cardsPanel, "Player", new Vector2(-280, 0));
        ComputerCardArea = CreateCardArea(cardsPanel, "Computer", new Vector2(280, 0));
    }

    private RectTransform CreateCardArea(RectTransform parent, string label, Vector2 position)
    {
        var area = CreateUIObject(label + "Area", parent);
        area.sizeDelta = new Vector2(420, 210);
        area.anchoredPosition = position;

        var image = area.gameObject.AddComponent<Image>();
        image.color = new Color(0.08f, 0.35f, 0.15f, 0.9f);

        var title = CreateText(label + "Label", area, label, 26, TextAnchor.UpperCenter, new Vector2(380, 40));
        title.rectTransform.anchoredPosition = new Vector2(0, 85);
        title.color = Color.white;

        return area;
    }

    private void CreateScorePanel(RectTransform parent)
    {
        var scorePanel = CreateUIObject("ScorePanel", parent);
        scorePanel.sizeDelta = new Vector2(1100, 90);
        scorePanel.anchoredPosition = new Vector2(0, -100);

        var panelImage = scorePanel.gameObject.AddComponent<Image>();
        panelImage.color = new Color(0.1f, 0.4f, 0.16f, 0.8f);

        ScoreText = CreateText("ScoreText", scorePanel, "Rounds: 0  Player: 0  Computer: 0  Cards left: 52", 22, TextAnchor.MiddleCenter, new Vector2(1040, 80));
        ScoreText.rectTransform.anchoredPosition = Vector2.zero;
        ScoreText.color = Color.white;
    }

    private void CreateBlackjackPanel(RectTransform parent)
    {
        BlackjackPanel = CreateUIObject("BlackjackPanel", parent).gameObject;
        var panelRect = BlackjackPanel.GetComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(1000, 500);
        panelRect.anchoredPosition = new Vector2(0, -30);

        var panelImage = BlackjackPanel.AddComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 0.7f);

        var header = CreateText("BlackjackHeader", panelRect, "Blackjack", 34, TextAnchor.UpperCenter, new Vector2(980, 60));
        header.rectTransform.anchoredPosition = new Vector2(0, 210);
        header.color = Color.white;

        var leftPanel = CreateUIObject("BlackjackLeft", panelRect);
        leftPanel.sizeDelta = new Vector2(460, 280);
        leftPanel.anchoredPosition = new Vector2(-240, 10);
        var leftImage = leftPanel.gameObject.AddComponent<Image>();
        leftImage.color = new Color(0.1f, 0.4f, 0.16f, 0.9f);

        BlackjackPlayerArea = CreateUIObject("BlackjackPlayerArea", leftPanel);
        BlackjackPlayerArea.sizeDelta = new Vector2(420, 190);
        BlackjackPlayerArea.anchoredPosition = new Vector2(0, -15);
        var playerAreaImage = BlackjackPlayerArea.gameObject.AddComponent<Image>();
        playerAreaImage.color = new Color(0.05f, 0.25f, 0.12f, 1f);

        BlackjackPlayerTotalText = CreateText("PlayerTotal", leftPanel, "Player Total: 0", 24, TextAnchor.LowerLeft, new Vector2(420, 40));
        BlackjackPlayerTotalText.rectTransform.anchoredPosition = new Vector2(0, -120);
        BlackjackPlayerTotalText.color = Color.white;

        var rightPanel = CreateUIObject("BlackjackRight", panelRect);
        rightPanel.sizeDelta = new Vector2(460, 280);
        rightPanel.anchoredPosition = new Vector2(240, 10);
        var rightImage = rightPanel.gameObject.AddComponent<Image>();
        rightImage.color = new Color(0.1f, 0.4f, 0.16f, 0.9f);

        BlackjackDealerArea = CreateUIObject("BlackjackDealerArea", rightPanel);
        BlackjackDealerArea.sizeDelta = new Vector2(420, 190);
        BlackjackDealerArea.anchoredPosition = new Vector2(0, -15);
        var dealerAreaImage = BlackjackDealerArea.gameObject.AddComponent<Image>();
        dealerAreaImage.color = new Color(0.05f, 0.25f, 0.12f, 1f);

        BlackjackDealerTotalText = CreateText("DealerTotal", rightPanel, "Dealer Total: ?", 24, TextAnchor.LowerLeft, new Vector2(420, 40));
        BlackjackDealerTotalText.rectTransform.anchoredPosition = new Vector2(0, -120);
        BlackjackDealerTotalText.color = Color.white;

        BlackjackStatusText = CreateText("BlackjackStatus", panelRect, "Draw cards or stand to play.", 24, TextAnchor.MiddleCenter, new Vector2(900, 60));
        BlackjackStatusText.rectTransform.anchoredPosition = new Vector2(0, -160);
        BlackjackStatusText.color = Color.white;

        HitButton = CreateButton(panelRect, "Hit", new Vector2(180, 60), new Vector2(-200, -230));
        StandButton = CreateButton(panelRect, "Stand", new Vector2(180, 60), new Vector2(0, -230));
        CloseBlackjackButton = CreateButton(panelRect, "Close", new Vector2(180, 60), new Vector2(200, -230));
    }

    private RectTransform CreateUIObject(string name, Transform parent)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var rect = go.GetComponent<RectTransform>();
        rect.localScale = Vector3.one;
        return rect;
    }

    private Text CreateText(string name, Transform parent, string text, int fontSize, TextAnchor anchor, Vector2 size, Vector2? anchoredPosition = null)
    {
        var rect = CreateUIObject(name, parent);
        rect.sizeDelta = size;
        rect.anchoredPosition = anchoredPosition ?? Vector2.zero;
        var uiText = rect.gameObject.AddComponent<Text>();
        uiText.text = text;
        uiText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        uiText.fontSize = fontSize;
        uiText.alignment = anchor;
        uiText.horizontalOverflow = HorizontalWrapMode.Wrap;
        uiText.verticalOverflow = VerticalWrapMode.Truncate;
        uiText.color = Color.white;
        return uiText;
    }

    private Button CreateButton(RectTransform parent, string label, Vector2 size, Vector2 position)
    {
        var rect = CreateUIObject(label + "Button", parent);
        rect.sizeDelta = size;
        rect.anchoredPosition = position;
        var image = rect.gameObject.AddComponent<Image>();
        image.color = new Color(0.12f, 0.45f, 0.2f, 1f);
        var button = rect.gameObject.AddComponent<Button>();
        var text = CreateText("Text", rect, label, 22, TextAnchor.MiddleCenter, size);
        text.color = Color.white;
        return button;
    }

    private void SetMode(string mode)
    {
        currentMode = mode;
        UpdateRules();
        UpdateStatus($"Switched to {mode}. Press Play.");
        HighlightModeButtons();
    }

    private void HighlightModeButtons()
    {
        var selectedColor = new Color(0.18f, 0.65f, 0.36f, 1f);
        var normalColor = new Color(0.12f, 0.45f, 0.2f, 1f);
        SetButtonColor(HighCardButton, currentMode == "High Card" ? selectedColor : normalColor);
        SetButtonColor(WarButton, currentMode == "War" ? selectedColor : normalColor);
        SetButtonColor(BlackjackButton, currentMode == "Blackjack" ? selectedColor : normalColor);
    }

    private void SetButtonColor(Button button, Color color)
    {
        if (button == null) return;
        var image = button.GetComponent<Image>();
        if (image != null) image.color = color;
    }

    private void OnPlayPressed()
    {
        if (currentMode == "High Card")
            PlayHighCard();
        else if (currentMode == "War")
            PlayWar();
        else
            StartBlackjack();
    }

    private void PlayHighCard()
    {
        EnsureDeck();
        var playerCard = deck.Draw();
        var computerCard = deck.Draw();
        ResetTable();
        DrawCard(playerCard, PlayerCardArea, Vector2.zero);
        DrawCard(computerCard, ComputerCardArea, Vector2.zero);
        roundsPlayed++;

        if (playerCard.Value > computerCard.Value)
            playerScore++;
        else if (playerCard.Value < computerCard.Value)
            computerScore++;

        UpdateStatus(playerCard.Value > computerCard.Value ? "Player wins this round!" : playerCard.Value < computerCard.Value ? "Computer wins this round!" : "Tie round. No points awarded.");
        UpdateScore();
    }

    private void PlayWar()
    {
        EnsureDeck();
        var playerCard = deck.Draw();
        var computerCard = deck.Draw();
        ResetTable();
        DrawCard(playerCard, PlayerCardArea, Vector2.zero);
        DrawCard(computerCard, ComputerCardArea, Vector2.zero);
        roundsPlayed++;

        if (playerCard.Value > computerCard.Value)
        {
            playerScore++;
            UpdateStatus("Player wins the round.");
        }
        else if (playerCard.Value < computerCard.Value)
        {
            computerScore++;
            UpdateStatus("Computer wins the round.");
        }
        else
        {
            UpdateStatus("War! Drawing tie breaker cards...");
            ResolveWar();
        }

        UpdateScore();
    }

    private void ResolveWar()
    {
        if (deck.Cards.Count < 2)
            deck.Reset();

        var playerBonus = deck.Draw();
        var computerBonus = deck.Draw();
        ResetTable();
        DrawCard(playerBonus, PlayerCardArea, Vector2.zero);
        DrawCard(computerBonus, ComputerCardArea, Vector2.zero);

        if (playerBonus.Value > computerBonus.Value)
        {
            playerScore++;
            UpdateStatus("Player wins the war!");
        }
        else if (playerBonus.Value < computerBonus.Value)
        {
            computerScore++;
            UpdateStatus("Computer wins the war!");
        }
        else
        {
            UpdateStatus("Another tie in war! No extra points.");
        }

        UpdateScore();
    }

    private void StartBlackjack()
    {
        BlackjackPanel.SetActive(true);
        HitButton.interactable = true;
        StandButton.interactable = true;
        blackjackPlayerCards.Clear();
        blackjackDealerCards.Clear();
        ResetHandTable();
        UpdateBlackjackStatus("Blackjack started. Hit or stand.");
        DrawBlackjackCard(blackjackPlayerCards, BlackjackPlayerArea);
        DrawBlackjackCard(blackjackPlayerCards, BlackjackPlayerArea);
        DrawBlackjackCard(blackjackDealerCards, BlackjackDealerArea);
        DrawBlackjackCard(blackjackDealerCards, BlackjackDealerArea);
        UpdateBlackjackTotals();
    }

    private void OnHitPressed()
    {
        DrawBlackjackCard(blackjackPlayerCards, BlackjackPlayerArea);
        UpdateBlackjackTotals();

        if (GetBlackjackTotal(blackjackPlayerCards) > 21)
        {
            UpdateBlackjackStatus("Bust! Dealer wins.");
            EndBlackjack(false);
        }
    }

    private void OnStandPressed()
    {
        while (GetBlackjackTotal(blackjackDealerCards) < 17)
        {
            DrawBlackjackCard(blackjackDealerCards, BlackjackDealerArea);
            UpdateBlackjackTotals();
        }

        var playerTotal = GetBlackjackTotal(blackjackPlayerCards);
        var dealerTotal = GetBlackjackTotal(blackjackDealerCards);

        if (dealerTotal > 21 || playerTotal > dealerTotal)
        {
            playerScore++;
            UpdateBlackjackStatus("You win Blackjack!");
        }
        else if (playerTotal < dealerTotal)
        {
            computerScore++;
            UpdateBlackjackStatus("Dealer wins Blackjack.");
        }
        else
        {
            UpdateBlackjackStatus("Push. It is a tie.");
        }

        EndBlackjack(true);
    }

    private void EndBlackjack(bool updateScore)
    {
        HitButton.interactable = false;
        StandButton.interactable = false;
        if (updateScore)
            UpdateScore();
    }

    private void CloseBlackjackPanel()
    {
        BlackjackPanel.SetActive(false);
        UpdateStatus("Blackjack session closed. Select another game.");
        ResetTable();
    }

    private void DrawCard(CardData card, RectTransform parent, Vector2 position)
    {
        var created = CardUIFactory.CreateCard(parent, card, position);
        currentCards.Add(created);
    }

    private void DrawBlackjackCard(List<CardData> hand, RectTransform parent)
    {
        EnsureDeck();
        var card = deck.Draw();
        hand.Add(card);
        CardUIFactory.CreateCard(parent, card, new Vector2(-150 + (hand.Count - 1) * 130, 0));
    }

    private void ResetTable()
    {
        foreach (var card in currentCards)
        {
            if (card != null)
                Destroy(card);
        }
        currentCards.Clear();
    }

    private void ResetHandTable()
    {
        if (BlackjackPlayerArea != null)
        {
            foreach (Transform child in BlackjackPlayerArea)
                Destroy(child.gameObject);
        }

        if (BlackjackDealerArea != null)
        {
            foreach (Transform child in BlackjackDealerArea)
                Destroy(child.gameObject);
        }
    }

    private int GetBlackjackTotal(List<CardData> cards)
    {
        var total = 0;
        var aces = 0;
        foreach (var card in cards)
        {
            if (card.Rank == Rank.Jack || card.Rank == Rank.Queen || card.Rank == Rank.King)
                total += 10;
            else if (card.Rank == Rank.Ace)
            {
                total += 11;
                aces++;
            }
            else
                total += (int)card.Rank;
        }

        while (total > 21 && aces > 0)
        {
            total -= 10;
            aces--;
        }

        return total;
    }

    private void EnsureDeck()
    {
        if (deck.Cards.Count < 4)
        {
            deck.Reset();
            UpdateStatus("Deck reshuffled.");
        }
    }

    private void UpdateStatus(string message)
    {
        if (StatusText != null)
            StatusText.text = message;
    }

    private void UpdateScore()
    {
        if (ScoreText != null)
            ScoreText.text = $"Rounds: {roundsPlayed}  Player: {playerScore}  Computer: {computerScore}  Cards left: {deck.Cards.Count}";
    }

    private void UpdateBlackjackStatus(string message)
    {
        if (BlackjackStatusText != null)
            BlackjackStatusText.text = message;
    }

    private void UpdateBlackjackTotals()
    {
        if (BlackjackPlayerTotalText != null)
            BlackjackPlayerTotalText.text = $"Player Total: {GetBlackjackTotal(blackjackPlayerCards)}";
        if (BlackjackDealerTotalText != null)
            BlackjackDealerTotalText.text = $"Dealer Total: {GetBlackjackTotal(blackjackDealerCards)}";
    }

    private void UpdateRules()
    {
        if (RulesText == null)
            return;

        if (currentMode == "High Card")
            RulesText.text = "High Card: Draw one card for you and one for the computer. Higher rank wins. Tie means no score change.";
        else if (currentMode == "War")
            RulesText.text = "War: Each round draws one card each. Higher card wins the round. If tied, each player draws a tie-breaker card.";
        else
            RulesText.text = "Blackjack: Draw cards until you stop or exceed 21. You play against the dealer. Closest to 21 without going over wins.";
    }

    private void OnShufflePressed()
    {
        deck.Shuffle();
        UpdateStatus("Deck shuffled. Ready for the next game.");
        UpdateScore();
    }

    private void OnResetPressed()
    {
        deck.Reset();
        playerScore = 0;
        computerScore = 0;
        roundsPlayed = 0;
        ResetTable();
        UpdateScore();
        UpdateStatus("Game reset. Ready to play.");
    }

    private string GetRulesText()
    {
        return "High Card: Draw one card for you and one for the computer. Higher rank wins. Tie means no score change.";
    }
}
