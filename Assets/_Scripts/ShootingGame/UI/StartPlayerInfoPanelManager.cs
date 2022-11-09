using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayerInfo = ShootingGameManager.ShootingPlayerInfo;

public class StartPlayerInfoPanelManager : MonoBehaviour
{
    private TextMeshProUGUI[] _playerNicknameText = 
        new TextMeshProUGUI[ShootingGameManager._MAX_PLAYER_COUNT];
    private Image[] _playerColor =
        new Image[ShootingGameManager._MAX_PLAYER_COUNT];
    private GameObject[] _isYouImages =
        new GameObject[ShootingGameManager._MAX_PLAYER_COUNT];

    private void Awake()
    {
        for(int i = 0; i< ShootingGameManager._MAX_PLAYER_COUNT; ++i)
        {
            GameObject nextChild = transform.GetChild(i).gameObject;
            _playerNicknameText[i] = nextChild.GetComponentInChildren<TextMeshProUGUI>();
            _playerColor[i] = nextChild.GetComponent<Image>();
            _isYouImages[i] = nextChild.GetComponentsInChildren<Image>()[1].gameObject;
            _isYouImages[i].SetActive(false);
        }
        gameObject.SetActive(false);
    }

    public void SetPlayerList(List<PlayerInfo> playerInfos)
    {
        for(int i = 0; i < playerInfos.Count; ++i)
        {
            _playerNicknameText[i].text = playerInfos[i].PlayerNickname;
            _playerColor[i].color = playerInfos[i].PlayerColor;
            //만약 isMine 이면 
            _isYouImages[i].SetActive(true);
        }
    }
}
