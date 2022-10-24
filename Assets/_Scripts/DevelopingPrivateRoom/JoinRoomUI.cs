using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

using _UI = Defines.EPrivateRoomUIIndex;
using Oculus.Platform.Models;
using System;

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

    public static event Action OnPageCountChanged;
    public static int PageCount
    {
        get => _pageCount;
        set
        {
            _pageCount = value;
            OnPageCountChanged.Invoke();
        }
    }
    private static int _pageCount = 0;
    
    private const int PAGE_ROOM_COUNT = 4;

    private Dictionary<string, RoomInfo> _cachedRoomList = new Dictionary<string, RoomInfo>();
    private List<RoomInfo[]> _roomPageList = new List<RoomInfo[]>();

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
        PageCount = roomList.Count / PAGE_ROOM_COUNT + 1;

        UpdateCachedRoomList(roomList);

        UpdateRoomPageList();

        ShowRoomList(0);
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
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
    }

    private void UpdateRoomPageList()
    {
        _roomPageList.Clear();

        for (int i = 0; i < _pageCount; ++i)
        {
            _roomPageList.Add(new RoomInfo[PAGE_ROOM_COUNT]);
        }

        int pageCount = 0;
        int roomCount = 0;
        
        foreach (KeyValuePair<string, RoomInfo> room in _cachedRoomList)
        {
            if (roomCount == PAGE_ROOM_COUNT)
            {
                roomCount = 0;
                ++pageCount;
            }
            _roomPageList[pageCount][roomCount] = room.Value;

            ++roomCount;
        }
        
    }

    private void ShowRoomList(int page)
    {
        foreach (RoomInfoTextUI info in _roomInfoTexts)
        {
            info.SetRoom("");
            info.SetInfo("");
            info.SetLock(false);
        }

        for (int i = 0; i < PAGE_ROOM_COUNT; ++i)
        {
            RoomInfo room = _roomPageList[page][i];

            _roomInfoTexts[i].SetRoom(room.Name);
            _roomInfoTexts[i].SetInfo($"{room.Name}\t{room.PlayerCount} / {room.MaxPlayers}");
            _roomInfoTexts[i].SetLock(room.CustomProperties["password"] != null);

            _roomInfoTexts[i].UpdateRoomInfo();
        }
    }

    public void ChangeRoomPage(int page)
    {
        ShowRoomList(page);
    }

    private void RefreshRoomList()
    {
        ShowRoomList(0);
    }

    private void RandomJoin()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisable()
    {
        base.OnDisable();

        _makeRoomButton.onClick.RemoveListener(LoadMakeRoom);
        _refreshButton.onClick.RemoveListener(RefreshRoomList);
        _randomJoinButton.onClick.RemoveListener(RandomJoin);
    }
}
