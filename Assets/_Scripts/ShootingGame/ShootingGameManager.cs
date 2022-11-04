using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingGameManager : MonoBehaviour
{
    public float PlayTime { get; set; } = 120f;

    [SerializeField] private float _showNicknameOffsetTime = 1f;
    [SerializeField] private float _startCountDownOffsetTime = 3f;
    [SerializeField] private int _gameStartOffsetTime = 3;

    [SerializeField] private float _finalSeconds = 5f;
    private WaitForSeconds _waitForSecond;

    private const int _MAX_PLAYER_COUNT = 4;
    [SerializeField] private Color[] _playerColors = {
        new Color(255/255, 50/255, 50/255),
        new Color(50/255, 140/255, 255/255),
        new Color(255/255, 50/255, 255/255),
        new Color(53/255, 227/255, 34/255)
    };

    [SerializeField] private LuncherManager _luncherManager;

    private int elapsedTime = 0;

    private void Awake()
    {
        _waitForSecond = new WaitForSeconds(1f);
    }

    private IEnumerator CoGameStart()
    {
        _luncherManager.SetLuncher(PlayTime);

        yield return new WaitForSeconds(_showNicknameOffsetTime);
        Debug.Log("[Shooting] UI 띄움");
        yield return new WaitForSeconds(_startCountDownOffsetTime);

        Debug.Log("[Shooting] UI 내림");
        for(int i = _gameStartOffsetTime; i >= 0; --i)
        {
            Debug.Log("[Shooting] 게임 시작까지 " + i);
            yield return _waitForSecond;
        }

        Debug.Log("[Shooting] 게임 시작");
        StartCoroutine(CoInGame());
    }

    private IEnumerator CoInGame()
    {
        while (true)
        {
            yield return _waitForSecond;
            elapsedTime += 1;
            if(elapsedTime == PlayTime)
            {
                break;
            }
        }

        while(true)
        {
            yield return _waitForSecond;
            elapsedTime += 1;
            if(elapsedTime >= PlayTime + _finalSeconds)
            {
                break;
            }
        }
    }
}
