using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    public GameData gameData;
    private string path;

    private void Start()
    {
        Instance = this;

        path = Application.persistentDataPath + "/levelData.json";

        loadData();
    }

    private void loadData()
    {
        if (File.Exists(path))
        {
            string data = File.ReadAllText(path);
            gameData = JsonUtility.FromJson<GameData>(data);
        }
        else
        {
            newData();
        }
    }

    public void saveData()
    {
        string saveData = JsonUtility.ToJson(gameData);
        Debug.Log(saveData);
        File.WriteAllText(path, saveData);
    }

    public void newData()
    {
        gameData = new GameData();
        gameData.currentLevel = 1;
        gameData.levelOneHighscores = new float[10];
        gameData.levelTwoHighscores = new float[10];
        gameData.levelThreeHighscores = new float[10];
        gameData.levelFourHighscores = new float[10];
        gameData.levelFiveHighscores = new float[10];
        gameData.levelSixHighscores = new float[10];
        gameData.levelSevenHighscores = new float[10];
        gameData.levelEightHighscores = new float[10];
        gameData.levelNineHighscores = new float[10];
        gameData.levelTenHighscores = new float[10];
        gameData.levelElevenHighscores = new float[10];
        gameData.levelTwelveHighscores = new float[10];

        saveData();
    }

    private float[] newHighscoreValues(float[] current, float time)
    {
        List<float> newScores = current.ToList<float>();
        newScores.Add(time);
        newScores.RemoveAll(x => x == 0);
        newScores.Sort();
        Debug.Log(newScores.ToString());
        if (newScores.Count > 10)
        {
            newScores.Remove(newScores.ElementAt(10));
        }
        return newScores.ToArray();
    }

    public void levelComplete(float time, bool completed)
    {
        int level = LevelManager.instance.level;
        switch (level)
        {
            case 1:

                gameData.levelOneHighscores = newHighscoreValues(gameData.levelOneHighscores, time);
                break;
            case 2:

                gameData.levelTwoHighscores = newHighscoreValues(gameData.levelTwoHighscores, time);
                break;
            case 3:

                gameData.levelThreeHighscores = newHighscoreValues(gameData.levelThreeHighscores, time);
                break;
            case 4:

                gameData.levelFourHighscores = newHighscoreValues(gameData.levelFourHighscores, time);
                break;
            case 5:

                gameData.levelFiveHighscores = newHighscoreValues(gameData.levelFiveHighscores, time);
                break;
            case 6:

                gameData.levelSixHighscores = newHighscoreValues(gameData.levelSixHighscores, time);
                break;
            case 7:

                gameData.levelSevenHighscores = newHighscoreValues(gameData.levelSevenHighscores, time);
                break;
            case 8:

                gameData.levelEightHighscores = newHighscoreValues(gameData.levelEightHighscores, time);
                break;
            case 9:

                gameData.levelNineHighscores = newHighscoreValues(gameData.levelNineHighscores, time); ;
                break;
            case 10:

                gameData.levelTenHighscores = newHighscoreValues(gameData.levelTenHighscores, time); ;
                break;
            case 11:

                gameData.levelElevenHighscores = newHighscoreValues(gameData.levelElevenHighscores, time); ;
                break;
            case 12:

                gameData.levelTwelveHighscores = newHighscoreValues(gameData.levelTwelveHighscores, time); ;
                break;
        }

        if (completed && gameData.currentLevel == LevelManager.instance.level)
        {
            gameData.currentLevel += 1;
        }

        saveData();
    }
}

