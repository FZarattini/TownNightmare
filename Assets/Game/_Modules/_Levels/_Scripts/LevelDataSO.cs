using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/New Level", order = 1)]
public class LevelDataSO : ScriptableObject
{
    public string levelName;
    public Sprite levelSprite;

    public int deckSize;
    public int playerHandSize;
    public float defencesModifier;
    public int totalCitizens;

    public float citizensResponseOdds;
}
