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

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Connection Fail... ReConnecting...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedLobby()
    {
        _infoText.text = "Lobby Joined!";
        ActiveButtons();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(_roomCount != roomList.Count)
        {
            _roomCount = roomList.Count;

            foreach(RoomInfo roominfo in roomList)
            {
                CreateRoom(roominfo.Name);
            }
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
        foreach(GameObject room in _roomList)
        {
            Destroy(room);
        }
        PhotonNetwork.LeaveLobby();
    }

    private RoomOptions _roomOption = new RoomOptions()
    {
        MaxPlayers = 1
    };
    private void CreateRoom()
    {
        ++_roomCount;
        CreateRoom(_roomCount.ToString());
    }
    private void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName, _roomOption, null, null);
        GameObject roomInfo = Instantiate(_roomPanel, _scrollViewContent.transform);
        roomInfo.GetComponent<Transform>().gameObject.GetComponent<Text>().text = _roomCount.ToString();
        _roomList.Add(roomInfo);
    }
}
