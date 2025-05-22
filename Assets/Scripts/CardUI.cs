using UnityEngine;
using TMPro;

public class CardUI : MonoBehaviour
{
    [System.Serializable]
    private class Card
    {
        public string id;
        public string characterName;
        public string imageName;
        public string text;
        public string leftOption;
        public string rightOption;
        public int leftEconomyEffect;
        public int leftMilitaryEffect;
        public int leftReligionEffect;
        public int leftPeopleEffect;
        public int rightEconomyEffect;
        public int rightMilitaryEffect;
        public int rightReligionEffect;
        public int rightPeopleEffect;
        public string[] tags;
    }

    [System.Serializable]
    private class CardList
    {
        public Card[] cards;
    }
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI cardText;
    public TextMeshProUGUI leftOptionText;
    public TextMeshProUGUI rightOptionText;

    private CardList JsonReader()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Cards");
        if (jsonFile != null)
        {
            // Parse the JSON data
            CardList cardList = JsonUtility.FromJson<CardList>(jsonFile.text);
            foreach (Card card in cardList.cards)
            {
                Debug.Log("Card ID: " + card.id);
                Debug.Log("Character Name: " + card.characterName);
                Debug.Log("Image Name: " + card.imageName);
                Debug.Log("Text: " + card.text);
                Debug.Log("Left Option: " + card.leftOption);
                Debug.Log("Right Option: " + card.rightOption);
            }
            return cardList;
        }
        else
        {
            Debug.LogError("Failed to load JSON file.");
        }

        return null;
    }

    private void Start()
    {
        // Load the JSON file

        CardList cardList = JsonReader();
        cardText.text = cardList.cards[0].text;
        characterNameText.text = cardList.cards[0].characterName; 
        
    }

    public void SetCard(DecisionCardData cardData)
    {
        characterNameText.text = cardData.characterName;
        cardText.text = cardData.text;
        leftOptionText.text = cardData.leftOption;
        rightOptionText.text = cardData.rightOption;
    }
}