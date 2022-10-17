using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI _stateText;

    [SerializeField]
    private Button _startButton;

    private void Awake()
    {
        _stateText.text = "Ready...";

        _startButton.onClick.AddListener(() => { OnClickStartButton(); });

        PhotonNetwork.ConnectUsingSettings();

        DeactiveJoinButton();
    }

    public override void OnConnectedToMaster()
    {
        _stateText.text = "Ready!!!";
        Debug.Log("마스터 서버 접속 완료");

        ActiveJoinButton();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        _stateText.text = "ReReady......";
        Debug.Log("마스터 서버 재접속 중");

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedLobby()
    {
        _stateText.text = "MainLobby";
        PhotonNetwork.LoadLevel("Lobby");
    }

    public void OnClickStartButton()
    {
        _stateText.text = "Loading";

        Debug.Log("스타트 버튼 클릭");

        PhotonNetwork.JoinLobby();
    }

    private void DeactiveJoinButton()
    {
        _startButton.interactable = false;
    }

    private void ActiveJoinButton()
    {
        _startButton.interactable = true;
    }

}
