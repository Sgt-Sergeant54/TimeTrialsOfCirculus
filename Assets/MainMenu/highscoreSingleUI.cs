using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class highscoreSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI time;

    public void UpdateHighscores(string highscore)
    {
        time.text = highscore;
    }
}
