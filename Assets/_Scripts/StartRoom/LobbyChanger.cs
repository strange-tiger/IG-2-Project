using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyChanger : MonoBehaviourPunCallbacks
{
    private Defines.ESceneNumder _nextScene;
    [SerializeField] private bool _hasPlayer;
    [SerializeField] private OVRRaycaster[] _canvases;

    private void Awake()
    {
        if(!_hasPlayer)
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
        OVRScreenFade.instance.FadeOut();
        if(!_hasPlayer)
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
        PhotonNetwork.JoinLobby();
    }

    private RoomOptions _roomOptions = new RoomOptions
    {
        MaxPlayers = 30
    };
    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom(_nextScene.ToString(), _roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel((int)_nextScene);
    }
}
