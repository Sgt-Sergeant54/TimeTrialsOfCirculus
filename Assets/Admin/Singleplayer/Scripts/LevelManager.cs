using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Vector2 currentCheckPoint;

    [SerializeField] private CanvasGroup[] countdownNumers;

    [SerializeField] private CanvasGroup endGameUI;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI winMessage;
    [SerializeField] private CanvasGroup nextLevelButton;

    [SerializeField] private ProgressionFlag[] lapProgress;

    public int lapsLeft;
    private int maxLaps;

    [SerializeField] private TextMeshProUGUI currentTimeUI;
    [SerializeField] private TextMeshProUGUI lapsUI;

    public float levelTimer;
    public bool levelStart;

    public bool levelEnd;

    public checkPointFlag endFlag;

    public int level;

    [SerializeField] private float maximumWinTime;

    private void Start()
    {
        instance = this;
        levelStart = false;
        levelTimer = 0f;
        levelEnd = false;
        maxLaps = lapsLeft;

        newLapReset();

        StartCoroutine(levelCountdown());
    }

    private IEnumerator levelCountdown()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(4);
        countdownNumers[3].alpha = 0;
        countdownNumers[0].alpha = 1;
        levelAudioManager.Instance.playCountdown(0);
        yield return new WaitForSecondsRealtime(1);
        countdownNumers[0].alpha = 0;
        countdownNumers[1].alpha = 1;
        levelAudioManager.Instance.playCountdown(1);
        yield return new WaitForSecondsRealtime(1);
        countdownNumers[1].alpha = 0;
        countdownNumers[2].alpha = 1;
        levelAudioManager.Instance.playCountdown(2);
        yield return new WaitForSecondsRealtime(1);
        countdownNumers[2].alpha = 0;
        levelAudioManager.Instance.playCountdown(3);

        Time.timeScale = 1f;
        levelStart = true;
    }

    private void Update()
    {
        if (levelStart)
        {
            levelTimer += Time.deltaTime;
            currentTimeUI.text = convertToTime(levelTimer);
        }
    }

    public void newLap()
    {
        if (lapCheck())
        {
            if (lapsLeft == 1)
            {
                levelStart = false;
                levelEnd = true;
                endLevel();
            }
            else
            {
                lapsLeft -= 1;
                newLapReset();
            }
        }
    }

    private bool lapCheck()
    {
        foreach (ProgressionFlag p in lapProgress)
        {
            if (p.IsPassed != true)
            {
                return false;
            }
        }
        return true;
    }

    public void newLapReset()
    {
        lapsUI.text = "Laps: " + (maxLaps - lapsLeft) + "/" + maxLaps;
        foreach (ProgressionFlag p in lapProgress)
        {
            p.IsPassed = false;
        }
    }

    private string convertToTime(float timer)
    {
        float minute = 60;
        int minutes = (int)(timer / minute);
        int seconds = (int)(timer % minute);
        string timeString = "";

        timeString += minutes + ":" + seconds;

        return timeString;
    }

    private void endLevel()
    {
        Time.timeScale = 0f;
        endGameUI.alpha = 1;
        endGameUI.blocksRaycasts = true;
        endGameUI.interactable = true;

        

        if (levelTimer <= maximumWinTime)
        {
            winMessage.text = "Level Completed";
            nextLevelButton.alpha = 1;
            nextLevelButton.interactable = true;
            nextLevelButton.blocksRaycasts = true;
            if (GameDataManager.Instance != null)
            {
                GameDataManager.Instance.levelComplete(levelTimer, true);
            }
        }
        else
        {
            winMessage.text = "Not Quick Enough";
            if (GameDataManager.Instance != null)
            {
                GameDataManager.Instance.levelComplete(levelTimer, false);
            }
        }
        try
        {
            scoreText.text = convertToTime(levelTimer) + " / " + convertToTime(maximumWinTime);
        }catch
        {

        }
    }

    public void GoToScene(int Scene)
    {
        SceneManager.LoadScene(Scene);
    }
}
