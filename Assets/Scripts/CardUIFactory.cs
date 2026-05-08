using UnityEngine;
using UnityEngine.UI;

public static class CardUIFactory
{
    public static GameObject CreateCard(RectTransform parent, CardData cardData, Vector2 position)
    {
        var cardGO = new GameObject($"Card_{cardData}", typeof(RectTransform), typeof(Image));
        cardGO.transform.SetParent(parent, false);

        var rectTransform = cardGO.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(120, 170);
        rectTransform.anchoredPosition = position;

        var image = cardGO.GetComponent<Image>();
        image.color = Color.white;

        var topText = CreateText(cardGO.transform, cardData.RankText, new Vector2(-40, 65), 24, TextAnchor.UpperLeft, cardData.Suit == Suit.Hearts || cardData.Suit == Suit.Diamonds ? Color.red : Color.black);
        var centerText = CreateText(cardGO.transform, cardData.SuitText, Vector2.zero, 50, TextAnchor.MiddleCenter, cardData.Suit == Suit.Hearts || cardData.Suit == Suit.Diamonds ? Color.red : Color.black);
        var bottomText = CreateText(cardGO.transform, cardData.RankText, new Vector2(40, -65), 24, TextAnchor.LowerRight, cardData.Suit == Suit.Hearts || cardData.Suit == Suit.Diamonds ? Color.red : Color.black);
        bottomText.rectTransform.localEulerAngles = new Vector3(0, 0, 180);

        return cardGO;
    }

    public static GameObject CreateCardBack(RectTransform parent, Vector2 position)
    {
        var cardGO = new GameObject("CardBack", typeof(RectTransform), typeof(Image));
        cardGO.transform.SetParent(parent, false);

        var rectTransform = cardGO.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(120, 170);
        rectTransform.anchoredPosition = position;

        var image = cardGO.GetComponent<Image>();
        image.color = new Color(0.05f, 0.18f, 0.11f, 1f);

        CreateText(cardGO.transform, "CAS", Vector2.zero, 20, TextAnchor.MiddleCenter, new Color(0.7f, 1f, 0.8f));
        return cardGO;
    }

    private static Text CreateText(Transform parent, string content, Vector2 anchoredPosition, int fontSize, TextAnchor anchor, Color color)
    {
        var textGO = new GameObject("Text", typeof(RectTransform), typeof(Text));
        textGO.transform.SetParent(parent, false);

        var rectTransform = textGO.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(110, 40);
        rectTransform.anchoredPosition = anchoredPosition;

        var text = textGO.GetComponent<Text>();
        text.text = content;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = fontSize;
        text.alignment = anchor;
        text.color = color;

        return text;
    }
}
