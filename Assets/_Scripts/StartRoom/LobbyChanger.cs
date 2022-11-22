using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using SceneNumber = Defines.ESceneNumder;
using Asset.MySql;

public class LobbyChanger : MonoBehaviourPunCallbacks
{
    [SerializeField] private bool _isStartRoom;
    [SerializeField] private OVRRaycaster[] _canvases;
    [SerializeField] private GameObject _playerPrefab;
    
    protected GameObject _myPlayer;

    private SceneNumber _nextScene;
    private string _nextSceneRoomName;
    private RoomOptions _nextRoomOption;
    private bool _needSceneChange = false;
    private bool _canBeManyRooms = false;
    private RoomOptions _roomOptions = new RoomOptions
    {
        PublishUserId = true,
        MaxPlayers = 30
    };

    private int _roomCount = 0;

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

    public void ChangeLobby(SceneNumber sceneNumber)
    {
        ChangeLobby(sceneNumber, sceneNumber.ToString(), _roomOptions);
    }

    public void ChangeLobby(SceneNumber sceneNumber, RoomOptions roomOption)
    {
        ChangeLobby(sceneNumber, sceneNumber.ToString(), roomOption);
    }

    public void ChangeLobby(SceneNumber sceneNumber, RoomOptions roomOption, bool canBeManyRoom = false)
    {
        ChangeLobby(sceneNumber, sceneNumber.ToString(), roomOption, canBeManyRoom);
    }

    public void ChangeLobby(SceneNumber sceneNumber, string roomName, RoomOptions roomOption, bool canBeManyRoom = false)
    {
        _nextSceneRoomName = roomName;
        _nextScene = sceneNumber;
        _nextRoomOption = roomOption;
        _canBeManyRooms = canBeManyRoom;
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
            Debug.LogError("[LobbyChanger] " + roomName);
            PhotonNetwork.JoinOrCreateRoom(roomName, _nextRoomOption, TypedLobby.Default);
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

    public override void OnJoinedLobby()
    {
        if (_needSceneChange)
        {
            if (_nextScene <= SceneNumber.StartRoom)
            {
                PhotonNetwork.LoadLevel((int)_nextScene);

                return;
            }
            Debug.Log("[LogOut] LobbyChanger OnJoinedLobby");
            PhotonNetwork.JoinOrCreateRoom(_nextSceneRoomName, _nextRoomOption, TypedLobby.Default);
        }
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("[LogOut] LobbyChanger OnCreatedRoom" + PhotonNetwork.CurrentRoom.Name);
        MySqlSetting.UpdateValueByBase(Asset.EaccountdbColumns.Nickname, PhotonNetwork.NickName, Asset.EaccountdbColumns.IsOnline, 1);
    }

    public override void OnJoinedRoom()
    {
        if (_needSceneChange)
        {
            Debug.Log("[LogOut] LobbyChanger OnCreatedRoom" + PhotonNetwork.CurrentRoom.Name);
            PhotonNetwork.LoadLevel((int)_nextScene);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (_canBeManyRooms)
        {
            Debug.Log("[LogOut] LobbyChanger OnJoinRoomFailed, Reconnecting with new name");
            PhotonNetwork.JoinOrCreateRoom(_nextSceneRoomName + _roomCount.ToString(), _nextRoomOption, TypedLobby.Default);
            ++_roomCount;
        }
        else
        {
            Debug.Log("[LogOut] LobbyChanger OnJoinRoomFailed, Reconnecting with same name");
            PhotonNetwork.JoinOrCreateRoom(_nextSceneRoomName, _nextRoomOption, TypedLobby.Default);
        }
    }
}
