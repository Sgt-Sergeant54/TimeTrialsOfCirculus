using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TimeTrialsUI : MonoBehaviour
{
    public static TimeTrialsUI instance;

    [SerializeField] private CanvasGroup playGame;
    [SerializeField] private CanvasGroup highscoresTable;

    [SerializeField] private Image[] levelHighlights;

    [SerializeField] private Transform container;
    [SerializeField] private Transform highscoreSingleTemplate;

    public int selectedLevel;

    private void Start()
    {
        instance = this;
        selectedLevel = 0;

        playGame.alpha = 0;
        playGame.blocksRaycasts = false;
        playGame.interactable = false;

        highscoresTable.alpha = 0;
    }

    public void selectLevel(int level)
    {
        if (selectedLevel == level) return;
        if (selectedLevel != 0)
        {
            levelHighlights[selectedLevel - 1].enabled = false;
        }
        selectedLevel = level;
        if (highscoresTable.alpha == 0 )
        { 
            highscoresTable.alpha = 1;
        }

        levelHighlights[selectedLevel - 1].enabled = true;

        if (selectedLevel <= GameDataManager.Instance.gameData.currentLevel && selectedLevel < 3)
        {
            playGame.alpha = 1;
            playGame.blocksRaycasts = true;
            playGame.interactable = true;
        }
        else
        {
            playGame.alpha = 0;
            playGame.blocksRaycasts = false;
            playGame.interactable = false;
        }

        setTableData();
    }

    private void setTableData()
    {
        float[] highscores = getLevelScores();

        foreach (Transform x in container)
        {
            if (x == highscoreSingleTemplate) continue;
            Destroy(x.gameObject);
        }

        foreach (float f in highscores)
        {
            if (f != 0)
            {
                Transform highscoreSingleTransform = Instantiate(highscoreSingleTemplate, container);
                highscoreSingleTransform.gameObject.SetActive(true);
                highscoreSingleUI highscoreSingle = highscoreSingleTransform.GetComponent<highscoreSingleUI>();
                highscoreSingle.UpdateHighscores(floatToTime(f));
            }
        }
    }

    private float[] getLevelScores()
    {

        GameData data = GameDataManager.Instance.gameData;
        switch (selectedLevel)
        {
            case 1:
                return data.levelOneHighscores;

            case 2:
                return data.levelTwoHighscores;

            case 3:
                return data.levelThreeHighscores;

            case 4:
                return data.levelFourHighscores;

            case 5:
                return data.levelFiveHighscores;

            case 6:
                return data.levelSixHighscores;

            case 7:
                return data.levelSevenHighscores;

            case 8:
                return data.levelEightHighscores;

            case 9:
                return data.levelNineHighscores;

            case 10:
                return data.levelTenHighscores;

            case 11:
                return data.levelElevenHighscores;

            case 12:
                return data.levelTwelveHighscores;

            default:
                return null;

        }
    }

    private string floatToTime(float time)
    {
        float minute = 60;
        int minutes = (int)(time / minute);
        int seconds = (int)(time % minute);
        string timeString = "";
        if (minutes != 0)
        {
            timeString += minutes + " minutes ";
            if (seconds != 0)
            {
                timeString += " and ";
            }
        }

        if (seconds != 0)
        {
            timeString += seconds + " seconds";
        }
        return timeString;
    }

    public void closeTable()
    {
        if (selectedLevel != 0)
        {
            levelHighlights[selectedLevel - 1].enabled = false;
        }

        selectedLevel = 0;
        playGame.alpha = 0;
        playGame.blocksRaycasts = false;
        playGame.interactable = false;
        highscoresTable.alpha = 0f;
    }
}
