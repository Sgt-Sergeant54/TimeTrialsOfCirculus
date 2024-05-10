using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    [SerializeField] private CanvasGroup startMenuCanvasGroup;
    [SerializeField] private CanvasGroup optionsCanvasGroup;
    [SerializeField] private CanvasGroup timeTrialsCanvasGroup;

    private void Start()
    {
        instance = this;

        startMenuCanvasGroup.alpha = 1;
        optionsCanvasGroup.alpha = 0;
        timeTrialsCanvasGroup.alpha = 0;
    }

    public void backToStartMenu()
    {
        startMenuCanvasGroup.alpha = 1;
        startMenuCanvasGroup.blocksRaycasts = true;
        startMenuCanvasGroup.interactable = true;
    }

    public void closeStartMenu()
    {
        startMenuCanvasGroup.alpha = 0;
        startMenuCanvasGroup.interactable = false;
        startMenuCanvasGroup.blocksRaycasts = false;
    }

    public void closeTimeTrials()
    {
        timeTrialsCanvasGroup.alpha = 0;
        timeTrialsCanvasGroup.blocksRaycasts = false;
        timeTrialsCanvasGroup.interactable = false;
    }

    public void closeOptions()
    {
        optionsCanvasGroup.alpha = 0;
        optionsCanvasGroup.interactable = false;
        optionsCanvasGroup.blocksRaycasts = false;
    }

    public void TutorialOnClick()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void TimeTrialOnClick()
    {
        timeTrialsCanvasGroup.alpha = 1;
        timeTrialsCanvasGroup.blocksRaycasts = true;
        timeTrialsCanvasGroup.interactable = true;
    }

    public void MultiplayerOnClick()
    {
        SceneManager.LoadScene("MultiplayerLobby");
    }

    public void OptionsOnClick()
    {
        optionsCanvasGroup.alpha = 1;
        optionsCanvasGroup.blocksRaycasts = true;
        optionsCanvasGroup.interactable = true;
    }

    public void QuitOnClick()
    {
       Application.Quit();
    }

    public void StartLevel()
    {
        SceneManager.LoadScene(TimeTrialsUI.instance.selectedLevel);
    }

}
