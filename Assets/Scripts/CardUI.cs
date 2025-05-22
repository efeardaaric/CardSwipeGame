using UnityEngine;
using TMPro;

public class CardUI : MonoBehaviour
{
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI cardText;
    public TextMeshProUGUI leftOptionText;
    public TextMeshProUGUI rightOptionText;

    public void SetCard(DecisionCardData cardData)
    {
        characterNameText.text = cardData.characterName;
        cardText.text = cardData.text;
        leftOptionText.text = cardData.leftOption;
        rightOptionText.text = cardData.rightOption;
    }
}
