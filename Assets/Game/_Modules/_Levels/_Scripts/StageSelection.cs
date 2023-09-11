using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelection : MonoBehaviour
{
    [SerializeField] LevelDataSO _levelData;
    [SerializeField] TextMeshProUGUI _stageTitle;
    [SerializeField] Image _stageImage;
    [SerializeField] TextMeshProUGUI _stageHighScore;
    [SerializeField] string _sceneName;

    // Start is called before the first frame update
    void Start()
    {
        _stageImage.sprite = _levelData.levelSprite;

        _stageTitle.text = _levelData.levelName;

        SetStageHighscore();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(_sceneName);
    }

    void SetStageHighscore()
    {
        var stageScore = PlayerPrefs.GetInt(_levelData.levelName, 0);
        _stageHighScore.text = $"Best Score: {stageScore}";
    }
}
