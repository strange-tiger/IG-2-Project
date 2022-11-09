using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayerNumber = ShootingGameManager.EShootingPlayerNumber;

public class ShootingPlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private ShootingGameManager _shootingGameManager;

    private PlayerNumber _playerNumber;

    public void ConnectEvent(ShootingGameManager shootingGameManager)
    {
        shootingGameManager.OnTimePass.RemoveListener(SetTime);
        shootingGameManager.OnTimePass.AddListener(SetTime);

        shootingGameManager.OnAddScore.RemoveListener(SetScore);
        shootingGameManager.OnAddScore.AddListener(SetScore);

        _shootingGameManager = shootingGameManager;
    }

    private void SetTime(int time)
    {
        int leftTime = (int)_shootingGameManager.PlayTime - time;
        _timeText.text = leftTime > 0 ? leftTime.ToString() : "0";
    }

    private void SetScore(PlayerNumber playerNumber, int score)
    {
        if(playerNumber != _playerNumber)
        {
            return;
        }
        
        _scoreText.text = score.ToString();
    }

    private void OnDestroy()
    {
        _shootingGameManager.OnTimePass.RemoveListener(SetTime);
        _shootingGameManager.OnAddScore.RemoveListener(SetScore);
    }
}
