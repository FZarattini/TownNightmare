using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenController : MonoBehaviour
{
    [SerializeField] GameObject choicesParent;
    [SerializeField] GameObject stageSelectionParent;

    public void SelectStageButton()
    {
        choicesParent.SetActive(false);
        stageSelectionParent.SetActive(true);  
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
