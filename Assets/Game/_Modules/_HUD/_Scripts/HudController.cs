using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HudController : MonoBehaviour
{
    [SerializeField] GameObject _hudParent;
    [SerializeField] TextMeshProUGUI _citizensQuantity;
    [SerializeField] TextMeshProUGUI _playerScore;

    public static Action OnHowToPlayButtonPressed = null;

    private void OnEnable()
    {
        LevelController.OnCitizensQuantityChanged += UpdateCitizensQuantity;
        GameManager.OnPlayerScoreChanged += UpdatePlayerScore;
        HowToPlayScreen.OnHowToPlayOpened += DisableHUD;
        HowToPlayScreen.OnHowToPlayClosed += EnableHUD;
    }

    private void OnDisable()
    {
        LevelController.OnCitizensQuantityChanged -= UpdateCitizensQuantity;
        GameManager.OnPlayerScoreChanged -= UpdatePlayerScore;
        HowToPlayScreen.OnHowToPlayOpened -= DisableHUD;
        HowToPlayScreen.OnHowToPlayClosed -= EnableHUD;
    }

    void UpdateCitizensQuantity(int quantity)
    {
        _citizensQuantity.text = quantity.ToString();
    }

    void UpdatePlayerScore(int value)
    {
        _playerScore.text = value.ToString();
    }

    public void HowToPlayButton()
    {
        OnHowToPlayButtonPressed?.Invoke();
    }

    void EnableHUD()
    {
        _hudParent.SetActive(true);
    }

    void DisableHUD()
    {
        _hudParent.SetActive(false);
    }
}
