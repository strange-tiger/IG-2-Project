using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using PlayerInfo = ShootingGameManager.ShootingPlayerInfo;
using SceneNumber = Defines.ESceneNumber;

public class EndResultPanel : MonoBehaviourPun
{
    [SerializeField] private LobbyChanger _lobbyChanger;

    [Header("UI")]
    [SerializeField] private GameObject _goldPanel;
    [SerializeField] private GameObject _starPanel;
    [SerializeField] private GameObject _regamePanel;
    private CheckPanelManager _regameCheckScript;

    private TextMeshProUGUI[] _playerNicknameTexts =
        new TextMeshProUGUI[ShootingGameManager._MAX_PLAYER_COUNT];
    
    private Image[] _playerColors =
        new Image[ShootingGameManager._MAX_PLAYER_COUNT];
    
    private TextMeshProUGUI[] _playerScoreTexts =
        new TextMeshProUGUI[ShootingGameManager._MAX_PLAYER_COUNT];

    private TextMeshProUGUI[] _playerGoldTexts;

    private Image[] _goldStars;

    private RoomOptions _waitingRoomOption = new RoomOptions
    {
        MaxPlayers = ShootingGameManager._MAX_PLAYER_COUNT,
        CleanupCacheOnLeave = true,
        PublishUserId = true,
        CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { ShootingServerManager.RoomPropertyKey, 1 } },
        CustomRoomPropertiesForLobby = new string[]
                {
                    ShootingServerManager.RoomPropertyKey,
                },
        IsVisible = true,
        IsOpen = true,
    };

    private void Awake()
    {
        _regamePanel.SetActive(false);
        _regameCheckScript = _regamePanel.GetComponent<CheckPanelManager>();

        for (int i = 0; i < ShootingGameManager._MAX_PLAYER_COUNT; ++i)
        {
            GameObject nextChild = transform.GetChild(i).gameObject;
            _playerNicknameTexts[i] = nextChild.GetComponentInChildren<TextMeshProUGUI>();
            _playerColors[i] = nextChild.GetComponent<Image>();
            _playerScoreTexts[i] = nextChild.GetComponentsInChildren<Image>()[1].GetComponentInChildren<TextMeshProUGUI>();
        }

        _playerGoldTexts = _goldPanel.GetComponentsInChildren<TextMeshProUGUI>();
        _goldPanel.SetActive(false);

        _goldStars = _starPanel.GetComponentsInChildren<Image>();
        _starPanel.SetActive(false);
    }

    public void SetPlayerList(List<PlayerInfo> playerInfos)
    {
        int playerInfoCount = playerInfos.Count;
        for (int i = 0; i < playerInfoCount; ++i)
        {
            PlayerInfo info = playerInfos[i];

            _playerNicknameTexts[i].text = info.PlayerNickname;
            _playerColors[i].color = info.PlayerColor;
            _playerScoreTexts[i].text = info.PlayerScore.ToString();
            _playerGoldTexts[i].text = "+ " + info.PlayerGold.ToString();
            _goldStars[i].gameObject.SetActive(info.IsWinner);
        }
    }

    public void ShowStarImage()
    {
        _starPanel.SetActive(true);
    }

    public void ShowGoldPanel()
    {
        _goldPanel.SetActive(true);
    }

    public void ShowRestartPanel()
    {
        if (PhotonNetwork.IsMessageQueueRunning)
        {
            PhotonNetwork.RemoveBufferedRPCs();
        }
        _regamePanel.SetActive(true);
        _regameCheckScript.ShowCheckPanel("REGAME?",
            () =>
            {
                Debug.Log("[Shooting] 게임 재시작");
                _lobbyChanger.ChangeLobby(SceneNumber.ShootingWaitingRoom, _waitingRoomOption, true,
                    _waitingRoomOption.CustomRoomProperties, (byte)_waitingRoomOption.MaxPlayers);
            },
            () =>
            {
                Debug.Log("[Shooting] 게임 종료");
                _lobbyChanger.ChangeLobby(SceneNumber.WesternLobby);
            });
    }
}
