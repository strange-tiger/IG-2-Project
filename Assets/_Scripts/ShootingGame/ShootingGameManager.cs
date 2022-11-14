//#define _DEV_MODE_

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Asset.MySql;
using Photon.Pun;

public class ShootingGameManager : MonoBehaviourPun
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

    [Header("PlayTimeSetting")]
    [SerializeField] private float _showNicknameOffsetTime = 1f;
    [SerializeField] private float _startCountDownOffsetTime = 3f;
    [SerializeField] private AudioClip[] _startCountDownSounds;
    [SerializeField] private int _gameStartOffsetTime = 3;
    [SerializeField] private float _hoorayTime = 4f;
    
    [Header("FinalShots")]
    [SerializeField] private float _finalSeconds = 5f;
    [SerializeField] private AudioClip[] _finalSoundEffect;
    private WaitForSeconds _waitForSecond;

    private List<AudioClip> _soundEffects = new List<AudioClip>();

    [Header("PlayerSetting")]
    public const int _MAX_PLAYER_COUNT = 2;
    [SerializeField] private Color[] _playerColors = {
        new Color(255/255, 50/255, 50/255),
        new Color(50/255, 140/255, 255/255),
        new Color(255/255, 50/255, 255/255),
        new Color(53/255, 227/255, 34/255)
    };
    [SerializeField] private Transform[] _playerPosition;
    [SerializeField] private Transform[] _canvasPosition;
    private int _playerCount = 0;
    public int PlayerCount { get => _playerCount; set => _playerCount = value; }

    [Serializable]
    public class ShootingPlayerInfo
    {
        public EShootingPlayerNumber PlayerNumber { get; set; }
        public string PlayerNickname { get; set; }
        public Color PlayerColor { get; set; }
        public int PlayerScore { get; set; }
        public int PlayerGold { get; set; }
        public bool IsWinner { get; set; }
    }
    private List<ShootingPlayerInfo> _shootingPlayerInfos = new List<ShootingPlayerInfo>();

    [Header("Luncher")]
    [SerializeField] private LuncherManager _luncherManager;
    [SerializeField] private GameObject _lunchObjects;
    public GameObject LunchObjects { get => _lunchObjects; private set => _lunchObjects = value; }

    [Header("UI")]
    [SerializeField] private GameObject _uiCanvas;
    [SerializeField] private ShootingUIManager _uiManager;

    [Header("EndSoundEffects")]
    [SerializeField] private AudioClip _goldStarAudioClip;
    [SerializeField] private AudioClip _hoorayAudioClip;
    [SerializeField] private AudioClip _eagleAudioClip;
    [SerializeField] private AudioClip _giveGoldAudioClip;

    public UnityEvent<int> OnTimePass = new UnityEvent<int>();
    private int _elapsedTime = 0;
    public int ElapsedTime
    {
        get => _elapsedTime;
        set
        {
            if(PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("CountTime", RpcTarget.AllViaServer, value);
            }
        }
    }

    public UnityEvent<EShootingPlayerNumber, int> OnAddScore = new UnityEvent<EShootingPlayerNumber, int>();
    public UnityEvent OnGameOver = new UnityEvent();

    private AudioSource _audioSource;
    private GunShoot _myClient;
    private string _myClientNickname;
    private EShootingPlayerNumber _myClientNumber;

    private void Awake()
    {
        _waitForSecond = new WaitForSeconds(1f);

        _audioSource = GetComponent<AudioSource>();
        foreach(AudioClip effect in _startCountDownSounds)
        {
            _soundEffects.Add(effect);
        }
        foreach (AudioClip effect in _finalSoundEffect)
        {
            _soundEffects.Add(effect);
        }
    }

    public void AddPlayer(string playerNickname, GunShoot clientScript)
    {
        _myClient = clientScript;
        _myClientNickname = playerNickname;
        photonView.RPC("AddPlayerToGame", RpcTarget.AllBuffered, playerNickname);
    }

    [PunRPC]
    private void AddPlayerToGame(string playerNickname)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            foreach(ShootingPlayerInfo info in _shootingPlayerInfos)
            {
                if(info.PlayerNickname == playerNickname)
                {
                    return;
                }
            }

            if(_shootingPlayerInfos.Count == _MAX_PLAYER_COUNT)
            {
                return;
            }

            _shootingPlayerInfos.Add(new ShootingPlayerInfo()
            {
                PlayerNumber = (EShootingPlayerNumber) _playerCount,
                PlayerNickname = playerNickname,
                PlayerColor = _playerColors[_playerCount],
            });

            int playerCount = _playerCount;
            string nickname = playerNickname;
            photonView.RPC("PlayerAdded", RpcTarget.AllBuffered, playerCount, nickname);
            ++_playerCount;
        }
    }

    [PunRPC]
    private void PlayerAdded(int playerNumber, string playerNickname)
    {
        Debug.Log("[Shooting] 플레이어 추가됨");
        if(!PhotonNetwork.IsMasterClient)
        {
            _shootingPlayerInfos.Add(new ShootingPlayerInfo()
            {
                PlayerNumber = (EShootingPlayerNumber)playerNumber,
                PlayerNickname = playerNickname,
                PlayerColor = _playerColors[playerNumber]
            });
            Debug.Log("[Shooting] 플레이어 info에 추가함");
        }

        if (_shootingPlayerInfos.Count == _MAX_PLAYER_COUNT)
        {
            Debug.Log("[Shooting] 플레이어 다 모임");
            foreach (ShootingPlayerInfo info in _shootingPlayerInfos)
            {
                if (info.PlayerNickname == _myClientNickname)
                {
                    Debug.Log("[Shooting] 나의 플레이어 정보 초기화 함");
                    int PlayerNumber = (int)info.PlayerNumber;
                    _myClientNumber = info.PlayerNumber;
                    _myClient.transform.root.position =
                        _playerPosition[PlayerNumber].position;
                    _myClient.transform.root.rotation =
                        _playerPosition[playerNumber].rotation;
                    _uiCanvas.transform.position = _canvasPosition[PlayerNumber].position;


                    _myClient.PlayerInfoSetting(info.PlayerNumber, info.PlayerColor);


                    break;
                }
            }

            Debug.Log("[Shooting] 게임 시작함");
            StartGame();
        }
    }

    public void StartGame()
    {
        StartCoroutine(CoGameStart());
    }

    private IEnumerator CoGameStart()
    {
        _luncherManager.SetLuncher(PlayTime);
        Debug.Log("[Shooting] 런처 세팅함");
        yield return new WaitForSeconds(_showNicknameOffsetTime);

        Debug.Log("[Shooting] UI 띄움");
        _uiManager.ShowStartPlayerPanel(_shootingPlayerInfos, _myClientNickname, _myClientNumber);
        yield return new WaitForSeconds(_startCountDownOffsetTime);

        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(CoInGame());
        }
    }

    private IEnumerator CoInGame()
    {
        Debug.Log("[Shooting] UI 내림");
        _uiManager.photonView.RPC("DisableCurrentPanel", RpcTarget.AllBuffered);

        for (int i = _gameStartOffsetTime; i >= 0; --i)
        {
            Debug.Log("[Shooting] 게임 시작까지 " + i);

            photonView.RPC("CountDown", RpcTarget.AllViaServer, i);
            yield return _waitForSecond;
        }

        Debug.Log("[Shooting] 게임 시작");
        _uiManager.photonView.RPC("ShowPlayerPanel", RpcTarget.AllBuffered);

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
            photonView.RPC("PlaySoundEffect", RpcTarget.AllViaServer, _startCountDownSounds.Length + ElapsedTime - 1 - (int)PlayTime);
            if(ElapsedTime >= PlayTime + _finalSeconds)
            {
                break;
            }
        }

        yield return _waitForSecond;
        photonView.RPC("StopGame", RpcTarget.AllBufferedViaServer);

        Debug.Log("[Shooting] 게임 끝");
        photonView.RPC("GameEnd", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void PlaySoundEffect(int number)
    {
        _audioSource.PlayOneShot(_soundEffects[number]);
    }

    [PunRPC]
    private void CountTime(int elapsedTime)
    {
        _elapsedTime = elapsedTime;
        OnTimePass.Invoke(_elapsedTime);
    }

    [PunRPC]
    private void CountDown(int i)
    {
        if (i != 0)
        {
            _uiManager.ShowCountDownPanel(i.ToString());
        }
        else
        {
            _uiManager.ShowCountDownPanel("ATTACK!");
        }
        _audioSource.PlayOneShot(_startCountDownSounds[_gameStartOffsetTime - i]);
    }

    [PunRPC]
    private void StopGame()
    {
        Destroy(_lunchObjects);
        _audioSource.PlayOneShot(_finalSoundEffect[_finalSoundEffect.Length - 1]);
        _uiManager.ShowCountDownPanel("STOP!");
    }

    [PunRPC]
    private void GameEnd()
    {
        StartCoroutine(CoGameEnd());
    }

    private IEnumerator CoGameEnd()
    {
        yield return _waitForSecond;
        Debug.Log("[Shooting] 게임 결과 출력");
        bool isMyClientWinner = SortPlayerListByScore();
        _uiManager.ShowEndScore(_shootingPlayerInfos);

        yield return _waitForSecond;
        Debug.Log("[Shooting] 황금 별 달아주기");
        _uiManager.ShowStarImage();
        _audioSource.PlayOneShot(_goldStarAudioClip);
        if(isMyClientWinner)
        {
            _audioSource.PlayOneShot(_hoorayAudioClip);
        }
        else
        {
            _audioSource.PlayOneShot(_eagleAudioClip);
        }

        yield return new WaitForSeconds(_hoorayTime);
        Debug.Log("[Shooting] 환호 후 재시작, 골드 페널 띄우기");
        GiveGold();
        _uiManager.ShowGoldPanel();
        _audioSource.PlayOneShot(_giveGoldAudioClip);
        _uiManager.ShowRestartPanel();
    }

    private bool SortPlayerListByScore()
    {
        // 점수 순위로 정렬
        _shootingPlayerInfos.Sort(delegate (ShootingPlayerInfo playerA, ShootingPlayerInfo playerB)
        {
            if (playerA.PlayerScore < playerB.PlayerScore)
            {
                return 1;
            }
            else if (playerA.PlayerScore > playerB.PlayerScore)
            {
                return -1;
            }
            else
            {
                if (playerA.PlayerNumber > playerB.PlayerNumber)
                {
                    return 1;
                }
                else if (playerA.PlayerNumber < playerB.PlayerNumber)
                {
                    return -1;
                }
            }

            return 0;
        });

        int highestScore = _shootingPlayerInfos[0].PlayerScore;
        bool isWinner = false;
        
        foreach(ShootingPlayerInfo playerInfo in _shootingPlayerInfos)
        {
            if(playerInfo.PlayerScore == highestScore)
            {
                playerInfo.IsWinner = true;
                playerInfo.PlayerGold = playerInfo.PlayerScore * 2;
                isWinner = playerInfo.PlayerNickname == _myClientNickname;
            }
            else
            {
                playerInfo.PlayerGold = playerInfo.PlayerScore;
            }
        }

        return isWinner;
    }

    private void GiveGold()
    {
#if _DEV_MODE_
        Debug.Log("[Shooting] 골드 지급함");
#else
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        foreach(ShootingPlayerInfo playerinfo in _shootingPlayerInfos)
        {
            MySqlSetting.EarnGold(playerinfo.PlayerNickname, playerinfo.PlayerGold);
        }
        Debug.Log("[Shooting] 골드 지급 됨");
#endif
    }

    public void AddScoreToPlayer(EShootingPlayerNumber playerNumber, int addPoint)
    {
        photonView.RPC("AddScoreToPlayerInMaster", RpcTarget.AllBuffered, playerNumber, addPoint);
    }

    [PunRPC]
    private void AddScoreToPlayerInMaster(EShootingPlayerNumber playerNumber, int addPoint)
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        foreach (ShootingPlayerInfo info in _shootingPlayerInfos)
        {
            if (info.PlayerNumber == playerNumber)
            {
                info.PlayerScore += addPoint;
                OnAddScore.Invoke(playerNumber, info.PlayerScore);
                photonView.RPC("PlayerScoreAdded", RpcTarget.AllBuffered, 
                    playerNumber, info.PlayerScore);
                break;
            }
        }
    }

    [PunRPC]
    private void PlayerScoreAdded(EShootingPlayerNumber playerNumber, int score)
    {
        foreach (ShootingPlayerInfo info in _shootingPlayerInfos)
        {
            if (info.PlayerNumber == playerNumber)
            {
                info.PlayerScore = score;
                OnAddScore.Invoke(playerNumber, score);
                break;
            }
        }
    }
}
