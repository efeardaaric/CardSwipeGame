using System;
using System.Collections.Generic;

[Serializable]
public class CardData
{
    public string text;
    public string leftOption;
    public string rightOption;
}

[Serializable]
public class CardDataList
{
    public List<CardData> cards;
}
//SAL