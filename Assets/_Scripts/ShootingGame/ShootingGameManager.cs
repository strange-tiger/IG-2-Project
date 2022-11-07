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
    }

    [SerializeField] private float _playTime = 120f;
    public float PlayTime { get => _playTime; set => _playTime = value; }

    [Header("SettingTime")]
    [SerializeField] private float _showNicknameOffsetTime = 1f;
    [SerializeField] private float _startCountDownOffsetTime = 3f;
    [SerializeField] private AudioClip[] _startCountDownSounds;
    [SerializeField] private int _gameStartOffsetTime = 3;
    [SerializeField] private float _hoorayTime = 4f;
    
    [Header("FinalShots")]
    [SerializeField] private float _finalSeconds = 5f;
    [SerializeField] private AudioClip[] _finalSoundEffect;
    private WaitForSeconds _waitForSecond;

    public const int _MAX_PLAYER_COUNT = 4;
    [SerializeField] private Color[] _playerColors = {
        new Color(255/255, 50/255, 50/255),
        new Color(50/255, 140/255, 255/255),
        new Color(255/255, 50/255, 255/255),
        new Color(53/255, 227/255, 34/255)
    };
    //[SerializeField] private Transform[] _playerPosition;
    private int _playerCount = 0;

    public class ShootingPlayerInfo
    {
        public EShootingPlayerNumber PlayerNumber { get; set; }
        public string PlayerNickname { get; set; }
        public Color PlayerColor { get; set; }
        public int PlayerScore { get; set; }
    }
    private List<ShootingPlayerInfo> _shootingPlayerInfos = new List<ShootingPlayerInfo>();

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

    public void SetPlayerNumberAndColor(out Color playerColor, out EShootingPlayerNumber playerNumber, in string playerNickname)
    {
        _shootingPlayerInfos.Add(new ShootingPlayerInfo()
        {
            PlayerNickname = playerNickname,
            PlayerNumber = (EShootingPlayerNumber)_playerCount,
            PlayerColor = _playerColors[_playerCount],
            PlayerScore = 0
        });
        ++_playerCount;

        playerColor = _shootingPlayerInfos[_playerCount].PlayerColor;
        playerNumber = _shootingPlayerInfos[_playerCount].PlayerNumber;
    }

    private IEnumerator CoGameStart()
    {
        _luncherManager.SetLuncher(PlayTime);

        yield return new WaitForSeconds(_showNicknameOffsetTime);
        Debug.Log("[Shooting] UI 띄움");

        _uiManager.ShowStartPlayerPanel(_shootingPlayerInfos);
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
        Debug.Log("[Shooting] 게임 결과 출력");
        _shootingPlayerInfos.Sort(delegate (ShootingPlayerInfo playerA, ShootingPlayerInfo playerB)
        {
            if (playerA.PlayerScore < playerB.PlayerScore)
            {
                return -1;
            }
            else if (playerA.PlayerScore > playerB.PlayerScore)
            {
                return 1;
            }
            else
            {
                if (playerA.PlayerNumber > playerB.PlayerNumber)
                {
                    return -1;
                }
                else if (playerA.PlayerNumber < playerB.PlayerNumber)
                {
                    return 1;
                }
            }

            return 0;
        });
        _uiManager.ShowEndScore(_shootingPlayerInfos);

        yield return _waitForSecond;
        Debug.Log("[Shooting] 황금 별 달아주기");
        _uiManager.ShowStarImage();

        yield return new WaitForSeconds(_hoorayTime);
        Debug.Log("[Shooting] 환호 후 재시작 페널 띄우기");
        _uiManager.ShowGoldPanel();
        _uiManager.ShowRestartPanel();
    }

    public void AddScoreToPlayer(EShootingPlayerNumber playerNumber, int addPoint)
    {
        _shootingPlayerInfos[(int)playerNumber].PlayerScore += addPoint;
        OnAddScore.Invoke(playerNumber, _shootingPlayerInfos[(int)playerNumber].PlayerScore);
    }
}
