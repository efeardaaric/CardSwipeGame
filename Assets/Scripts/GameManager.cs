using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Kart Sistemi")]
    public Transform cardSpawnPoint;
    public GameObject cardPrefab;

    [Header("Metrik Sliderlar")]
    public Slider economySlider;
    public Slider militarySlider;
    public Slider religionSlider;
    public Slider peopleSlider;

    [Header("Deðiþim Yazýlarý")]
    public TextMeshProUGUI economyChangeText;
    public TextMeshProUGUI militaryChangeText;
    public TextMeshProUGUI religionChangeText;
    public TextMeshProUGUI peopleChangeText;

    [Header("Oyun Sonu Paneli")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI deathReasonText;

    private List<DecisionCardData> allCards;
    private GameObject currentCard;

    void Start()
    {
        gameOverPanel.SetActive(false);

        bool cardsLoaded = LoadCards();
        if (!cardsLoaded)
        {
            Debug.LogError("Kartlar yüklenemedi. Lütfen 'cards.json' dosyasýný kontrol et.");
            return;
        }

        if (cardPrefab == null || cardSpawnPoint == null)
        {
            Debug.LogError("cardPrefab veya cardSpawnPoint referansý baðlanmamýþ.");
            return;
        }

        SpawnNewCard();
    }

    bool LoadCards()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("cards");
        if (jsonData == null)
        {
            Debug.LogError("Resources klasöründe 'cards.json' dosyasý bulunamadý.");
            return false;
        }

        string wrappedJson = "{\"cards\":" + jsonData.text + "}";
        try
        {
            allCards = JsonUtility.FromJson<CardListWrapper>(wrappedJson).cards;
        }
        catch
        {
            Debug.LogError("cards.json dosyasý hatalý veya yanlýþ formatta.");
            return false;
        }

        return allCards != null && allCards.Count > 0;
    }

    public void SpawnNewCard()
    {
        if (currentCard != null)
            Destroy(currentCard);

        if (allCards == null || allCards.Count == 0)
        {
            Debug.LogError("Kart listesi boþ. Oyun ilerleyemez.");
            return;
        }

        var randomCard = allCards[Random.Range(0, allCards.Count)];

        currentCard = Instantiate(cardPrefab, cardSpawnPoint.position, Quaternion.identity);
        currentCard.transform.SetParent(cardSpawnPoint, false);

        // CardUI kontrolü
        var cardUI = currentCard.GetComponent<CardUI>();
        if (cardUI == null)
        {
            Debug.LogError("CardUI script prefab'a eklenmemiþ.");
            return;
        }

        var cardSwipe = currentCard.GetComponent<CardSwipe>();
        if (cardSwipe == null)
        {
            Debug.LogError("CardSwipe script prefab'a eklenmemiþ.");
            return;
        }

        cardUI.SetCard(randomCard);
        cardSwipe.Init(randomCard, this);
    }

    public void OnCardSwiped(DecisionCardData data, bool swipedRight)
    {
        int eco = swipedRight ? data.rightEconomyEffect : data.leftEconomyEffect;
        int mil = swipedRight ? data.rightMilitaryEffect : data.leftMilitaryEffect;
        int rel = swipedRight ? data.rightReligionEffect : data.leftReligionEffect;
        int ppl = swipedRight ? data.rightPeopleEffect : data.leftPeopleEffect;

        UpdateMetric(economySlider, economyChangeText, eco);
        UpdateMetric(militarySlider, militaryChangeText, mil);
        UpdateMetric(religionSlider, religionChangeText, rel);
        UpdateMetric(peopleSlider, peopleChangeText, ppl);

        if (economySlider.value <= 0)
            EndGame("Hazine tükendi.");
        else if (militarySlider.value <= 0)
            EndGame("Ordu seni devirdi.");
        else if (religionSlider.value <= 0)
            EndGame("Ulema seni afaroz etti.");
        else if (peopleSlider.value <= 0)
            EndGame("Halk seni devirdi.");
        else
            SpawnNewCard();
    }

    void UpdateMetric(Slider slider, TextMeshProUGUI changeText, int changeAmount)
    {
        slider.value += changeAmount;
        ShowChangeText(changeText, changeAmount);
    }

    void ShowChangeText(TextMeshProUGUI text, int value)
    {
        text.text = value > 0 ? "+" + value.ToString() : value.ToString();
        text.color = value > 0 ? Color.green : Color.red;
        text.gameObject.SetActive(true);
        CancelInvoke(nameof(HideChangeTexts));
        Invoke(nameof(HideChangeTexts), 1.5f);
    }

    void HideChangeTexts()
    {
        economyChangeText.gameObject.SetActive(false);
        militaryChangeText.gameObject.SetActive(false);
        religionChangeText.gameObject.SetActive(false);
        peopleChangeText.gameObject.SetActive(false);
    }

    void EndGame(string reason)
    {
        gameOverPanel.SetActive(true);
        deathReasonText.text = reason;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [System.Serializable]
    public class CardListWrapper
    {
        public List<DecisionCardData> cards;
    }
}
