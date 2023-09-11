using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinScreen : UIScreen
{
    [Title("References")]
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _remainingCardsText;
    [SerializeField] TextMeshProUGUI _finalScoreText;

    public static Action<int> OnCalculateFinalScore = null;

    public void SetupScreen(int remainingCards)
    {
        var score = GameManager.Instance.PlayerScore;
        var finalScore = score * remainingCards;

        _scoreText.text = $"Score: {score}";
        _remainingCardsText.text = $"Remaining Cards: {remainingCards}";
        _finalScoreText.text = $"Final Score: {finalScore}";
        
        OnCalculateFinalScore?.Invoke(finalScore);
    }
}
