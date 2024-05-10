using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class RaceManager : NetworkBehaviour
{
    public static RaceManager instance;

    public Vector2 currentCheckPoint;

    [SerializeField] private CanvasGroup[] countdownNumers;

    [SerializeField] private CanvasGroup endGameUI;
    [SerializeField] private TextMeshProUGUI endMessage;
    
    [SerializeField] private CanvasGroup raceUI;
    [SerializeField] private CanvasGroup raceOptionsUI;
    [SerializeField] private CanvasGroup racePrompt;

    [SerializeField] private Slider Laps;
    [SerializeField] private Slider timeAfterFinish;

    [SerializeField] private ProgressionFlagNetwork[] lapProgress;

    public int lapsLeft;
    private NetworkVariable<int> maxLaps = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<float> AfterFirstFinish = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] private TextMeshProUGUI currentTimeUI;
    [SerializeField] private TextMeshProUGUI lapsUI;

    public float levelTimer;

    public NetworkVariable<bool> raceStart = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public bool localRaceStart = true;
    public bool levelStart;

    public bool playerEnd;
    public NetworkVariable<bool> levelEnd = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private bool localLevelEnd;

    public CheckPointFlagNetwork endFlag;

    private bool winner;

    void Start()
    {
        instance = this;
        levelStart = false;
        levelTimer = 0f;
        winner = false;
        
        playerEnd = false;
    }

    public override void OnNetworkSpawn()
    {
        levelEnd.Value = false;
        raceStart.Value = false;
        maxLaps.Value = 0;
        AfterFirstFinish.Value = 0;
    }

    private IEnumerator levelCountdown()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1);
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

    public void RaceStarting()
    {
        raceUI.alpha = 1;
        levelStart = true;
        localRaceStart = false;
        localLevelEnd = true;

        if (!IsHost)
        {
            lapsLeft = maxLaps.Value;
        }

        levelTimer = 0f;
        winner = false;
        playerEnd = false;

        newLapReset();
        StartCoroutine(levelCountdown());
    }

    public void RaceStop()
    {
        raceUI.alpha = 0;
        endGameUI.alpha = 0;
        endGameUI.interactable = false;
        endGameUI.blocksRaycasts = false;
        localLevelEnd = false;
        NetworkGameManager.instance.RaceStart = false;
        if (IsHost)
        {
            racePrompt.alpha = 1f;
        }
        localRaceStart = true;

    }

    public void setRaceSetting()
    {
        lapsLeft = (int)Laps.value;
        maxLaps.Value = lapsLeft;

        AfterFirstFinish.Value = timeAfterFinish.value;

        raceStart.Value = true;
        raceOptionsUI.alpha = 0;
        raceOptionsUI.blocksRaycasts = false;
        raceOptionsUI.interactable = false;
        racePrompt.alpha = 0f;
    }

    void Update()
    {
        if (levelStart)
        {
            levelTimer += Time.deltaTime;
            currentTimeUI.text = convertToTime(levelTimer);
        }

        if (levelEnd.Value && localLevelEnd)
        {
            endLevel();
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

    public void newLap()
    {
        if (lapCheck())
        {
            if (lapsLeft == 1)
            {
                if (IsHost)
                {
                    levelEnd.Value = true;
                }
                else
                {
                    winRaceServerRpc();
                }
                winner = true;
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
        foreach (ProgressionFlagNetwork p in lapProgress)
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
        lapsUI.text = "Laps: " + (maxLaps.Value - lapsLeft) + "/" + maxLaps.Value;
        foreach (ProgressionFlagNetwork p in lapProgress)
        {
            p.IsPassed = false;
        }
    }

    private void endLevel()
    {
        levelStart = false;
        endGameUI.alpha = 1;
        endGameUI.interactable = true;
        endGameUI.blocksRaycasts = true;

        if (IsHost)
        {
            raceStart.Value = false;
            levelEnd.Value = false;
        }
        else
        {
            resetValuesServerRpc();
        }

        if (winner)
        {
            endMessage.text = "Winner";
        }
        else
        {
            endMessage.text = "You Lose";
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void winRaceServerRpc()
    {
        levelEnd.Value = true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void resetValuesServerRpc()
    {
        raceStart.Value = false;
        levelEnd.Value = false;
    }

}
