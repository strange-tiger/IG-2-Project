using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using SceneNumber = Defines.ESceneNumder;

public class WaitingServerManager : LobbyChanger
{
    [SerializeField] private GameObject _door;
    private ExitWaiting _exitWaitingScript;
    private DoorSenser _doorSenserScript;

    [SerializeField] private TextMeshProUGUI _playerCountText;
    [SerializeField] private TextMeshProUGUI _maxPlayerCountText;

    [SerializeField] private int _countDownSeconds = 3;
    [SerializeField] private AudioClip[] _countDownAudioClips = new AudioClip[3];
    private AudioSource _audioSource;

    [SerializeField] private GameObject _countDownPrefab;
    private StartGameCountDown _countDownScript;
    private TextMeshProUGUI _countDownText;

    private WaitForSeconds _waitForSecond;

    private const int _MAX_PLAYER_COUNT = ShootingGameManager._MAX_PLAYER_COUNT;

    protected override void Awake()
    {
        base.Awake();

        GameObject countDown = Instantiate(_countDownPrefab, MenuUIManager.Instance.transform.parent.GetChild(0));
        _countDownScript = countDown.GetComponent<StartGameCountDown>();
        _countDownText = countDown.GetComponentInChildren<TextMeshProUGUI>();
        _countDownText.gameObject.SetActive(false);

        _waitForSecond = new WaitForSeconds(1f);
        _audioSource = GetComponent<AudioSource>();

        _exitWaitingScript = _door.GetComponent<ExitWaiting>();
        _doorSenserScript = _door.GetComponent<DoorSenser>();

        _playerCountText.text = PhotonNetwork.PlayerList.Length.ToString();
        photonView.RPC("PlayerEntered", RpcTarget.All);

        _maxPlayerCountText.text = $"/{_MAX_PLAYER_COUNT}";
    }

    [PunRPC]
    private void PlayerEntered()
    {
        Debug.Log("[ShootingWaiting] 플레이어 참가함");
        int playerCount = PhotonNetwork.PlayerList.Length;
        _playerCountText.text = playerCount.ToString();

        if (playerCount == _MAX_PLAYER_COUNT)
        {
            Debug.Log("[ShootingWaiting] 모든 플레이어 모임");
            StartGame();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("[ShootingWaiting] 플레이어 나감");
        int playerCount = PhotonNetwork.PlayerList.Length;
        _playerCountText.text = playerCount.ToString();
    }

    private void StartGame()
    {
        _doorSenserScript.enabled = false;
        _exitWaitingScript.OutFocus();
        _exitWaitingScript.enabled = false;

        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(CoStartCountDown());
        }
    }

    private IEnumerator CoStartCountDown()
    {
        for(int i = 0; i <= _countDownSeconds; ++i)
        {
            int countTime = i;
            photonView.RPC("CountDown", RpcTarget.All, countTime);
            yield return _waitForSecond;
        }

        photonView.RPC("EnterGame", RpcTarget.All);
    }

    [PunRPC]
    private void CountDown(int countTime)
    {
        Debug.Log("[ShootingWaiting] CountDown 중");
        _countDownScript.SetCountDownText((_countDownSeconds - countTime).ToString());
        _countDownText.gameObject.SetActive(true);
        _audioSource.PlayOneShot(_countDownAudioClips[countTime]);
    }

    [PunRPC]
    private void EnterGame()
    {
        PhotonNetwork.LoadLevel((int)SceneNumber.ShootingGameRoom);
    }
}
