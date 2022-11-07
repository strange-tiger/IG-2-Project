using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShootingPlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private ShootingGameManager _gameManager;

    public void ConnectEvent(ShootingGameManager shootingGameManager)
    {
        shootingGameManager.TimePass.RemoveListener(SetTime);
        shootingGameManager.TimePass.AddListener(SetTime);
        _gameManager = shootingGameManager;
    }

    private void SetTime(int time)
    {
        int leftTime = (int)_gameManager.PlayTime - time;
        _timeText.text = leftTime > 0 ? leftTime.ToString() : "0";
    }

    private void OnDestroy()
    {
        _gameManager.TimePass.RemoveListener(SetTime);
    }
}
