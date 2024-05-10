using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup pauseMenu;
    [SerializeField] private CanvasGroup gameUI;
    [SerializeField] private CanvasGroup optionsMenu;

    [SerializeField] private GameObject player; 

    private bool isPaused;

    private void Start()
    {
        isPaused = false;
    }

    private void Update()
    {
        if (!isPaused && LevelManager.instance.levelStart && Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
            isPaused = true;
        }else if (isPaused && LevelManager.instance.levelStart && Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
            isPaused = false;
        }
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        pauseMenu.alpha = 1f;
        pauseMenu.blocksRaycasts = true;
        pauseMenu.interactable = true;
        gameUI.alpha = 0f;
        player.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.alpha = 0f;
        pauseMenu.blocksRaycasts = false;
        pauseMenu.interactable = false;
        gameUI.alpha = 1f;
        player.SetActive(true);
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
