using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/New Card", order = 1)]
public class CardSO : ScriptableObject
{
    [Title("Card Attributes")]
    public string cardName;
    public string cardDescription;
    public CardType cardType;

    [Title("Outcomes")]
    public float positiveOutcomeProbability;
    public string positiveOutcomeDescription;
    public string negativeOutcomeDescription;

    [Title("Effects")]
    public List<CardEffects> cardEffects;
    public int MaxCitizenCasualty;


    public OutcomeType RollOutCome(float defencesModifier)
    {
        float randomOutcome = UnityEngine.Random.Range(0, 1f);

        randomOutcome += defencesModifier;

        if (randomOutcome <= positiveOutcomeProbability)
            return OutcomeType.Positive;
        else
            return OutcomeType.Negative;
    }

    public int RollCitizenCasualty(OutcomeType outcome)
    {
        if (outcome == OutcomeType.Positive)
            return UnityEngine.Random.Range(1, MaxCitizenCasualty + 1);
        else
            return 0;
    }
}

[Serializable]
public class PlayOutcome
{
    public OutcomeType outcome;
    public string result;
}

public enum CardType
{
    Air,
    Land,
    NaturalDisaster,
    Magic,    
    Resource,
}

public enum OutcomeType
{
    Positive,
    Negative,
}

