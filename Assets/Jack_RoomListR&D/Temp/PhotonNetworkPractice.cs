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
    [SerializeField] private Button _exitRoomButton;

    [Header("Room List")]
    [SerializeField] private GameObject _roomPanel;
    [SerializeField] private GameObject _scrollViewContent;

    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
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
        _exitRoomButton.onClick.AddListener(() => { ExitRoom(); });

        SetInteractableOfAllButtons(false);

        _exitRoomButton.gameObject.SetActive(false);
    }

    private void SetInteractableOfAllButtons(bool value)
    {
        _createRoomButton.interactable = value;
        _resetRoomListButton.interactable = value;
        _exitRoomButton.interactable = value;
    }

    public override void OnConnectedToMaster()
    {
        _infoText.text = "Connected! Joinning Lobby...";
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        _infoText.text = "Lobby Joined!";
        SetInteractableOfAllButtons(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _infoText.text = "Room List Updated";
        Invoke("setInfoText", 0.5f);
        RoomListUpdate(roomList);
    }

    private void RoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log(roomList.Count);

        foreach (RoomInfo room in roomList)
        {
            if(room.RemovedFromList)
            {
                cachedRoomList.Remove(room.Name);
            }
            else
            {
                cachedRoomList[room.Name] = room;
            }
        }

        SetRoomList();
    }
    private void SetRoomList()
    {
        foreach(Transform roomPanel in _scrollViewContent.GetComponentsInChildren<Transform>())
        {
            if(roomPanel.gameObject == _scrollViewContent.gameObject)
            {
                continue;
            }

            Destroy(roomPanel.gameObject);
        }

        foreach(KeyValuePair<string, RoomInfo> room in cachedRoomList)
        {
            if(room.Value.RemovedFromList)
            {
                continue;
            }

            GameObject newRoomPanel = Instantiate(_roomPanel, _scrollViewContent.transform);
            newRoomPanel.GetComponentInChildren<Text>().text = room.Key;
        }

        _roomCount = cachedRoomList.Count;
    }

    public override void OnLeftLobby()
    {
        _infoText.text = "Reconnecting Lobby...";
        SetInteractableOfAllButtons(false);
        PhotonNetwork.JoinLobby();
    }

    private void setInfoText()
    {
        _infoText.text = "...";
    }

    private void ResetRoomList()
    {
        SetInteractableOfAllButtons(false);
        _infoText.text = "Resetting Room List...";

        SetRoomList();

        SetInteractableOfAllButtons(true);
        _infoText.text = "Done!";
        Invoke("setInfoText", 0.5f);
    }

    private void ExitRoom()
    {
        PhotonNetwork.LeaveRoom();
        _createRoomButton.gameObject.SetActive(true);
        _resetRoomListButton.gameObject.SetActive(true);
        _exitRoomButton.gameObject.SetActive(false);
    }

    private RoomOptions _roomOption = new RoomOptions()
    {
        MaxPlayers = 5
    };
    private void CreateRoom()
    {
        ++_roomCount;
        PhotonNetwork.CreateRoom(_roomCount.ToString(), _roomOption, null, null);
        _infoText.text = "Creating Room...";
        SetRoomList();
    }

    public override void OnCreatedRoom()
    {
        _infoText.text = $"Created Room {PhotonNetwork.CurrentRoom.Name}";
        _createRoomButton.gameObject.SetActive(false);
        _resetRoomListButton.gameObject.SetActive(false);
        _exitRoomButton.gameObject.SetActive(true);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Fail to Created Room {_roomCount}");
    }
}
