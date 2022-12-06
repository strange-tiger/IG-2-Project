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

    // 다음 이동 씬 관련
    private bool _needSceneChange = false;                      // 서버 이동이 필요한
    private SceneNumber _nextScene;
    private string _nextSceneRoomName;
    private RoomOptions _nextRoomOption;
    private RoomOptions _defaultRoomOptions = new RoomOptions
    {
        PublishUserId = true,
        MaxPlayers = 30
    };

    private bool _joinRandomRoom = false;
    private Hashtable _expectedCustromRoomProperties = null;
    private byte _expectedMaxPlayers = 0;

    private bool _lobbyChangeRoom = false;                      // 서버에서 이동해야하는 경우인지

    protected virtual void Awake()
    {
        // PhotonNetwork 상 로비에 있을 경우 플레이어를 Instantiate하지 않음
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

    /// <summary>
    /// PhotonNetwork 상 서버나 룸을 특정 씬으로 이동하게 함
    /// </summary>
    /// <param name="sceneNumber">이동하려는 씬 번호. 해당 이름의 서버로 들어감</param>
    public void ChangeLobby(SceneNumber sceneNumber)
    {
        ChangeLobby(sceneNumber, sceneNumber.ToString(), _defaultRoomOptions);
    }

    /// <summary>
    /// PhotonNetwork 상 서버나 룸으로 이동하게 함
    /// </summary>
    /// <param name="sceneNumber">이동하려는 씬 번호.</param>
    /// <param name="isLobbyChange">로비에서 이동해야하는 씬인지 여부. true면 로비에서 해당 씬을 호출함</param>
    public void ChangeLobby(SceneNumber sceneNumber, bool isLobbyChange)
    {
        _lobbyChangeRoom = isLobbyChange;
        ChangeLobby(sceneNumber, sceneNumber.ToString(), _defaultRoomOptions);
    }

    /// <summary>
    /// PhotonNetwork 상 서버나 룸으로 이동하게 함
    /// </summary>
    /// <param name="sceneNumber">이동하려는 씬 번호</param>
    /// <param name="roomOption">입장하려는 룸의 조건</param>
    /// <param name="joinRamdonRoom">랜덤 룸인지. 랜덤 룸이라면 다음의 조건들 받아들여 특정 조건의 랜덤룸에 참여하거나 생성할 수 있도록 함. 방 이름도 자동으로 생성됨</param>
    /// <param name="expectedCustomRoomProperties">커스텀 룸의 프로퍼티</param>
    /// <param name="expectedMaxPlayers">기대 플레이어 수</param>
    public void ChangeLobby(SceneNumber sceneNumber, RoomOptions roomOption, bool joinRamdonRoom = false,
        Hashtable expectedCustomRoomProperties = null, byte expectedMaxPlayers = 0)
    {
        ChangeLobby(sceneNumber, sceneNumber.ToString(), roomOption, joinRamdonRoom, expectedCustomRoomProperties, expectedMaxPlayers);
    }

    /// <summary>
    /// PhotonNetwork 상 서버나 룸으로 이동하게 함
    /// </summary>
    /// <param name="sceneNumber">이동하려는 씬 번호</param>
    /// <param name="roomName">이동하려는 씬 이름(특정 상황에서는 해당 이름으로 룸을 만들지 않음)</param>
    /// <param name="roomOption">입장하려는 룸의 조건</param>
    /// <param name="joinRamdonRoom">랜덤 룸인지. 랜덤 룸이라면 다음의 조건들 받아들여 특정 조건의 랜덤룸에 참여하거나 생성할 수 있도록 함. 방 이름도 자동으로 생성됨</param>
    /// <param name="expectedCustomRoomProperties">커스텀 룸의 프로퍼티</param>
    /// <param name="expectedMaxPlayers">기대 플레이어 수</param>
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
