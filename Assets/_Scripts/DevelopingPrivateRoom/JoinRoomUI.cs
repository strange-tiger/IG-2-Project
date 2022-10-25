using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

using _UI = Defines.EPrivateRoomUIIndex;
using _PH = ExitGames.Client.Photon;
using _DB = Asset.MySql.MySqlSetting;

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

    private List<Dictionary<string, string>> _roomList = new List<Dictionary<string, string>>();
    private List<Dictionary<string, string>[]> _roomPage = new List<Dictionary<string, string>[]>();

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

#region Legacy
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
            Dictionary<string, string> room = _roomPage[page][i];

            _roomInfoTexts[i].SetRoom(room["UserID"]);
            _roomInfoTexts[i].SetInfo($"{room["DisplayName"]}\t{room["RoomNumber"]}");
            _roomInfoTexts[i].SetLock(room["Password"] != "");

            _roomInfoTexts[i].UpdateRoomInfo();
        }
    }
    
#endregion

    private void RoomListUpdate()
    {
        _roomList = _DB.GetRoomList();

        PageCount = _roomList.Count / PAGE_ROOM_COUNT + 1;

        UpdateRoomPageList(_roomList);
    }

    private void UpdateRoomPageList(List<Dictionary<string, string>> roomList)
    {
        _roomPage.Clear();

        for (int i = 0; i < _pageCount; ++i)
        {
            _roomPage.Add(new Dictionary<string, string>[PAGE_ROOM_COUNT]);
        }

        int pageCount = 0;
        int roomCount = 0;

        foreach (Dictionary<string, string> roomInfo in _roomList)
        {
            if (roomCount == PAGE_ROOM_COUNT)
            {
                roomCount = 0;
                ++pageCount;
            }
            _roomPage[pageCount][roomCount] = roomInfo;

            ++roomCount;
        }
    }

    public void ChangeRoomPage(int page)
    {
        ShowRoomList(page);
    }

    private void RefreshRoomList()
    {
        RoomListUpdate();
        ShowRoomList(0);
    }

    private static readonly _PH.Hashtable CUSTOM_ROOM_PROPERTIES_UNLOCKED = 
        new _PH.Hashtable() { { "password", "" } };
    private const int ANY_MAX_PLAYER = 0;
    private void RandomJoin()
    {
        try
        {
            PhotonNetwork.JoinRandomRoom(CUSTOM_ROOM_PROPERTIES_UNLOCKED, ANY_MAX_PLAYER);
        }
        catch
        {
            Debug.LogError("랜덤 매칭 실패");
        }
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        _DB.AddNewRoomInfo("", "", "", 0);
    }

    public override void OnDisable()
    {
        base.OnDisable();

        _makeRoomButton.onClick.RemoveListener(LoadMakeRoom);
        _refreshButton.onClick.RemoveListener(RefreshRoomList);
        _randomJoinButton.onClick.RemoveListener(RandomJoin);
    }
}
