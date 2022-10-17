using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomChange : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject _canvas;
    
    [Header("Buttons")]
    [SerializeField] private Button[] _room;

    private void Start()
    {
        _room[0].onClick.AddListener(() => { GoRoom(); });
    }

    private void GoRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.CreateRoom(null);
    }

    public void OnJoinedRoom(int sceneNumber)
    {
        PhotonNetwork.LoadLevel(sceneNumber);
    }
}
