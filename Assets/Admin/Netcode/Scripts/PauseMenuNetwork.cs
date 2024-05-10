using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenuNetwork : NetworkBehaviour
{
    [SerializeField] private CanvasGroup pauseMenu;
    [SerializeField] private CanvasGroup gameUI;
    [SerializeField] private CanvasGroup optionsMenu;

    private bool isPaused;

    private void Start()
    {
        isPaused = false;
        
    }

    private void Update()
    { 

        if (!isPaused && NetworkGameManager.instance.inGame && Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
            isPaused = true;
        }else if (isPaused && NetworkGameManager.instance.inGame && Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
            isPaused = false;
        }
    }

    private void Pause()
    {
        pauseMenu.alpha = 1f;
        pauseMenu.blocksRaycasts = true;
        pauseMenu.interactable = true;
        gameUI.alpha = 0f;
    }

    public void Resume()
    {
        pauseMenu.alpha = 0f;
        pauseMenu.blocksRaycasts = false;
        pauseMenu.interactable = false;
        gameUI.alpha = 1f;
    }

    public void Options()
    {
        pauseMenu.alpha = 0f;
        pauseMenu.blocksRaycasts = false;
        pauseMenu.interactable = false;
        optionsMenu.alpha = 1f;
        optionsMenu.blocksRaycasts = true;
        optionsMenu.interactable = true;
    }

    public void closeOptions()
    {
        pauseMenu.alpha = 1f;
        pauseMenu.blocksRaycasts = true;
        pauseMenu.interactable = true;
        optionsMenu.alpha = 0f;
        optionsMenu.blocksRaycasts = false;
        optionsMenu.interactable = false;
    }
}
