using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

using _UI = Defines.EPrivateRoomUIIndex;

public class JoinRoomUI : MonoBehaviourPunCallbacks
{
    [Header("Manager")]
    [SerializeField] PrivateRoomUIManager _uiManager;

    [Header("Button")]
    [SerializeField] Button _makeRoomButton;
    [SerializeField] Button _refreshButton;
    [SerializeField] Button _randomJoinButton;

    [Header("Text")]
    [SerializeField] RoomInfoTextUI[] _roomInfoTexts;

    private Dictionary<string, RoomInfo> _cachedRoomList = new Dictionary<string, RoomInfo>();
    
    private const int PAGE_ROOM_COUNT = 4;
    private int _pageCount = 0;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnEnable()
    {
        base.OnEnable();

        _makeRoomButton.onClick.RemoveListener(LoadMakeRoom);
        _makeRoomButton.onClick.AddListener(LoadMakeRoom);

        _refreshButton.onClick.RemoveListener(RefreshRoomList);
        _refreshButton.onClick.AddListener(RefreshRoomList);

        _randomJoinButton.onClick.RemoveListener(RandomJoin);
        _randomJoinButton.onClick.AddListener(RandomJoin);
    }

    private void LoadMakeRoom() => _uiManager.LoadUI(_UI.MAKE);

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Room List Update");
        RoomListUpdate(roomList);
    }

    private void RoomListUpdate(List<RoomInfo> roomList)
    {
        _pageCount = roomList.Count / PAGE_ROOM_COUNT + 1;

        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList)
            {
                _cachedRoomList.Remove(room.Name);
            }
            else
            {
                _cachedRoomList[room.Name] = room;
            }
        }

        SetRoomList();
    }

    private void SetRoomList()
    {
        foreach (RoomInfoTextUI info in _roomInfoTexts)
        {
            info.SetInfo("");
            info.SetLock(false);
        }

        foreach (KeyValuePair<string, RoomInfo> room in _cachedRoomList)
        {
            
        }

        //for(int i = 0; i < PAGE_ROOM_COUNT; ++i)
        //{
        //    _roomInfoTexts[i].SetInfo(_cachedRoomList.);
        //}
    }

    private void RefreshRoomList()
    {

    }

    private void RandomJoin()
    {

    }

    public override void OnDisable()
    {
        base.OnDisable();

        _makeRoomButton.onClick.RemoveListener(LoadMakeRoom);
        _refreshButton.onClick.RemoveListener(RefreshRoomList);
        _randomJoinButton.onClick.RemoveListener(RandomJoin);
    }
}
