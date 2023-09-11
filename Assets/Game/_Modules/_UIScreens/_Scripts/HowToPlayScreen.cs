using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayScreen : MonoBehaviour
{
    [SerializeField] GameObject howToPlayObject;

    public static Action OnHowToPlayOpened = null;
    public static Action OnHowToPlayClosed = null;

    private void OnEnable()
    {
        HudController.OnHowToPlayButtonPressed += OpenHowToPlayScreen;        
    }

    private void OnDisable()
    {
        HudController.OnHowToPlayButtonPressed -= OpenHowToPlayScreen;        
    }

    void OpenHowToPlayScreen()
    {
        howToPlayObject.SetActive(true);
        OnHowToPlayOpened?.Invoke();
    }

    public void CloseHowToPlayScreen()
    {
        howToPlayObject.SetActive(false);
        OnHowToPlayClosed?.Invoke();
    }
}
