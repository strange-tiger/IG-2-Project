using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayerInfo = ShootingGameManager.ShootingPlayerInfo;

public class ShootingUIManager : MonoBehaviour
{
    [SerializeField] private ShootingGameManager _shootingGameManager;
    private StartPlayerInfoPanelManager _startPlayerInfoScript;

    [SerializeField] private GameObject _startPlayerPanel;
    private ShootingPlayerUI _playerUI;

    [SerializeField] private GameObject _countDownPanel;
    private TextMeshProUGUI _countDownText;
    
    [SerializeField] private GameObject _playerPanel;

    [SerializeField] private GameObject _endScorePanel;
    private EndResultPanel _endScoreScript;

    private GameObject _currentPanel;

    private void Awake()
    {
        _startPlayerPanel.SetActive(false);
        _startPlayerInfoScript = _startPlayerPanel.GetComponent<StartPlayerInfoPanelManager>();

        _countDownPanel.SetActive(false);
        _countDownText = _countDownPanel.GetComponentInChildren<TextMeshProUGUI>();
        
        _playerPanel.SetActive(false);
        _playerUI = _playerPanel.GetComponent<ShootingPlayerUI>();
        _playerUI.ConnectEvent(_shootingGameManager);

        _endScorePanel.SetActive(false);
        _endScoreScript = _endScorePanel.GetComponent<EndResultPanel>();

        _currentPanel = _startPlayerPanel;
    }

    public void SetCanvasPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void ShowStartPlayerPanel(List<PlayerInfo> infos)
    {
        _currentPanel.SetActive(false);
        _startPlayerInfoScript.SetPlayerList(infos);
        _startPlayerPanel.SetActive(true);
        _currentPanel = _startPlayerPanel;
    }

    public void ShowCountDownPanel(string message)
    {
        _currentPanel.SetActive(false);
        _countDownText.text = message;
        _countDownPanel.SetActive(true);
        _currentPanel = _countDownPanel;
    }

    public void ShowPlayerPanel()
    {
        _currentPanel.SetActive(false);
        _playerPanel.SetActive(true);
        _currentPanel = _playerPanel;
    }

    public void ShowEndScore(List<PlayerInfo> infos)
    {
        _currentPanel.SetActive(false);
        _endScoreScript.SetPlayerList(infos);
        _endScorePanel.SetActive(true);
        _currentPanel = _endScorePanel;
    }

    public void ShowStarImage()
    {
        _endScoreScript.ShowStarImage();
    }

    public void ShowGoldPanel()
    {
        _endScoreScript.ShowGoldPanel();
    }

    public void ShowRestartPanel()
    {
        _endScoreScript.ShowRestartPanel();
    }

    public void DisableCurrentPanel()
    {
        _currentPanel.SetActive(false);
    }
}
