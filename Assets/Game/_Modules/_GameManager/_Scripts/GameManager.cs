using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] int playerScore;
    [SerializeField] bool onDialogue;

    public static Action<int> OnPlayerScoreChanged = null;

    public bool OnDialogue
    {
        get => onDialogue;
        set => onDialogue = value;
    }

    private void OnEnable()
    {
        LevelController.OnLevelStart += ResetScore;
    }

    private void OnDisable()
    {
        LevelController.OnLevelStart -= ResetScore;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void AddPlayerScore(int value)
    {
        playerScore += value;

        OnPlayerScoreChanged?.Invoke(playerScore);
    }

    void ResetScore()
    {
        playerScore = 0;

        OnPlayerScoreChanged?.Invoke(playerScore);
    }

    public void SaveScore(string key)
    {
        if(PlayerPrefs.GetInt(key, 0) < playerScore)
            PlayerPrefs.SetInt(key, playerScore);
    }
}
