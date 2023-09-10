using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectsData", menuName = "ScriptableObjects/New Effects Data", order = 1)]
public class EffectsDataSO : ScriptableObject
{
    [Title("Casualties")]
    public int hungerCasualtiesPerRound;
    public int thirstCasualtiesPerRound;
    public int sicknessCasualtiesPerRound;

    [Title("End Messages")]
    public string hungerEndedMessage;
    public string thirstEndedMessage;
    public string sicknessEndedMessage;

    [Title("Recovery Messages")]
    public string hungerRecoveryMessage;
    public string thirstRecoveryMessage;
    public string sicknessRecoveryMessage;

    [Title("Rounds Data")]
    public int hungerMaxRounds;
    public int thirstMaxRounds;
    public int sicknessMaxRounds;


    [Title("Recovery Data")]
    public float effectRecoveryChance;


    public string GetAlreadyHungerMessage()
    {
        return $"The citizens are already starving!";
    }

    public string GetHungerMessage()
    {
        return $"The citizens are now starving. {hungerCasualtiesPerRound} citizens will die every turn for {hungerMaxRounds} turns unless they find a way to recover!";
    }

    public string GetAlreadyThirstMessage()
    {
        return $"The citizens area already parched!";
    }

    public string GetThirstMessage()
    {
        return $"The citizens are now parched. {thirstCasualtiesPerRound} citizens will die every turn for {thirstMaxRounds} turns unless they find a way to recover!";
    }

    public string GetAlreadySickMessage()
    {
        return $"The citizens are already sick.";
    }

    public string GetSickMessage()
    {
        return $"The citizens are now sick. {sicknessCasualtiesPerRound} citizens will die every turn for {sicknessMaxRounds} turns unless they find a way to recover!";
    }
}

public enum CardEffects
{
    Hunger,
    Thirst,
    Sickness,
}
