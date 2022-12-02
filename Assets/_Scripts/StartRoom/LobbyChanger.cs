using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Asset.MySql;
using SceneNumber = Defines.ESceneNumber;
using MapType = Defines.EMapType;

public class LobbyChanger : MonoBehaviourPunCallbacks
{
    [SerializeField] private bool _isInLobby;
    [SerializeField] private OVRRaycaster[] _canvases;
    [SerializeField] private GameObject _playerPrefab;

    [SerializeField] private Vector3 _playerSpawnPosition = new Vector3(0f, 1f, 3f);
    [SerializeField] private Vector3 _playerSpawnRotatinon;

    [SerializeField] protected MapType _mapType;
    [SerializeField] protected bool _isFixedPosition;
    [SerializeField] protected Vector3 _fixedPosition;
    [SerializeField] protected Vector3 _fixedRotation;

    protected GameObject _myPlayer;

    private SceneNumber _nextScene;
    private string _nextSceneRoomName;
    private RoomOptions _nextRoomOption;
    private bool _needSceneChange = false;
    private RoomOptions _defaultRoomOptions = new RoomOptions
    {
        PublishUserId = true,
        MaxPlayers = 30
    };

    private bool _joinRandomRoom = false;
    private Hashtable _expectedCustromRoomProperties = null;
    private byte _expectedMaxPlayers = 0;

    private bool _lobbyChangeRoom = false;

    protected virtual void Awake()
    {
        if (!_isInLobby)
        {
            GameObject player = PhotonNetwork.Instantiate(_playerPrefab.name, _playerSpawnPosition,
                Quaternion.Euler(_playerSpawnRotatinon), 0, null);
            BasicPlayerNetworking playerNetworking = player.GetComponent<BasicPlayerNetworking>();
            playerNetworking.photonView.RPC("SetNickname", RpcTarget.All, TempAccountDB.ID, TempAccountDB.Nickname);
            playerNetworking.SetMap(_mapType, _isFixedPosition, _fixedPosition, _fixedRotation);
            playerNetworking.CanvasSetting(_canvases);
            _myPlayer = player;
        }


    }

    public override void OnEnable()
    {
        base.OnEnable();

        Application.wantsToQuit -= PlayerOffline;
        Application.wantsToQuit += PlayerOffline;

    }
    public void ChangeLobby(SceneNumber sceneNumber)
    {
        ChangeLobby(sceneNumber, sceneNumber.ToString(), _defaultRoomOptions);
    }

    public void ChangeLobby(SceneNumber sceneNumber, bool isLobbyChange)
    {
        _lobbyChangeRoom = isLobbyChange;
        ChangeLobby(sceneNumber, sceneNumber.ToString(), _defaultRoomOptions);
    }

    public void ChangeLobby(SceneNumber sceneNumber, RoomOptions roomOption, bool joinRamdonRoom = false,
        Hashtable expectedCustomRoomProperties = null, byte expectedMaxPlayers = 0)
    {
        ChangeLobby(sceneNumber, sceneNumber.ToString(), roomOption, joinRamdonRoom, expectedCustomRoomProperties, expectedMaxPlayers);
    }

    public void ChangeLobby(SceneNumber sceneNumber, string roomName, RoomOptions roomOption, bool joinRamdonRoom = false,
        Hashtable expectedCustomRoomProperties = null, byte expectedMaxPlayers = 0)
    {
        _nextScene = sceneNumber;
        _nextSceneRoomName = roomName;
        _nextRoomOption = roomOption;
        _joinRandomRoom = joinRamdonRoom;
        if (_joinRandomRoom)
        {
            _expectedCustromRoomProperties = expectedCustomRoomProperties;
            _expectedMaxPlayers = expectedMaxPlayers;
        }

        _needSceneChange = true;

        OVRScreenFade.instance.FadeOut();

        PlayerControlManager.Instance.IsRayable = false;
        PlayerControlManager.Instance.IsMoveable = false;

        if (!_isInLobby)
        {
            PhotonNetwork.LeaveRoom();
        }
        else if(_lobbyChangeRoom)
        {
            Debug.Log("[LogOut] LobbyChanger LoadLevel On Lobby");
            PhotonNetwork.LoadLevel((int)_nextScene);
        }
        else
        {
            PhotonNetwork.JoinOrCreateRoom(roomName, _nextRoomOption, TypedLobby.Default);
        }
    }

    public override void OnConnectedToMaster()
    {
        if (_needSceneChange)
        {
            Debug.Log("[LogOut] LobbyChanger OnConnectedToMaster");

            if (_joinRandomRoom)
            {
                Debug.Log($"[LogOut] LobbyChanger {_defaultRoomOptions.CustomRoomPropertiesForLobby.ToStringFull()} {_defaultRoomOptions.CustomRoomProperties}");
                PhotonNetwork.JoinRandomOrCreateRoom(
                    expectedCustomRoomProperties: _expectedCustromRoomProperties,
                    expectedMaxPlayers: _expectedMaxPlayers,
                    roomOptions: _nextRoomOption);
            }
            else
            {
                PhotonNetwork.JoinLobby();
            }
        }
    }

    public override void OnJoinedLobby()
    {
        if (_needSceneChange)
        {
            if (_nextScene <= SceneNumber.StartRoom || _lobbyChangeRoom)
            {
                Debug.Log("[LogOut] LobbyChanger LoadLevel On Lobby");
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
    }

    public override void OnJoinedRoom()
    {
        if (_needSceneChange)
        {
            if (_joinRandomRoom)
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { ShootingServerManager.RoomPropertyKey, 1 } });
                PhotonNetwork.CurrentRoom.SetPropertiesListedInLobby(new string[] { ShootingServerManager.RoomPropertyKey });
            }
            Debug.Log($"[LogOut] LobbyChanger OnCreatedRoom {PhotonNetwork.CurrentRoom.Name} {PhotonNetwork.CurrentRoom.PropertiesListedInLobby.ToStringFull()}");
            PhotonNetwork.LoadLevel((int)_nextScene);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("[Server] Offline Update");

        MySqlSetting.UpdateValueByBase(Asset.EaccountdbColumns.Nickname, PhotonNetwork.NickName, Asset.EaccountdbColumns.IsOnline, 0);

    }

    private bool PlayerOffline()
    {
        try
        {
            Debug.Log("[Player] Offline Update");

            MySqlSetting.UpdateValueByBase(Asset.EaccountdbColumns.Nickname, PhotonNetwork.NickName, Asset.EaccountdbColumns.IsOnline, 0);

            return true;
        }
        catch (System.Exception error)
        {
            Debug.LogError(error);

            return false;
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("[LogOut] LobbyChanger OnJoinRoomFailed, Reconnecting with same name");
        PhotonNetwork.JoinOrCreateRoom(_nextSceneRoomName, _nextRoomOption, TypedLobby.Default);
    }

    public override void OnDisable()
    {
        base.OnDisable();

        Application.wantsToQuit -= PlayerOffline;

    }

    private void OnApplicationQuit()
    {
        Debug.Log("[Player] Offline Update");

        MySqlSetting.UpdateValueByBase(Asset.EaccountdbColumns.Nickname, PhotonNetwork.NickName, Asset.EaccountdbColumns.IsOnline, 0);
    }
}
