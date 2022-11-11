using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyChanger : MonoBehaviourPunCallbacks
{
    private Defines.ESceneNumder _nextScene;
    private bool _needSceneChange = false;
    [SerializeField] private bool _isStartRoom;
    [SerializeField] private OVRRaycaster[] _canvases;
    [SerializeField] private GameObject _playerPrefab;
    protected GameObject _myPlayer;

    protected virtual void Awake()
    {
        if (!_isStartRoom)
        {
            GameObject player = PhotonNetwork.Instantiate(_playerPrefab.name, new Vector3(0f, 1f, 3f),
                Quaternion.Euler(0f, 0f, 0f), 0, null);
            BasicPlayerNetworking playerNetworking = player.GetComponent<BasicPlayerNetworking>();
            playerNetworking.photonView.RPC("SetNickname", RpcTarget.All, TempAccountDB.ID, TempAccountDB.Nickname);
            playerNetworking.CanvasSetting(_canvases);
            _myPlayer = player;
        }
    }

    public void ChangeLobby(Defines.ESceneNumder sceneNumber)
    {
        _nextScene = sceneNumber;
        _needSceneChange = true;

        OVRScreenFade.instance.FadeOut();

        PlayerControlManager.Instance.IsRayable = false;
        PlayerControlManager.Instance.IsMoveable = false;

        if (!_isStartRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            Debug.LogError("[LobbyChanger] " + _nextScene.ToString());
            PhotonNetwork.JoinOrCreateRoom(_nextScene.ToString(), _roomOptions, TypedLobby.Default);
        }
    }

    public override void OnConnectedToMaster()
    {
        if (_needSceneChange)
        {
            Debug.Log("[LogOut] LobbyChanger OnConnectedToMaster");
            PhotonNetwork.JoinLobby();
        }
    }

    private RoomOptions _roomOptions = new RoomOptions
    {
        MaxPlayers = 30
    };
    public override void OnJoinedLobby()
    {
        if (_needSceneChange)
        {
            if (_nextScene <= Defines.ESceneNumder.StartRoom)
            {
                PhotonNetwork.LoadLevel((int)_nextScene);

                return;
            }

            PhotonNetwork.JoinOrCreateRoom(_nextScene.ToString(), _roomOptions, TypedLobby.Default);
        }
    }

    public override void OnJoinedRoom()
    {
        if (_needSceneChange)
        {
            Debug.Log("[LogOut] LobbyChanger OnJoinedRoom");
            PhotonNetwork.LoadLevel((int)_nextScene);
        }
    }
}
