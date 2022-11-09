#define _DEV_MODE_

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
    public const int _MAX_PLAYER_COUNT = 4;
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

#if _DEV_MODE_
        for (int i = 0; i < _MAX_PLAYER_COUNT; ++i)
        {
            _shootingPlayerInfos.Add(new ShootingPlayerInfo()
            {
                PlayerColor = _playerColors[i],
                PlayerNumber = (EShootingPlayerNumber)i,
                PlayerNickname = "Nickname" + i.ToString(),
                PlayerScore = 3
            });
        }
        StartCoroutine(CoGameStart());
#endif
    }

    public void StartGame()
    {
        StartCoroutine(CoGameStart());
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
            _shootingPlayerInfos.Add(new ShootingPlayerInfo()
            {
                PlayerNumber = (EShootingPlayerNumber) _playerCount,
                PlayerNickname = playerNickname,
                PlayerColor = _playerColors[_playerCount],
            });
            photonView.RPC("PlayerAdded", RpcTarget.AllBuffered, _playerCount, playerNickname);
            ++_playerCount;
        }
    }

    [PunRPC]
    private void PlayerAdded(int playerNumber, string playerNickname)
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            _shootingPlayerInfos.Add(new ShootingPlayerInfo()
            {
                PlayerNumber = (EShootingPlayerNumber)playerNumber,
                PlayerNickname = playerNickname,
                PlayerColor = _playerColors[playerNumber]
            });
        }

        if (_shootingPlayerInfos.Count == _MAX_PLAYER_COUNT)
        {
            foreach (ShootingPlayerInfo info in _shootingPlayerInfos)
            {
                if (info.PlayerNickname == _myClientNickname)
                {
                    _myClient.PlayerInfoSetting(info.PlayerNumber, info.PlayerColor);
                 
                    int playerNumberInt = (int)info.PlayerNumber;
                    _myClient.transform.root.position = 
                        _playerPosition[playerNumberInt].position;
                    _uiCanvas.transform.position = _canvasPosition[playerNumberInt].position;
                    return;
                }
            }

            StartGame();
        }
    }
    
    private IEnumerator CoGameStart()
    {
        _luncherManager.SetLuncher(PlayTime);
        yield return new WaitForSeconds(_showNicknameOffsetTime);

        Debug.Log("[Shooting] UI ���");
        _uiManager.ShowStartPlayerPanel(_shootingPlayerInfos);
        yield return new WaitForSeconds(_startCountDownOffsetTime);

        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(CoInGame());
        }
    }

    private IEnumerator CoInGame()
    {
        Debug.Log("[Shooting] UI ����");
        _uiManager.photonView.RPC("DisableCurrentPanel", RpcTarget.AllBuffered); // ����ȭ ���ʿ� (�ذ�)

        for (int i = _gameStartOffsetTime; i >= 0; --i)
        {
            Debug.Log("[Shooting] ���� ���۱��� " + i);

            //if (i != 0)
            //{
            //    _uiManager.ShowCountDownPanel(i.ToString()); // ����ȭ �ʿ�(�ذ�)
            //}
            //else
            //{
            //    _uiManager.ShowCountDownPanel("ATTACK!"); // ����ȭ �ʿ�(�ذ�)
            //}
            //_audioSource.PlayOneShot(_startCountDownSounds[_gameStartOffsetTime - i]); // ����ȭ �ʿ� (�ذ�)
            photonView.RPC("CountDown", RpcTarget.AllViaServer, i);
            yield return _waitForSecond;
        }

        Debug.Log("[Shooting] ���� ����");
        _uiManager.photonView.RPC("ShowPlayerPanel", RpcTarget.AllBuffered); // ����ȭ ���ʿ�(�ذ�)

        ElapsedTime = 0;

        while (true)
        {
            yield return _waitForSecond;
            ElapsedTime += 1; // ����ȭ, ����ȭ �ʿ� (�ذ�)
            _luncherManager.LunchObject(ElapsedTime); // ���������� �۵�
            if(ElapsedTime == PlayTime)
            {
                break;
            }
        }

        while(true)
        {
            yield return _waitForSecond;
            ElapsedTime += 1; // ����ȭ, ����ȭ �ʿ�
            _luncherManager.LunchObject(ElapsedTime); // ���������� �۵�
            //_audioSource.PlayOneShot(_finalSoundEffect[ElapsedTime - 1 - (int)PlayTime]); // ����ȭ �ʿ� (�ذ�)
            photonView.RPC("PlaySoundEffect", RpcTarget.AllViaServer, _startCountDownSounds.Length + ElapsedTime - 1 - (int)PlayTime);
            if(ElapsedTime >= PlayTime + _finalSeconds)
            {
                break;
            }
        }

        yield return _waitForSecond;
        //_audioSource.PlayOneShot(_finalSoundEffect[_finalSoundEffect.Length - 1]); // ����ȭ �ʿ�(�ذ�)
        //_uiManager.ShowCountDownPanel("STOP!"); // ����ȭ �ʿ�(�ذ�)
        photonView.RPC("StopGame", RpcTarget.AllBufferedViaServer);

        Debug.Log("[Shooting] ���� ��");
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
        Debug.Log("[Shooting] ���� ��� ���");
        SetPlayerList();
        _uiManager.ShowEndScore(_shootingPlayerInfos);

        yield return _waitForSecond;
        Debug.Log("[Shooting] Ȳ�� �� �޾��ֱ�");
        _uiManager.ShowStarImage();

        yield return new WaitForSeconds(_hoorayTime);
        Debug.Log("[Shooting] ȯȣ �� �����, ��� ��� ����");
        GiveGold();
        _uiManager.ShowGoldPanel();
        _uiManager.ShowRestartPanel();
    }

    private void SetPlayerList()
    {
        // ���� ������ ����
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
        
        foreach(ShootingPlayerInfo playerInfo in _shootingPlayerInfos)
        {
            if(playerInfo.PlayerScore == highestScore)
            {
                playerInfo.IsWinner = true;
                playerInfo.PlayerGold = playerInfo.PlayerScore * 2;
            }
            else
            {
                playerInfo.PlayerGold = playerInfo.PlayerScore;
            }
        }
    }

    private void GiveGold()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        foreach(ShootingPlayerInfo playerinfo in _shootingPlayerInfos)
        {
            MySqlSetting.EarnGold(playerinfo.PlayerNickname, playerinfo.PlayerGold);
        }
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
