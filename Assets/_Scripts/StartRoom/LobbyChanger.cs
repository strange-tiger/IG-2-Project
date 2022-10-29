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

    private void Awake()
    {
        if(!_isStartRoom)
        {
            GameObject player = PhotonNetwork.Instantiate("NewPlayer",new Vector3(0f, 1f, 3f),
                Quaternion.Euler(0f, 0f, 0f), 0, null);
            PlayerNetworking playerNetworking = player.GetComponent<PlayerNetworking>();
            playerNetworking.photonView.RPC("SetNickname", RpcTarget.All, TempAccountDB.ID, TempAccountDB.Nickname);
            playerNetworking.CanvasSetting(_canvases);
        }
    }

    public void ChangeLobby(Defines.ESceneNumder sceneNumber)
    {
        _nextScene = sceneNumber;
        _needSceneChange = true;
        OVRScreenFade.instance.FadeOut();
        if(!_isStartRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            PhotonNetwork.JoinOrCreateRoom(_nextScene.ToString(), _roomOptions, TypedLobby.Default);
        }
    }

    public override void OnConnectedToMaster()
    {
        if(_needSceneChange)
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
        if(_needSceneChange)
        {
            Debug.Log("[LogOut] LobbyChanger OnJoinedLobby");
            PhotonNetwork.JoinOrCreateRoom(_nextScene.ToString(), _roomOptions, TypedLobby.Default);
        }
    }

    public override void OnJoinedRoom()
    {
        if(_needSceneChange)
        {
            Debug.Log("[LogOut] LobbyChanger OnJoinedRoom");
            PhotonNetwork.LoadLevel((int)_nextScene);
        }
    }
}
