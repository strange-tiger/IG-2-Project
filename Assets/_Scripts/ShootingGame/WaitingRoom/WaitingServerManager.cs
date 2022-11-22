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
    private GameObject _doorInteraction;
    private GameObject _doorSencer;

    [SerializeField] private TextMeshProUGUI _playerCountText;

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

        _doorInteraction = _door.GetComponentInChildren<WaitingRoomDoorInteraction>().gameObject;
        _doorInteraction.SetActive(false);
        _doorSencer = _door.GetComponentInChildren<DoorSenser>().gameObject;

        _playerCountText.text = $"{PhotonNetwork.PlayerList.Length.ToString()}/{_MAX_PLAYER_COUNT}";
        photonView.RPC("PlayerEntered", RpcTarget.All);

    }

    [PunRPC]
    private void PlayerEntered()
    {
        Debug.Log("[ShootingWaiting] 플레이어 참가함");
        int playerCount = PhotonNetwork.PlayerList.Length;
        _playerCountText.text = $"{PhotonNetwork.PlayerList.Length.ToString()}/{_MAX_PLAYER_COUNT}"; ;

        if (playerCount == _MAX_PLAYER_COUNT)
        {
            Debug.Log("[ShootingWaiting] 모든 플레이어 모임");
            PhotonNetwork.CurrentRoom.IsOpen = false;
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
        _doorInteraction.GetComponent<WaitingRoomDoorInteraction>().OutFocus();
        _doorInteraction.SetActive(false);
        _doorSencer.SetActive(false);

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
