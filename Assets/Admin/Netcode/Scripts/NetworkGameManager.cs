using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NetworkGameManager : NetworkBehaviour
{

    public static NetworkGameManager instance;

    public Vector2 currentCheckPoint;

    [SerializeField] private CanvasGroup joinCodeCanvas;
    [SerializeField] private TextMeshProUGUI joinCodeText;

    [SerializeField] private CanvasGroup beforeGameUI;

    [SerializeField] private CanvasGroup typeCodeUI;
    [SerializeField] private TMP_InputField enteredCode;

    [SerializeField] private CanvasGroup inGameUI;

    [SerializeField] private CanvasGroup raceOptionsUI;
    [SerializeField] private CanvasGroup racePrompt;

    public bool RaceStart;

    public bool inGame;

    private async void Start()
    {
        instance = this;

        RaceStart = false;

        await UnityServices.InitializeAsync();

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        inGame = false;

        joinCodeCanvas.alpha = 0;
        beforeGameUI.alpha = 1;
        typeCodeUI.alpha = 0;
    }

    public async void createOnClick()
    {
        string joinCode = await StartHostWithRelay(5);

        joinCodeText.text = joinCode;
        beforeGameUI.alpha = 0;
        beforeGameUI.blocksRaycasts = false;
        beforeGameUI.interactable = false;
        joinCodeCanvas.alpha = 1;
        inGame = true;
        inGameUI.alpha = 1;
        inGameUI.blocksRaycasts = true;
        inGameUI.interactable = true;
        racePrompt.alpha = 1;
    }

    public void enterCodeOnClick()
    {
        enteredCode.text = "";
        typeCodeUI.alpha = 1;
        typeCodeUI.blocksRaycasts = true;
    }

    public async void joinOnClick()
    {
        Debug.Log(enteredCode.text);
        bool joined = await StartClientWithRelay(enteredCode.text);

        if (joined)
        {
            beforeGameUI.alpha = 0;
            beforeGameUI.blocksRaycasts = false;
            beforeGameUI.interactable = false;
            typeCodeUI.alpha = 0;
            typeCodeUI.blocksRaycasts = false;
            joinCodeText.text = enteredCode.text;
            joinCodeCanvas.alpha = 1;
            inGame = true;
            inGameUI.alpha = 1;
            inGameUI.blocksRaycasts = true;
            inGameUI.interactable = true;
        }

    }

    public void closeEnterCode()
    {
        typeCodeUI.alpha = 0;
        typeCodeUI.blocksRaycasts = false;
        typeCodeUI.interactable = false;
    }

    public async Task<string> StartHostWithRelay(int maxConnections)
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        return NetworkManager.Singleton.StartHost() ? joinCode : null;
    }

    public async Task<bool> StartClientWithRelay(string joinCode)
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode: joinCode);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
        return !string.IsNullOrEmpty(joinCode) && NetworkManager.Singleton.StartClient();
    }

    private void Update()
    {
        if (NetworkManager.ShutdownInProgress)
        {
            toMainMenu();
        }
        if (IsHost)
        {
            if (Input.GetKeyDown(KeyCode.F) && !RaceStart)
            {
                RaceStart = true;
                raceOptionsUI.alpha = 1;
                raceOptionsUI.blocksRaycasts = true;
                raceOptionsUI.interactable = true;
            }
        }

    }

    public void leaveGame()
    {
        NetworkManager.Shutdown();

        toMainMenu();
    }

    public void toMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
