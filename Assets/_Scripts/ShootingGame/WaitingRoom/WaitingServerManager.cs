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

    [SerializeField] private int _countDownSeconds = 3;
    [SerializeField] private AudioClip[] _countDownAudioClips = new AudioClip[3];
    private AudioSource _audioSource;

    [SerializeField] private GameObject _countDownPrefab;
    private StartGameCountDown _countDownScript;

    private WaitForSeconds _waitForSecond;

    protected override void Awake()
    {
        base.Awake();
        if(photonView.IsMine)
        {
            GameObject countDown = Instantiate(_countDownPrefab, MenuUIManager.Instance.transform.parent.GetChild(0));
            countDown.SetActive(false);
        }

        _waitForSecond = new WaitForSeconds(1f);
        _audioSource = GetComponent<AudioSource>();

        _exitWaitingScript = _door.GetComponent<ExitWaiting>();
        _doorSenserScript = _door.GetComponent<DoorSenser>();

        _playerCountText.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        _playerCountText.text = playerCount.ToString();

        if (PhotonNetwork.IsMasterClient)
        {
            if(playerCount == ShootingGameManager._MAX_PLAYER_COUNT)
            {
                StartGame();
            }
        }
    }

    private void StartGame()
    {
        _doorSenserScript.enabled = false;
        _exitWaitingScript.OutFocus();
        _exitWaitingScript.enabled = false;
        StartCoroutine(CoStartCountDown());
    }

    private IEnumerator CoStartCountDown()
    {
        for(int i = 0; i < _countDownSeconds; ++i)
        {
            photonView.RPC("CountDown", RpcTarget.All, i);
            yield return _waitForSecond;
        }

        photonView.RPC("EnterGame", RpcTarget.All);
    }

    [PunRPC]
    private void CountDown(int countTime)
    {
        _countDownScript.SetCountDownText((_countDownSeconds - countTime).ToString());
        _audioSource.PlayOneShot(_countDownAudioClips[countTime]);
    }

    [PunRPC]
    private void EnterGame()
    {
        PhotonNetwork.LoadLevel((int)SceneNumber.ShootingGameRoom);
    }
}
