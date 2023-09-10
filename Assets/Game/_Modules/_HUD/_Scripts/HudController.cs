using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HudController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _citizensQuantity;
    [SerializeField] TextMeshProUGUI _playerScore;

    private void OnEnable()
    {
        LevelController.OnCitizensQuantityChanged += UpdateCitizensQuantity;
        GameManager.OnPlayerScoreChanged += UpdatePlayerScore;
    }

    private void OnDisable()
    {
        LevelController.OnCitizensQuantityChanged -= UpdateCitizensQuantity;
        GameManager.OnPlayerScoreChanged -= UpdatePlayerScore;
    }

    void UpdateCitizensQuantity(int quantity)
    {
        _citizensQuantity.text = quantity.ToString();
    }

    void UpdatePlayerScore(int value)
    {
        _playerScore.text = value.ToString();
    }
}
