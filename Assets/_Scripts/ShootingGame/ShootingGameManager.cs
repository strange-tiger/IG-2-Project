using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShootingGameManager : MonoBehaviour
{
    public enum EShootingPlayerNumber
    {
        Player1,
        Player2,
        Player3,
        Player4,
        Max
    }

    [SerializeField] private float _playTime = 120f;
    public float PlayTime { get => _playTime; set => _playTime = value; }

    [Header("SettingTime")]
    [SerializeField] private float _showNicknameOffsetTime = 1f;
    [SerializeField] private float _startCountDownOffsetTime = 3f;
    [SerializeField] private AudioClip[] _startCountDownSounds;
    [SerializeField] private int _gameStartOffsetTime = 3;

    [Header("FinalShots")]
    [SerializeField] private float _finalSeconds = 5f;
    [SerializeField] private AudioClip[] _finalSoundEffect;
    private WaitForSeconds _waitForSecond;

    private const int _MAX_PLAYER_COUNT = 4;
    [SerializeField] private Color[] _playerColors = {
        new Color(255/255, 50/255, 50/255),
        new Color(50/255, 140/255, 255/255),
        new Color(255/255, 50/255, 255/255),
        new Color(53/255, 227/255, 34/255)
    };
    private int[] _playerScore = new int[(int)EShootingPlayerNumber.Max];

    [SerializeField] private LuncherManager _luncherManager;
    [SerializeField] private GameObject _lunchObjects;
    [SerializeField] private ShootingUIManager _uiManager;

    public UnityEvent<int> OnTimePass = new UnityEvent<int>();
    private int _elapsedTime = 0;
    public int ElapsedTime
    {
        get => _elapsedTime;
        set
        {
            _elapsedTime = value;
            OnTimePass.Invoke(_elapsedTime);
        }
    }

    public UnityEvent<EShootingPlayerNumber, int> OnAddScore = new UnityEvent<EShootingPlayerNumber, int>();


    private AudioSource _audioSource;

    private void Awake()
    {
        _waitForSecond = new WaitForSeconds(1f);

        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(CoGameStart());
    }

    private IEnumerator CoGameStart()
    {
        _luncherManager.SetLuncher(PlayTime);

        yield return new WaitForSeconds(_showNicknameOffsetTime);
        Debug.Log("[Shooting] UI 띄움");
        _uiManager.ShowStartPlayerPanel(null, _playerColors);
        yield return new WaitForSeconds(_startCountDownOffsetTime);

        Debug.Log("[Shooting] UI 내림");
        _uiManager.DisableCurrentPanel();
        for(int i = _gameStartOffsetTime; i >= 0; --i)
        {
            Debug.Log("[Shooting] 게임 시작까지 " + i);
            if(i != 0)
            {
                _uiManager.ShowCountDownPanel(i.ToString());
            }
            else
            {
                _uiManager.ShowCountDownPanel("ATTACK!");
            }
            _audioSource.PlayOneShot(_startCountDownSounds[_gameStartOffsetTime - i]);
            yield return _waitForSecond;
        }

        Debug.Log("[Shooting] 게임 시작");
        _uiManager.ShowPlayerPanel();
        StartCoroutine(CoInGame());
    }

    private IEnumerator CoInGame()
    {
        ElapsedTime = 0;

        while (true)
        {
            yield return _waitForSecond;
            ElapsedTime += 1;
            _luncherManager.LunchObject(ElapsedTime);
            if(ElapsedTime == PlayTime)
            {
                break;
            }
        }

        while(true)
        {
            yield return _waitForSecond;
            ElapsedTime += 1;
            _luncherManager.LunchObject(ElapsedTime);
            _audioSource.PlayOneShot(_finalSoundEffect[ElapsedTime - 1 - (int)PlayTime]);
            if(ElapsedTime >= PlayTime + _finalSeconds)
            {
                break;
            }
        }

        yield return _waitForSecond;
        _audioSource.PlayOneShot(_finalSoundEffect[_finalSoundEffect.Length - 1]);
        _uiManager.ShowCountDownPanel("STOP!");
        Destroy(_lunchObjects);

        Debug.Log("[Shooting] 게임 끝");
        StartCoroutine(CoGameEnd());
    }

    private IEnumerator CoGameEnd()
    {
        yield return _waitForSecond;
        _uiManager.DisableCurrentPanel();
    }

    public void AddScoreToPlayer(EShootingPlayerNumber playerNumber, int addPoint)
    {
        _playerScore[(int)playerNumber] += addPoint;
        OnAddScore.Invoke(playerNumber, _playerScore[(int)playerNumber]);
    }
}
