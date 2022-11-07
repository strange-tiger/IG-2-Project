using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayerInfo = ShootingGameManager.ShootingPlayerInfo;

public class EndResultPanel : MonoBehaviour
{
    [SerializeField] private GameObject _goldPanel;
    [SerializeField] private GameObject _starImage;
    [SerializeField] private GameObject _regamePanel;
    private CheckPanelManager _regameCheckScript;

    private TextMeshProUGUI[] _playerNicknameText =
        new TextMeshProUGUI[ShootingGameManager._MAX_PLAYER_COUNT];
    private Image[] _playerColor =
        new Image[ShootingGameManager._MAX_PLAYER_COUNT];
    private TextMeshProUGUI[] _playerScoreText =
        new TextMeshProUGUI[ShootingGameManager._MAX_PLAYER_COUNT];
    private GameObject[] _isYouImages =
        new GameObject[ShootingGameManager._MAX_PLAYER_COUNT];

    private void Awake()
    {
        _goldPanel.SetActive(false);
        _starImage.SetActive(false);
        _regamePanel.SetActive(false);
        _regameCheckScript = _regamePanel.GetComponent<CheckPanelManager>();

        for (int i = 0; i < ShootingGameManager._MAX_PLAYER_COUNT; ++i)
        {
            GameObject nextChild = transform.GetChild(i).gameObject;
            _playerNicknameText[i] = nextChild.GetComponentInChildren<TextMeshProUGUI>();
            _playerColor[i] = nextChild.GetComponent<Image>();
            _playerScoreText[i] = _playerColor[i].GetComponentInChildren<Image>().GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void SetPlayerList(List<PlayerInfo> playerInfos)
    {
        for(int i = 0; i<playerInfos.Count; ++i)
        {
            PlayerInfo info = playerInfos[i];

            _playerNicknameText[i].text = info.PlayerNickname;
            _playerColor[i].color = info.PlayerColor;
            _playerScoreText[i].text = info.PlayerScore.ToString();
        }
    }

    public void ShowStarImage()
    {
        _starImage.SetActive(true);
    }

    public void ShowGoldPanel()
    {
        _goldPanel.SetActive(true);
    }

    public void ShowRestartPanel()
    {
        _regamePanel.SetActive(true);
        _regameCheckScript.ShowCheckPanel("REGAME?",
            () =>
            {
                Debug.Log("[Shooting] 게임 재시작");
            },
            () =>
            {
                Debug.Log("[Shooting] 게임 종료");
            });
    }
}
