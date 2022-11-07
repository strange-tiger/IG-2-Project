using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShootingUIManager : MonoBehaviour
{
    [SerializeField] private ShootingGameManager _shootingGameManager;

    [SerializeField] private GameObject _startPlayerPanel;
    private ShootingPlayerUI _playerUI;

    [SerializeField] private GameObject _countDownPanel;
    private TextMeshProUGUI _countDownText;
    
    [SerializeField] private GameObject _playerPanel;
    //[SerializeField] private GameObject _endScorePanel;

    private GameObject _currentPanel;

    private void Awake()
    {
        _startPlayerPanel.SetActive(false);

        _countDownPanel.SetActive(false);
        _countDownText = _countDownPanel.GetComponentInChildren<TextMeshProUGUI>();
        
        _playerPanel.SetActive(false);
        _playerUI = _playerPanel.GetComponent<ShootingPlayerUI>();
        _playerUI.ConnectEvent(_shootingGameManager);

        _currentPanel = _startPlayerPanel;
    }

    public void SetCanvasPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void ShowStartPlayerPanel(string[] playerNicknames, Color[] playerColors)
    {
        _currentPanel.SetActive(false);
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

    public void DisableCurrentPanel()
    {
        _currentPanel.SetActive(false);
    }
}
