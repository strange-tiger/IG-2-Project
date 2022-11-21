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
    private bool _needSceneChange = false;

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
        ChangeLobby(sceneNumber, sceneNumber.ToString());
    }

    public void ChangeLobby(SceneNumber sceneNumber, string roomName)
    {
        _nextSceneRoomName = roomName;
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
            Debug.LogError("[LobbyChanger] " + roomName);
            PhotonNetwork.JoinOrCreateRoom(roomName, _roomOptions, TypedLobby.Default);
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
            if (_nextScene <= SceneNumber.StartRoom)
            {
                PhotonNetwork.LoadLevel((int)_nextScene);

                return;
            }
            PhotonNetwork.JoinOrCreateRoom(_nextSceneRoomName, _roomOptions, TypedLobby.Default);
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
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            MySqlSetting.UpdateValueByBase(Asset.EaccountdbColumns.Nickname, newPlayer.NickName, Asset.EaccountdbColumns.IsOnline, 1);
        }
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {

        if (PhotonNetwork.IsMasterClient)
        {
            MySqlSetting.UpdateValueByBase(Asset.EaccountdbColumns.Nickname, otherPlayer.NickName, Asset.EaccountdbColumns.IsOnline, 0);
        }

    }
}
