using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PhotonNetworkPractice : MonoBehaviourPunCallbacks
{
    [Header("Info Text")]
    [SerializeField] private TextMeshProUGUI _infoText;

    [Header("Buttons")]
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private Button _resetRoomListButton;

    [Header("Room List")]
    [SerializeField] private GameObject _roomPanel;
    [SerializeField] private GameObject _scrollViewContent;
    private List<GameObject> _roomList = new List<GameObject>();
    private int _roomCount = 0;

    private void Awake()
    {
        ResetButton();
        _infoText.text = "Connecting...";
        PhotonNetwork.ConnectUsingSettings();
    }

    private void ResetButton()
    {
        _createRoomButton.onClick.AddListener(() => { CreateRoom(); });
        _resetRoomListButton.onClick.AddListener(() => { ResetRoomList(); });

        DeactiveButtons();
    }

    private void ActiveButtons()
    {
        _createRoomButton.interactable = true;
        _resetRoomListButton.interactable = true;
    }

    private void DeactiveButtons()
    {
        _createRoomButton.interactable = false;
        _resetRoomListButton.interactable = false;
    }

    public override void OnConnectedToMaster()
    {
        _infoText.text = "Connected! Joinning Lobby...";
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        _infoText.text = "Lobby Joined!";
        ActiveButtons();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _infoText.text = "Room List Updated";
        RoomListUpdate(roomList);
    }

    private void RoomListUpdate(List<RoomInfo> roomList)
    {
        _roomCount = 0;
        foreach (GameObject room in _roomList)
        {
            Destroy(room);
        }

        foreach (RoomInfo roominfo in roomList)
        {
            AddRoomToList(roominfo.Name);
            _roomCount = int.Parse(roominfo.Name);
        }
    }

    public override void OnLeftLobby()
    {
        _infoText.text = "Reconnecting Lobby...";
        DeactiveButtons();
        PhotonNetwork.JoinLobby();
    }

    private void ResetRoomList()
    {
        DeactiveButtons();
        foreach (GameObject room in _roomList)
        {
            Destroy(room);
        }
        _infoText.text = "Resetting Room List...";
    }

    private RoomOptions _roomOption = new RoomOptions()
    {
        MaxPlayers = 1
    };
    private void CreateRoom()
    {
        ++_roomCount;
        AddRoomToList(_roomCount.ToString());
        PhotonNetwork.CreateRoom(_roomCount.ToString(), _roomOption, null, null);
    }
    private void AddRoomToList(string roomName)
    {
        GameObject roomInfo = Instantiate(_roomPanel, _scrollViewContent.transform);
        roomInfo.GetComponentInChildren<Text>().text = roomName;
        _roomList.Add(roomInfo);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log($"Created Room {PhotonNetwork.CurrentRoom.Name}");
        //PhotonNetwork.LeaveRoom();
        //PhotonNetwork.JoinLobby();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Fail to Created Room {_roomCount}");
    }
}
