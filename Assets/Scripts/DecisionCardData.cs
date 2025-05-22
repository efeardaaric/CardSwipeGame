[System.Serializable]
public class DecisionCardData
{
    public string id;
    public string characterName;
    public string imageName; // Karakter portresi için
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
    public string[] tags; // Butterfly sistem için
}
